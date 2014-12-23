s_defaultHTTPDConfFileLocation="/usr/local/apache2/conf/httpd.conf"
p_defaultHTTPDConfFileLocation="/etc/httpd/conf/httpd.conf"

if [ -f $s_defaultHTTPDConfFileLocation ]; then
defaultHTTPDConfFileLocation=$s_defaultHTTPDConfFileLocation
bakFile=$defaultHTTPDConfFileLocation"_bak"
cp $defaultHTTPDConfFileLocation $bakFile
echo "ErrorSign" >> $defaultHTTPDConfFileLocation
exit 0
fi

if [ -f $p_defaultHTTPDConfFileLocation ]; then
defaultHTTPDConfFileLocation=$p_defaultHTTPDConfFileLocation
bakFile=$defaultHTTPDConfFileLocation"_bak"
cp $defaultHTTPDConfFileLocation $bakFile
echo "ErrorSign" >> $defaultHTTPDConfFileLocation
exit 0
fi
