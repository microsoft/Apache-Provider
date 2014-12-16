#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

function usage {
   echo  "Usage:"
   echo  "-d: httpd.conf file location, default is /etc/httpd/conf/httpd.conf for package and /usr/local/apache2/conf/httpd.conf for source"
   echo "-ssld: ssl.conf file location, default is /etc/httpd/conf.d/ssl.conf for package and /usr/local/apache2/conf/extra/httpd-ssl.conf for source"
   echo  "-p: the http port which we want to set, separate by "," , like 80,81"
   echo  "-s: the ssl port which we set, separate by ",", like 443,444"
   echo  "-expireSSL: create a expire ssl port"
   echo  "-nossl: Disable all ssl port"
   echo  "-2sslc: create 2 SSL ports with different certificate"
   echo  "example:"
   echo  "1. create a http port : sh createPortforApache.sh -p 80"
   echo  "2. create a ssl port : sh createPortforApache.sh -s 443"
   echo  "3. create 2 http ports : sh createPortforApache.sh -p 80,81"
   echo  "4. create 2 http ports : sh createPortforApache.sh -s 443,444"
   echo  "5. create a http port and a ssl port: sh createPortforApache -p 80 -s 443"
   echo  "6. create a expire certifaicate SSL port: sh createPortforApache -expireSSL"
   echo  "7. Diable all ssl port: sh createPortforApache -nossl"
   echo  "8. create 2 ssl ports with different certificate: sh createPortforApache -2sslc"
}

function removeOldPorts {
    sed -i '/^<VirtualHost/i\<Ifmodule test\>' $1
    sed -i '/^<\/VirtualHost/a\<\/Ifmodule\>' $1
    sed -i 's/^Listen/#&/' $1

}

ServerAdmin=root@localhost
DocumentRoot=/var/www/html
ServerName=localhost
ErrorLog=logs/localhost-error_log
CustomLog="logs/localhost-custom_log common"
TransferLog=logs/localhost-access_log
SSLCertificateFile=/etc/ssl/crt/server.crt
SSLCertificateKeyFile=/etc/ssl/crt/server.key
CustomSSLCertificateFile=/tmp/localhost.crt
CustomSSLCertificateKeyFile=/tmp/localhost.key
CustomSSLCertificateFile1=/tmp/localhost1.crt
CustomSSLCertificateKeyFile1=/tmp/localhost1.key

function AddPort {
    echo "Listen $1" >> $2
    echo "<VirtualHost *:$1>" >> $2
    echo "ServerAdmin $ServerAdmin" >> $2
    echo "DocumentRoot $DocumentRoot" >> $2
    echo "ServerName $ServerName" >> $2
    echo "ErrorLog $ErrorLog" >> $2
    echo "CustomLog $CustomLog" >> $2
    echo "Transferlog $TransferLog" >> $2
    echo "</VirtualHost>" >> $2
}

function AddSSLPort {
    echo "Listen $1" >> $2
    echo "<VirtualHost _default_:$1>" >> $2
    echo "ServerAdmin $ServerAdmin" >> $2
    echo "DocumentRoot $DocumentRoot" >> $2
    echo "ServerName $ServerName" >> $2
    echo "ErrorLog $ErrorLog" >> $2
    echo "TransferLog $TransferLog" >> $2
    echo "LogLevel warn" >> $2
    echo "SSLEngine on" >> $2
    echo "SSLProtocol all -SSLv2" >> $2
    echo "SSLCipherSuite ALL:!ADH:!EXPORT:!SSLv2:RC4+RSA:+HIGH:+MEDIUM:+LOW" >> $2
    echo "SSLCertificateFile $SSLCertificateFile" >> $2
    echo "SSLCertificateKeyFile $SSLCertificateKeyFile" >> $2
    echo "CustomLog $CustomLog" >> $2
    echo "</VirtualHost>" >> $2
}

function restartApacheService {
     if [ "$isFromSource" = "true" ]; then
        /usr/local/apache2/bin/httpd -k restart
     else 
     if [ "$isFromPackage" = "true" ]; then
     	service httpd restart
     fi
     fi
}

function createSSLPort {
    hostname=`hostname`
    echo "Listen $2" >> $1
    echo "<VirtualHost _default_:443>" >> $1
    echo "ErrorLog $ErrorLog" >> $1
    echo "TransferLog $TransferLog" >> $1
    echo "LogLevel warn" >> $1
    echo "SSLEngine on" >> $1
    echo "ServerName $hostname" >> $1
    echo "SSLCertificateFile $3" >> $1
    echo "SSLCertificateKeyFile $4" >> $1
    echo "CustomLog $CustomLog" >> $1
    echo "</VirtualHost>" >> $1
}

function createSSLExpirePort {
    date -s last-month
    sh ./createSSLCertification.sh
    createSSLPort $1 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
    restartApacheService  

    date -s next-month
}

function create2SSLAndCertificate {
   sh ./createSSLCertification.sh
   createSSLPort $1 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
   
   sh ./createSSLCertification.sh -c $CustomSSLCertificateFile1 -k $CustomSSLCertificateKeyFile1
   createSSLPort $1 444 $CustomSSLCertificateFile1 $CustomSSLCertificateKeyFile1

}  
	
function cleanSSLBakFile {
    bakFile=$g_defaultSSLConfFileLocation"_bak"
    cp $g_defaultSSLConfFileLocation $g_defaultSSLConfFileLocation"_test"
    cp $bakFile $g_defaultSSLConfFileLocation
    rm $bakFile

}

function cleanHTTPBakFile {
    bakFile=$g_defaultHTTPDConfFileLocation"_bak"
    cp $g_defaultHTTPDConfFileLocation $g_defaultHTTPDConfFileLocation"_test"
    cp $bakFile $g_defaultHTTPDConfFileLocation
    rm $bakFile

}


#Main
httpPort=""
sslPort=""
expire=""
setHTTPLocation=false
setSSLLocation=false
needCreateHTTPPorts=false
needCreateSSLPorts=false

g_defaultHTTPDConfFileLocation=/etc/httpd/conf/httpd.conf
g_defaultSSLConfFileLocation=/etc/httpd/conf.d/ssl.conf
isFromPackage=false
isFromSource=false
isNoSSL=false;
isTwoSSLCertificate=false;

if [ -f "$g_defaultHTTPDConfFileLocation" ]; then
	isFromPackage=true
else 
   if [ -f "/usr/local/apache2/conf/httpd.conf" ]; then
	isFromSource=true
	g_defaultHTTPDConfFileLocation=/usr/local/apache2/conf/httpd.conf
	g_defaultSSLConfFileLocation=/usr/local/apache2/conf/extra/httpd-ssl.conf
   fi
fi

while [ $# -ne 0 ]; do
   case "$1" in
	-d)
	    g_defaultHTTPDConfFileLocation=$2
            setHTTPLocation=true
	    shift 2;;
	-ssld)
	    g_defaultSSLConfFileLocation=$2
	    setSSLLocation=true
	    shift 2;;
	-p)
	    httpPort=$2
	    needCreateHTTPPorts=true
	    shift 2;;
        -s)
	    sslPort=$2
    	    needCreateSSLPorts=true
	    shift 2;;
        -expireSSL)
            expire=true;
            shift 1;;
	-nossl)
	    isNoSSL=true;
	    shift 1;;
	-2sslc)
	    isTwoSSLCertificate=true;
	    shift 1;;
	-?)
	    usage
	    exit 0;;
	*)
	    usage
	    exit 0;;
    esac
done

if [ ! -f "$g_defaultHTTPDConfFileLocation" ] && [ "$needCreateHTTPPorts" = "true" ]; then
        echo "The httpd.conf file $g_defaultHTTPDConfFileLocation didn't exist"
	exit 1
fi

if [ ! -f "$g_defaultSSLConfFileLocation" ] && [ "$needCreateSSLPorts" = "true" ]; then
	echo "The ssl.conf file $g_defaultSSLConfFileLocation didn't exist"
	exit 1
fi

#create expire ssl port
if [ ! -z "$expire" ]; then 
    if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
        echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
        exit 1
    fi
    bakFile=$g_defaultSSLConfFileLocation"_bak"

    cp $g_defaultSSLConfFileLocation $bakFile

    removeOldPorts $g_defaultSSLConfFileLocation
    createSSLExpirePort $g_defaultSSLConfFileLocation
    
    cleanSSLBakFile
    exit 0
fi

#Disable all ssl port
if [ "$isTwoSSLCertificate" = "true" ]; then
    if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
        echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
        exit 1
    fi
    bakFile=$g_defaultSSLConfFileLocation"_bak"

    cp $g_defaultSSLConfFileLocation $bakFile

    removeOldPorts $g_defaultSSLConfFileLocation
    create2SSLAndCertificate $g_defaultSSLConfFileLocation
    restartApacheService
    cleanSSLBakFile
    exit 0
fi

#create 2 ssl with different certification
if [ "$isNoSSL" = "true" ]; then
    if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
        echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
        exit 1
    fi
    bakFile=$g_defaultSSLConfFileLocation"_bak"
        
    cp $g_defaultSSLConfFileLocation $bakFile
            
    removeOldPorts $g_defaultSSLConfFileLocation
    restartApacheService
    cleanSSLBakFile
    exit 0
fi



#Create HTTPPorts
if [ "$needCreateHTTPPorts" = "true" ] && [ "$needCreateSSLPorts" = "false" ]; then
	port[0]=`echo $httpPort | awk -F ',' '{print $1}'`
	port[1]=`echo $httpPort | awk -F ',' '{print $2}'`
	if [ -z "${port[0]}" ]; then
	    echo "Please add ports you want set"
	    exit 1
	fi
	bakFile=$g_defaultHTTPDConfFileLocation"_bak"
	cp $g_defaultHTTPDConfFileLocation $bakFile
	removeOldPorts $g_defaultHTTPDConfFileLocation
	AddPort ${port[0]} $g_defaultHTTPDConfFileLocation
	if [ ! -z "$port[1]" ]; then
		AddPort ${port[1]} $g_defaultHTTPDConfFileLocation
	fi
	restartApacheService
	cleanHTTPBakFile
	exit 0
fi
	
#Create SSLPorts
if [ "$needCreateSSLPorts" = "true" ] && [ "$needCreateHTTPPorts" = "false" ]; then
        port[0]=`echo $sslPort | awk -F ',' '{print $1}'`
        port[1]=`echo $sslPort | awk -F ',' '{print $2}'`
        if [ -z "${port[0]}" ]; then
            echo "Please add ports you want set"
            exit 1
        fi
        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
        AddSSLPort ${port[0]} $g_defaultSSLConfFileLocation
        if [ ! -z "$port[1]" ]; then
                AddSSLPort ${port[1]} $g_defaultSSLConfFileLocation
        fi
        restartApacheService
	cleanSSLBakFile
        exit 0
fi

#Create HTTP and SSL Ports
if [ "$needCreateSSLPorts" = "true" ] && [ "$needCreateHTTPPorts" = "true" ]; then
        
	port[0]=`echo $httpPort | awk -F ',' '{print $1}'`
        port[1]=`echo $httpPort | awk -F ',' '{print $2}'`

	sport[0]=`echo $sslPort | awk -F ',' '{print $1}'`
        sport[1]=`echo $sslPort | awk -F ',' '{print $2}'`
        if [ -z "${sport[0]}" ]; then
            echo "Please add ports you want set"
            exit 1
        fi
	
	if [ -z "${port[0]}" ]; then
            echo "Please add ports you want set"
            exit 1
        fi

        sslbakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $sslbakFile
        removeOldPorts $g_defaultSSLConfFileLocation
        AddSSLPort ${sport[0]} $g_defaultSSLConfFileLocation
        
        httpbakFile=$g_defaultHTTPDConfFileLocation"_bak"
        cp $g_defaultHTTPDConfFileLocation $httpbakFile
        removeOldPorts $g_defaultHTTPDConfFileLocation
        AddPort ${port[0]} $g_defaultHTTPDConfFileLocation
	

        restartApacheService
	cleanSSLBakFile
	cleanHTTPBakFile
        exit 0
fi

exit 0
