#!/bin/bash
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
   echo  "-1sslc: create 1 ssl port with custom certificate"
   echo  "-breakConf" break conf file. Please use -revertConf after the operate.
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
#    sed -i 's/^Listen/#&/' $1

}

ServerAdmin=root@localhost
DocumentRoot=/var/www/html
ServerName=`hostname`
ErrorLog=logs/localhost-error_log
CustomLog="logs/localhost-custom_log common"
TransferLog=logs/localhost-access_log
SSLCertificateFile=/etc/ssl/crt/server.crt
SSLCertificateKeyFile=/etc/ssl/crt/server.key
CustomSSLCertificateFile=localhost.crt
CustomSSLCertificateKeyFile=localhost.key
CustomSSLCertificateFile1=localhost1.crt
CustomSSLCertificateKeyFile1=localhost1.key
DebTempDir=/etc/apache2/tmp

function AddPort {
    #echo "Listen $1" >> $2
    echo "<VirtualHost 0.0.0.0:$1>" >> $2
    echo "ServerAdmin $ServerAdmin" >> $2
    echo "DocumentRoot $DocumentRoot" >> $2
    echo "ServerName $ServerName" >> $2
    echo "ErrorLog $ErrorLog" >> $2
    echo "CustomLog $CustomLog" >> $2
    echo "Transferlog $TransferLog" >> $2
    echo "</VirtualHost>" >> $2
}

function AddSSLPort {
    #echo "Listen $1" >> $2
    echo "<VirtualHost 255.255.255.255:$1>" >> $2
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

    if [ "$isDEB" = "true" ]; then
	service apache2 restart
    fi
    if [ "$isSles" = "true" ]; then
        service apache2 restart
    fi
    sleep 2
}

function createSSLPort {
    hostname=`hostname`
    #echo "Listen $2" >> $1
    echo "<VirtualHost 255.255.255.255:$2>" >> $1
    echo "ErrorLog $ErrorLog" >> $1
    echo "TransferLog $TransferLog" >> $1
    echo "LogLevel warn" >> $1
    echo "SSLEngine on" >> $1
    echo "ServerName $hostname" >> $1
    echo "SSLCertificateFile /etc/ssl/crt/$3" >> $1
    echo "SSLCertificateKeyFile /etc/ssl/crt/$4" >> $1
    echo "CustomLog $CustomLog" >> $1
    echo "</VirtualHost>" >> $1
}

function createHTTPPort {
    port[0]=`echo $httpPort | awk -F ',' '{print $1}'`
    port[1]=`echo $httpPort | awk -F ',' '{print $2}'`
    if [ -z "${port[0]}" ]; then
        echo "Please add ports you want set"
        exit 1
    fi

    if [ "$isDEB" = "true" ]; then
        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultHTTPDConfFileLocation/* $DebTempDir/
        rm $g_defaultHTTPDConfFileLocation/*
        touch $g_defaultHTTPDConfFileLocation/my.conf
        mkdir $g_defaultHTTPDConfFileLocation/../logs
        touch $ErrorLog
        touch $CustomLog
        touch $TransferLog
        AddPort ${port[0]} $g_defaultHTTPDConfFileLocation/my.conf 
	if [ ! -z "${port[1]}" ]; then
                AddPort ${port[1]} $g_defaultHTTPDConfFileLocation/my.conf
        fi

        restartApacheService
        mv $DebTempDir/* $g_defaultHTTPDConfFileLocation/
        mv $g_defaultHTTPDConfFileLocation/my.conf $DebTempDir/
    else
        bakFile=$g_defaultHTTPDConfFileLocation"_bak"
        cp $g_defaultHTTPDConfFileLocation $bakFile
        removeOldPorts $g_defaultHTTPDConfFileLocation
	AddPort ${port[0]} $g_defaultHTTPDConfFileLocation
	if [ ! -z "${port[1]}" ]; then
                AddPort ${port[1]} $g_defaultHTTPDConfFileLocation
        fi
        restartApacheService
        cleanHTTPBakFile
    fi
}

function createHTTPSPort {
    port[0]=`echo $sslPort | awk -F ',' '{print $1}'`
    port[1]=`echo $sslPort | awk -F ',' '{print $2}'`
    if [ -z "${port[0]}" ]; then 
        echo "Please add ports you want set"
        exit 1  
    fi

    if [ "$isDEB" = "true" ]; then 
        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultSSLConfFileLocation/* $DebTempDir/
        rm $g_defaultSSLConfFileLocation/*
        touch $g_defaultSSLConfFileLocation/myssl.conf
        mkdir $g_defaultSSLDConfFileLocation/../logs
        touch $ErrorLog
        touch $CustomLog
        touch $TransferLog
        AddSSLPort ${port[0]} $g_defaultSSLConfFileLocation/myssl.conf 
        if [ ! -z "${port[1]}" ]; then 
                AddSSLPort ${port[1]} $g_defaultSSLConfFileLocation/myssl.conf
        fi      

        restartApacheService
        mv $DebTempDir/* $g_defaultSSLConfFileLocation/
        mv $g_defaultSSLConfFileLocation/myssl.conf $DebTempDir/
    else    
        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
        AddSSLPort ${port[0]} $g_defaultSSLConfFileLocation
        if [ ! -z "${port[1]}" ]; then 
                AddSSLPort ${port[1]} $g_defaultSSLConfFileLocation
        fi      
        restartApacheService
        cleanSSLBakFile
    fi
}

function createHTTPAndHTTPSPort {

    port[0]=`echo $httpPort | awk -F ',' '{print $1}'`
    port[1]=`echo $httpPort | awk -F ',' '{print $2}'`
    sport[0]=`echo $sslPort | awk -F ',' '{print $1}'`
    sport[1]=`echo $sslPort | awk -F ',' '{print $2}'`

    if [ -z "${port[0]}" ]; then
        echo "Please add ports you want set"
        exit 1
    fi
    if [ -z "${sport[0]}" ]; then
        echo "Please add ports you want set"
        exit 1
    fi

    if [ "$isDEB" = "true" ]; then
        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultSSLConfFileLocation/* $DebTempDir/
        rm $g_defaultSSLConfFileLocation/*
        touch $g_defaultSSLConfFileLocation/myssl.conf
        mkdir $g_defaultSSLDConfFileLocation/../logs
        touch $ErrorLog
        touch $CustomLog
        touch $TransferLog
        AddSSLPort ${sport[0]} $g_defaultSSLConfFileLocation/myssl.conf
        touch $g_defaultHTTPDConfFileLocation/my.conf
	AddPort ${port[0]} $g_defaultHTTPDConfFileLocation/my.conf 
        restartApacheService
        mv $DebTempDir/* $g_defaultSSLConfFileLocation/
        mv $g_defaultSSLConfFileLocation/myssl.conf $DebTempDir/
	mv $g_defaultHTTPDConfFileLocation/my.conf $DebTempDir/
    else
	sslbakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $sslbakFile
        removeOldPorts $g_defaultSSLConfFileLocation
	#echo "Listen ${sport[0]}" >> $g_defaultSSLConfFileLocation
        AddSSLPort ${sport[0]} $g_defaultSSLConfFileLocation

        httpbakFile=$g_defaultHTTPDConfFileLocation"_bak"
        cp $g_defaultHTTPDConfFileLocation $httpbakFile
        removeOldPorts $g_defaultHTTPDConfFileLocation
        AddPort ${port[0]} $g_defaultHTTPDConfFileLocation


        restartApacheService
        cleanSSLBakFile
        cleanHTTPBakFile

    fi
}

function createSSLExpirePort {
    
    if [ "$isDEB" = "true" ]; then
	if [ ! -d "$g_defaultSSLConfFileLocation" ]; then
	   echo "Could not fild ssl conf file under $g_defaultSSLConfFileLocation"
	   exit 1
	fi

	mkdir $DebTempDir
	rm $DebTempDir/*
	cp $g_defaultSSLConfFileLocation/* $DebTempDir/
	rm $g_defaultSSLConfFileLocation/*
	touch $g_defaultSSLConfFileLocation/myssl.conf
	mkdir $g_defaultSSLConfFileLocation/../logs
	touch $ErrorLog
	touch $CustomLog
	touch $TransferLog
	date -s last-month
        sh /tmp/createSSLCertification.sh
        createSSLPort $g_defaultSSLConfFileLocation/myssl.conf 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
        restartApacheService
        date -s next-month
	mv $DebTempDir/* $g_defaultSSLConfFileLocation/
	mv $g_defaultSSLConfFileLocation/myssl.conf $DebTempDir/
    else
	if [ ! -f "$g_defaultSSLConfFileLocation" ]; then 
            echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
            exit 1  
        fi

        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
	date -s last-month
    	sh /tmp/createSSLCertification.sh
    	createSSLPort $g_defaultSSLConfFileLocation 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
    	restartApacheService
    	date -s next-month
        cleanSSLBakFile
    fi

}

function createNoSSLPort {

    if [ "$isDEB" = "true" ]; then
        if [ ! -d "$g_defaultSSLConfFileLocation" ]; then
           echo "Could not fild ssl conf file under $g_defaultSSLConfFileLocation"
           exit 1
        fi

        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultSSLConfFileLocation/* $DebTempDir/
        rm $g_defaultSSLConfFileLocation/*
        restartApacheService
        mv $DebTempDir/* $g_defaultSSLConfFileLocation/
    else
        if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
            echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
            exit 1
        fi

        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
        restartApacheService
        cleanSSLBakFile
    fi

}

function create2SSLAndCertificate {

    if [ "$isDEB" = "true" ]; then
        if [ ! -d "$g_defaultSSLConfFileLocation" ]; then
           echo "Could not fild ssl conf file under $g_defaultSSLConfFileLocation"
           exit 1
        fi

        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultSSLConfFileLocation/* $DebTempDir/
        rm $g_defaultSSLConfFileLocation/*
	touch $g_defaultSSLConfFileLocation/myssl.conf
        mkdir $g_defaultSSLConfFileLocation/../logs
        touch $ErrorLog
        touch $CustomLog
        touch $TransferLog
        sh /tmp/createSSLCertification.sh
        createSSLPort $g_defaultSSLConfFileLocation/myssl.conf 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
	sh /tmp/createSSLCertification.sh -c $CustomSSLCertificateFile1 -k $CustomSSLCertificateKeyFile1
        createSSLPort $g_defaultSSLConfFileLocation/myssl.conf 444 $CustomSSLCertificateFile1 $CustomSSLCertificateKeyFile1
        restartApacheService
        mv $DebTempDir/* $g_defaultSSLConfFileLocation/
        mv $g_defaultSSLConfFileLocation/myssl.conf $DebTempDir/
    else
        if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
            echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
            exit 1
        fi

        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
	sh /tmp/createSSLCertification.sh
	createSSLPort $g_defaultSSLConfFileLocation 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile

        sh /tmp/createSSLCertification.sh -c $CustomSSLCertificateFile1 -k $CustomSSLCertificateKeyFile1
        createSSLPort $g_defaultSSLConfFileLocation 444 $CustomSSLCertificateFile1 $CustomSSLCertificateKeyFile1
        restartApacheService
        cleanSSLBakFile
    fi
}  

function breakConfFile {
   if [ "$isDEB" = "true" ]; then
        if [ ! -d "$g_defaultHTTPDConfFileLocation" ]; then
           echo "Could not fild conf file under $g_defaultHTTPDConfFileLocation"
           exit 1
        fi

        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultHTTPDConfFileLocation/* $DebTempDir/
        rm $g_defaultHTTPDConfFileLocation/*
        touch $g_defaultHTTPDConfFileLocation/my.conf
	echo "ErrorSign" >> $g_defaultHTTPDConfFileLocation/my.conf
    else
        if [ ! -f "$g_defaultHTTPDConfFileLocation" ]; then
            echo "Could not find conf under $g_defaultHTTPDConfFileLocation"
            exit 1
        fi

        bakFile=$g_defaultHTTPDConfFileLocation"_bak"
        cp $g_defaultHTTPDConfFileLocation $bakFile
	echo "ErrorSign" >> $g_defaultHTTPDConfFileLocation
    fi

}

function revertConfFile {
   if [ "$isDEB" = "true" ]; then
        if [ ! -d "$g_defaultHTTPDConfFileLocation" ]; then
           echo "Could not fild conf file under $g_defaultHTTPDConfFileLocation"
           exit 1
        fi

	rm $g_defaultHTTPDConfFileLocation/*
	cp $DebTempDir/* $g_defaultHTTPDConfFileLocation/
	rm $DebTempDir/*
    else
        if [ ! -f "$g_defaultHTTPDConfFileLocation" ]; then
            echo "Could not find conf under $g_defaultHTTPDConfFileLocation"
            exit 1
        fi

        bakFile=$g_defaultHTTPDConfFileLocation"_bak"
        cp $bakFile $g_defaultHTTPDConfFileLocation
	rm $bakFile
    fi
	
}

function create1SSLAndCertificate {
   if [ "$isDEB" = "true" ]; then
        if [ ! -d "$g_defaultSSLConfFileLocation" ]; then
           echo "Could not fild ssl conf file under $g_defaultSSLConfFileLocation"
           exit 1
        fi

        mkdir $DebTempDir
        rm $DebTempDir/*
        cp $g_defaultSSLConfFileLocation/* $DebTempDir/
        rm $g_defaultSSLConfFileLocation/*
        touch $g_defaultSSLConfFileLocation/myssl.conf
        mkdir $g_defaultSSLConfFileLocation/../logs
        touch $ErrorLog
        touch $CustomLog
        touch $TransferLog
        sh /tmp/createSSLCertification.sh
        createSSLPort $g_defaultSSLConfFileLocation/myssl.conf 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile
        restartApacheService
        mv $DebTempDir/* $g_defaultSSLConfFileLocation/
        mv $g_defaultSSLConfFileLocation/myssl.conf $DebTempDir/
    else
        if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
            echo "Could not find ssl.conf under $g_defaultSSLConfFileLocation"
            exit 1
        fi

        bakFile=$g_defaultSSLConfFileLocation"_bak"
        cp $g_defaultSSLConfFileLocation $bakFile
        removeOldPorts $g_defaultSSLConfFileLocation
        sh /tmp/createSSLCertification.sh
        createSSLPort $g_defaultSSLConfFileLocation 443 $CustomSSLCertificateFile $CustomSSLCertificateKeyFile

        restartApacheService
        cleanSSLBakFile
    fi
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
isOneSSLCertificate=false;
isDEB=false
isSles=false

if [ -f "$g_defaultHTTPDConfFileLocation" ]; then
	isFromPackage=true
   	if [ ! -f "$g_defaultSSLConfFileLocation" ]; then
	    g_defaultSSLConfFileLocation=/etc/httpd/conf/extra/httpd-ssl.conf
	fi
else 
   if [ -f "/usr/local/apache2/conf/httpd.conf" ]; then
	isFromSource=true
	g_defaultHTTPDConfFileLocation=/usr/local/apache2/conf/httpd.conf
	g_defaultSSLConfFileLocation=/usr/local/apache2/conf/extra/httpd-ssl.conf
else
   if [ -f "/etc/apache2/apache2.conf" ]; then
	isFromPackage=true
	g_defaultHTTPDConfFileLocation=/etc/apache2/sites-enabled
        g_defaultSSLConfFileLocation=/etc/apache2/sites-enabled
	isDEB=true
	SSLCertificateFile=/etc/ssl/certs/ssl-cert-snakeoil.pem
	SSLCertificateKeyFile=/etc/ssl/private/ssl-cert-snakeoil.key
else 
   if [ -f "/etc/apache2/httpd.conf" ]; then
	isFromPackeage=true
	g_defaultHTTPDConfFileLocation=/etc/apache2/vhosts.d/vhost.conf
        g_defaultSSLConfFileLocation=/etc/apache2/vhosts.d/vhost-ssl.conf
        SSLCertificateFile=/etc/apache2/ssl.crt/server.crt
        SSLCertificateKeyFile=/etc/apache2/ssl.key/server.key
	isSles=true

   fi
   fi
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
            expire=true
            createSSLExpirePort 	
            exit 0
            shift 1;;
	-nossl)
	    isNoSSL=true
	    createNoSSLPort
	    exit 0
	    shift 1;;
	-2sslc)
	    isTwoSSLCertificate=true
	    create2SSLAndCertificate
	    exit 0
	    shift 1;;
	-1sslc)
	    isOneSSLCertificate=true
	    create1SSLAndCertificate
	    exit 0
	    shift 1;;
	-breakConf)
	    breakConfFile
	    exit 0
	    shift 1;;
	-revertConf)
	   revertConfFile
	   exit 0
	   shift 1;;
	-?)
	    usage
	    exit 0;;
	*)
	    usage
	    exit 0;;
    esac
done

if [ "$isDEB" = "true" ]; then
	if [ ! -d "$g_defaultHTTPDConfFileLocation" ] && [ "$needCreateHTTPPorts" = "true" ]; then
	    echo "Could not find path $g_defaultHTTPConfFileLocation"
	    exit 1
	fi
	if [ ! -d "$g_defaultSSLConfFileLocation" ] && [ "$needCreateSSLPorts" = "true" ]; then
            echo "Could not find path $g_defaultHTTPConfFileLocation"
            exit 1
        fi
else

	if [ ! -f "$g_defaultHTTPDConfFileLocation" ] && [ "$needCreateHTTPPorts" = "true" ]; then
            echo "The httpd.conf file $g_defaultHTTPDConfFileLocation didn't exist"
	    exit 1
	fi

	if [ ! -f "$g_defaultSSLConfFileLocation" ] && [ "$needCreateSSLPorts" = "true" ]; then
	    echo "The ssl.conf file $g_defaultSSLConfFileLocation didn't exist"
	    exit 1
	fi
fi

#Create HTTPPorts
if [ "$needCreateSSLPorts" = "false" ] && [ "$needCreateHTTPPorts" = "true" ]; then
	createHTTPPort
	exit 0
fi
	
#Create SSLPorts
if [ "$needCreateSSLPorts" = "true" ] && [ "$needCreateHTTPPorts" = "false" ]; then
        createHTTPSPort
        exit 0
fi

#Create HTTP and SSL Ports
if [ "$needCreateSSLPorts" = "true" ] && [ "$needCreateHTTPPorts" = "true" ]; then
	createHTTPAndHTTPSPort
        exit 0
fi
