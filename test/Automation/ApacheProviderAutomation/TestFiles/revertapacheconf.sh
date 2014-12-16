#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

g_defaultHTTPDConfFileLocation=/etc/httpd/conf/httpd.conf
g_defaultSSLConfFileLocation=/etc/httpd/conf.d/ssl.conf
isFromPackage=false
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

if [ "$isFromSource" = "true" ]; then
        /usr/local/apache2/bin/httpd -k restart
     else
     if [ "$isFromPackage" = "true" ]; then
        service httpd restart
     fi
fi

