s_defaultHTTPDConfFileLocation="/usr/local/apache2/conf/httpd.conf"
p_defaultHTTPDConfFileLocation="/etc/httpd/conf/httpd.conf"
s_bakFile="/usr/local/apache2/conf/httpd.conf_bak"
p_bakFile="/etc/httpd/conf/httpd.conf_bak"

if [ -f $s_bakFile ]; then
cp $s_bakFile $s_defaultHTTPDConfFileLocation
rm $s_bakFile
exit 0
fi

if [ -f $p_defaultHTTPDConfFileLocation ]; then
cp $p_bakFile $p_defaultHTTPDConfFileLocation 
rm $p_bakFile
exit 0
fi
