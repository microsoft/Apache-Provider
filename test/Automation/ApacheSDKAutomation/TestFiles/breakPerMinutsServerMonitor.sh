#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

function cleanHTTPBakFile {
    bakFile=$g_defaultHTTPDConfFileLocation"_bak"
    cp $g_defaultHTTPDConfFileLocation $g_defaultHTTPDConfFileLocation"_test"
    cp $bakFile $g_defaultHTTPDConfFileLocation
    rm $bakFile

	if [ -f "$g_frompackagelocation" ]; then
		sed 's/AllowOverride All/AllowOverride None/g' $g_frompackagelocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_frompackagelocation;
		sed 's/AllowOverride All/AllowOverride none/g' $g_frompackagelocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_frompackagelocation;
	fi
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


#Main
g_defaultHTTPDConfFileLocation=/etc/httpd/conf/httpd.conf
g_frompackagelocation=/etc/apache2/sites-enabled/000-default
DocumentRoot=/var/www/html/
isFromPackage=false
isFromSource=false
isDEB=false
isSles=false

if [ -f "$g_defaultHTTPDConfFileLocation" ]; then
        isFromPackage=true
else
   if [ -f "/usr/local/apache2/conf/httpd.conf" ]; then
        isFromSource=true
        g_defaultHTTPDConfFileLocation=/usr/local/apache2/conf/httpd.conf
else
   if [ -f "/etc/apache2/apache2.conf" ]; then
        isFromPackage=true
        g_defaultHTTPDConfFileLocation=/etc/apache2/apache2.conf
        isDEB=true
else
   if [ -f "/etc/apache2/httpd.conf" ]; then
        isFromPackeage=true
        g_defaultHTTPDConfFileLocation=/etc/apache2/vhosts.d/vhost.conf
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
   esac
done

if [ ! -f "$g_defaultHTTPDConfFileLocation" ]; then
        echo "The httpd.conf file $g_defaultHTTPDConfFileLocation didn't exist"
        exit 1
fi

bakFile=$g_defaultHTTPDConfFileLocation"_bak"
cp $g_defaultHTTPDConfFileLocation $bakFile


sed 's/AllowOverride None/AllowOverride All/g' $g_defaultHTTPDConfFileLocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_defaultHTTPDConfFileLocation;
sed 's/AllowOverride none/AllowOverride All/g' $g_defaultHTTPDConfFileLocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_defaultHTTPDConfFileLocation;

if [ -f "$g_frompackagelocation" ]; then
    sed 's/AllowOverride None/AllowOverride All/g' $g_frompackagelocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_frompackagelocation;
    sed 's/AllowOverride none/AllowOverride All/g' $g_frompackagelocation >/tmp/httpd.conf; mv /tmp/httpd.conf $g_frompackagelocation;
fi

if [ ! -d "$DocumentRoot" ]; then
  mkdir -p $DocumentRoot
fi

if [ -f $DocumentRoot/.htaccess ]; then
rm $DocumentRoot/.htaccess
else
touch $DocumentRoot/.htaccess
echo "123abc" >> $DocumentRoot/.htaccess
fi

restartApacheService
cleanHTTPBakFile

exit 0
