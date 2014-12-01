#!/bin/sh
function usage {
   echo  "Usage:"
   echo  "-d: httpd.conf file location, default is /etc/httpd/conf/httpd.conf"
   echo  "-p: the port which we set. [80,81,82]"
   echo  "example: createHTTPPortforApache.sh -p 80,81"
}

function removeOldPorts {
    sed -i '/^<VirtualHost/i\<Ifmodule test\>' $confFileLocation
    sed -i '/^<\/VirtualHost/a\<\/Ifmodule\>' $confFileLocation
    sed -i 's/^Listen/#&/' $confFileLocation

}

function AddPort {
    echo "<VirtualHost *:$1>" >> $2
    echo "ServerAdmin webmaster@dummy-host.example.com" >> $2
    echo "DocumentRoot /www/docs/dummy-host.example.com" >> $2
    echo "ServerName dummy-host.example.com" >> $2
    echo "ErrorLog logs/dummy-host.example.com-error_log" >> $2
    echo "CustomLog logs/dummy-host.example.com-access_log common" >> $2
    echo "</VirtualHost>" >> $2
}

#Main
confFileLocation=/etc/httpd/conf/httpd.conf
optP=""

while [ $# -ne 0 ]; do
   case "$1" in
	-d)
	    confFileLocation=$2
	    shift 2;;
	-p)
	    optP=$2
	    shift 2;;
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
   echo "Httpd.conf didn't exists"
   exit 0
fi

port[0]=`echo $optP | awk -F ',' '{print $1}'`
port[1]=`echo $optP | awk -F ',' '{print $2}'`

#echo ${port[0]}
#echo ${port[1]}

bakFile=$confFileLocation"_bak"
#echo $bakFile

cp $confFileLocation $bakFile

if [ -z "${port[0]}" ]; then
    echo "Please add ports you want set"
    exit 0
fi
removeOldPorts

AddPort ${port[0]} $confFileLocation

if [ ! -z "${port[1]}" ]; then
    AddPort ${port[1]} $confFileLocation
fi
service httpd restart
exit 0
