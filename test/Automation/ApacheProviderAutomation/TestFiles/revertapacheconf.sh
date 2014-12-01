#!/bin/sh
function usage {
   echo  "Usage:"
   echo  "-d: httpd.conf file location, default is /etc/httpd/conf/httpd.conf"
}

#Main
confFileLocation=/etc/httpd/conf/httpd.conf

while [ $# -ne 0 ]; do
   case "$1" in
        -d)
            confFileLocation=$2
            shift 2;;
        -?)
            usage
            exit 0;;
    esac
done

bakFile=$confFileLocation"_bak"
cp $bakFile $confFileLocation
rm $bakFile
service httpd restart
