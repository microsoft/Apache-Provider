#!/bin/sh
function usage {
   echo  "Usage:"
   echo  "-d: httpd.conf file location, default is /etc/httpd/conf/httpd.conf"
   echo  "-p: the port which we set. [80,81,82]"
   echo  "-s: the ssl port which we set. [443,444]"
   echo  "-expireSSL: create a expire ssl port"
   echo  "example: createHTTPPortforApache.sh -p 80,81"
}

function removeOldPorts {
    sed -i '/^<VirtualHost/i\<Ifmodule test\>' $confFileLocation
    sed -i '/^<\/VirtualHost/a\<\/Ifmodule\>' $confFileLocation
    sed -i 's/^Listen/#&/' $confFileLocation

}

function AddPort {
    echo "Listen $1"
    echo "<VirtualHost *:$1>" >> $2
    echo "ServerAdmin webmaster@dummy-host.example.com" >> $2
    echo "DocumentRoot /www/docs/dummy-host.example.com" >> $2
    echo "ServerName dummy-host.example.com" >> $2
    echo "ErrorLog logs/dummy-host.example.com-error_log" >> $2
    echo "CustomLog logs/dummy-host.example.com-access_log common" >> $2
    echo "</VirtualHost>" >> $2
}

function AddSSLPort {
    echo "Listen $1" >> $2
    echo "<VirtualHost _default_:$1>" >> $2
    echo "ErrorLog logs/ssl_error_log" >> $2
    echo "TransferLog logs/ssl_access_log" >> $2
    echo "LogLevel warn" >> $2
    echo "SSLEngine on" >> $2
    echo "SSLProtocol all -SSLv2" >> $2
    echo "SSLCipherSuite ALL:!ADH:!EXPORT:!SSLv2:RC4+RSA:+HIGH:+MEDIUM:+LOW" >> $2
    echo "SSLCertificateFile /etc/pki/tls/certs/localhost.crt" >> $2
    echo "SSLCertificateKeyFile /etc/pki/tls/private/localhost.key" >> $2
    echo "CustomLog logs/ssl_request_log common" >> $2
    echo "</VirtualHost>" >> $2
}

function createSSLExpirePort {
    date -s last-month
    ./createSSLCertification.sh
    hostname=`hostname`
    echo "Listen 443" >> $1
    echo "<VirtualHost _default_:443>" >> $1
    echo "ErrorLog logs/ssl_error_log" >> $1
    echo "TransferLog logs/ssl_access_log" >> $1
    echo "LogLevel warn" >> $1
    echo "SSLEngine on" >> $1
    echo "ServerName $hostname" >> $1
    echo "SSLCertificateFile /etc/ssl/crt/server.crt" >> $1
    echo "SSLCertificateKeyFile /etc/ssl/crt/server.key" >> $1
    echo "CustomLog logs/ssl_request_log common" >> $1
    echo "</VirtualHost>" >> $1
    service httpd restart
    date -s next-month
}

#Main
confFileLocation=/etc/httpd/conf/httpd.conf
optP=""
sslPort=""
expire="";
setLocation=""

while [ $# -ne 0 ]; do
   case "$1" in
	-d)
	    confFileLocation=$2
            setLocation=true;
	    shift 2;;
	-p)
	    optP=$2
	    shift 2;;
        -s)
	    sslPort=$2
	    shift 2;;
        -expireSSL)
            expire=true;
            shift 1;;
	-?)
	    usage
	    exit 0;;
	*)
	    usage
	    exit 0;;
    esac
done

#echo $confFileLocation
#echo $optP

if [ ! -f "$confFileLocation" ]; then
   echo "Didn't find conf file $confFileLocation"
   exit 0
fi

if [ ! -z "$expire" ]; then 
    if [ -z "$setLocation" ]; then
        echo "Please set ssl conf file location if want to create a expired ssl port"
        exit 0
    fi
    bakFile=$confFileLocation"_bak"
    #echo $bakFile

    cp $confFileLocation $bakFile

    removeOldPorts
    createSSLExpirePort $confFileLocation
    exit 0
fi

port[0]=`echo $optP | awk -F ',' '{print $1}'`
port[1]=`echo $optP | awk -F ',' '{print $2}'`

sslPorts[0]=`echo $sslPort | awk -F ',' '{print $1}'`
sslPorts[1]=`echo $sslPort | awk -F ',' '{print $2}'`


#echo ${port[0]}
#echo ${port[1]}

if [ -z "${port[0]}" ]; 
then if [ -z "${sslPorts[0]}" ]; then
        echo "Please add ports you want set"
        exit 0
    fi
else
   if [ ! -z "${sslPorts[0]}" ]; then
       echo "Couldn't both set -s and -p"
       exit 0
   fi
fi

bakFile=$confFileLocation"_bak"
#echo $bakFile

cp $confFileLocation $bakFile

removeOldPorts

if [ ! -z "${sslPorts[0]}" ];
then
	AddSSLPort ${sslPorts[0]} $confFileLocation
        if [ ! -z "${sslPorts[1]}" ]; then
             AddSSLPort ${sslPorts[1]} $confFileLocation
        fi
else
       AddPort ${port[0]} $confFileLocation
       if [ ! -z "${port[1]}" ]; then
           AddPort ${port[1]} $confFileLocation
       fi
fi
service httpd restart
exit 0
