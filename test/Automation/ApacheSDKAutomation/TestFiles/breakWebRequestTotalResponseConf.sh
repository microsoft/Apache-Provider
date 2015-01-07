#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

function cleanHTTPBakFile {
    bakFile=$g_defaultHTTPDConfFileLocation"_bak"
    cp $g_defaultHTTPDConfFileLocation $g_defaultHTTPDConfFileLocation"_test"
    cp $bakFile $g_defaultHTTPDConfFileLocation
    rm $bakFile

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


#Main
g_defaultHTTPDConfFileLocation=/etc/httpd/conf/httpd.conf
sFromPackage=false
isFromSource=false

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
   esac
done

if [ ! -f "$g_defaultHTTPDConfFileLocation" ] && [ "$needCreateHTTPPorts" = "true" ]; then
        echo "The httpd.conf file $g_defaultHTTPDConfFileLocation didn't exist"
        exit 1
fi

bakFile=$g_defaultHTTPDConfFileLocation"_bak"
cp $g_defaultHTTPDConfFileLocation $bakFile

sed -i 's/^KeepAlive Off/KeepAlive On/' $g_defaultHTTPDConfFileLocation
sed -i 's/^MaxKeepAliveRequests 100/MaxKeepAliveRequests 1/' $g_defaultHTTPDConfFileLocation
sed -i 's/^KeepAliveTimeout 15/KeepAliveTimeout 60/' $g_defaultHTTPDConfFileLocation

echo "<IfModule prefork.c>" >> $g_defaultHTTPDConfFileLocation
echo "StartServers 1" >> $g_defaultHTTPDConfFileLocation
echo "MinSpareServers 1" >> $g_defaultHTTPDConfFileLocation
echo "MaxSpareServers 1" >> $g_defaultHTTPDConfFileLocation
echo "ServerLimit 1" >> $g_defaultHTTPDConfFileLocation
echo "MaxClients 1" >> $g_defaultHTTPDConfFileLocation
echo "MaxRequestsPerChild 400" >> $g_defaultHTTPDConfFileLocation
echo "</IfModule>" >> $g_defaultHTTPDConfFileLocation

restartApacheService
cleanHTTPBakFile

exit 0
