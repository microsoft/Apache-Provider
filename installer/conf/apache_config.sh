#! /bin/bash

SCRIPT=`basename $0`
VERBOSE=1
OPERATION=""
RESTART_APACHE=0

INCLUDE_DIRECTIVE="include /etc/opt/microsoft/apache-cimprov/conf/mod_cimprov.conf"
INCLUDE_FILE="/etc/opt/microsoft/apache-cimprov/conf/mod_cimprov.conf"

#
# Routines to configure and unconfigure Apache server
#

# Note: FindApacheConfigFile() is found in SCXCore/installer/oss-kits/apache-oss-test.sh
#       If this function is modified, modify in that file as well!
FindApacheConfigFile()
{
    if [ -z "${APACHE_CONF}" -o -z "${CONF_STYLE}" ]; then
        # Source code installation w/default directories
        APACHE_CONF=/usr/local/apache2/conf/httpd.conf
        CONF_STYLE=f
        [ -e "${APACHE_CONF}" ] && return

        # Favor the conf.d-style directories
        APACHE_CONF=/etc/httpd/conf.d
        CONF_STYLE=d
        [ -d "${APACHE_CONF}" ] && return

        # Favor the conf.d-style directories
        APACHE_CONF=/etc/apache2/conf.d
        CONF_STYLE=d
        [ -d "${APACHE_CONF}" ] && return

        # Redhat-type installation
        APACHE_CONF=/etc/httpd/conf/httpd.conf
        CONF_STYLE=f
        [ -e "${APACHE_CONF}" ] && return

        # SuSE-type installation
        APACHE_CONF=/etc/apache2/default-server.conf
        CONF_STYLE=f
        [ -e "${APACHE_CONF}" ] && return

        # Ubuntu-type installation
        APACHE_CONF=/etc/apache2/apache2.conf
        CONF_STYLE=f
        [ -e "${APACHE_CONF}" ] && return

        # Unable to find it, so indicate so
        APACHE_CONF=
        CONF_STYLE=
    fi
}

ConfigureApacheConf_File()
{
    # Find anything?
    if [ -z "${APACHE_CONF}" -o -z "${CONF_STYLE}" ]; then
        echo "Failure configuring Apache: Apache configuration file was not found" 1>&2
        return 1;
    fi

    if [ "${CONF_STYLE}" = "d" ]; then
        #
    	# Support for conf.d style setups
    	#

        [ -e ${APACHE_CONF}/mod_cimprov.conf ] && rm ${APACHE_CONF}/mod_cimprov.conf
	[ $VERBOSE -ne 0 ] && echo "Linking file ${APACHE_CONF}/mod_cimprov.conf to ${INCLUDE_FILE}"
        ln -s ${INCLUDE_FILE} ${APACHE_CONF}/mod_cimprov.conf
	RESTART_APACHE=1
    elif [ "${CONF_STYLE}" = "f" ]; then
        #
    	# Support for httpd.conf-style setups
    	#

        grep -q "${INCLUDE_DIRECTIVE}" ${APACHE_CONF}
        if [ $? -eq 0 ]; then
            # mod_cimprov is already configured in Apache
            return 0
        fi
        #
        # Append the include directive to the Apache configuraiton file
        #
	[ $VERBOSE -ne 0 ] && echo "Appending include directive to file ${APACHE_CONF}"
        echo "${INCLUDE_DIRECTIVE}" >> ${APACHE_CONF}
        if [ $? -ne 0 ]; then
            echo "Can't append include directive to file ${APACHE_CONF}"
            return 1
        fi
	RESTART_APACHE=1
    else
        echo "Internal error configuring Apache; please report this problem to support services" 1>&2
        echo "    Conf file:  ${APACHE_CONF}" 1>&2
        echo "    Conf style: ${CONF_STYLE}" 1>&2
        return 1;
    fi

    # Configuration complete
    return 0
}

UnconfigureApacheConf_File()
{
    # Find anything?
    if [ -z "${APACHE_CONF}" -o -z "${CONF_STYLE}" ]; then
        echo "Failure unconfiguring Apache: Apache configuration file was not found" 1>&2
        return 1;
    fi

    if [ "${CONF_STYLE}" = "d" ]; then
        #
    	# Support for conf.d style setups
    	#

        if [ -L ${APACHE_CONF}/mod_cimprov.conf ]; then
	    [ $VERBOSE -ne 0 ] && echo "Removing linkage file ${APACHE_CONF}/mod_cimprov.conf"
            rm ${APACHE_CONF}/mod_cimprov.conf
	    RESTART_APACHE=1
            return 0
        fi

        # Looks like Apache wasn't previously configured
        echo "Apache not previously configured; nothing to unconfigure" 1>&2
        return 1
    elif [ "${CONF_STYLE}" = "f" ]; then
        #
    	# Support for httpd.conf-style setups
    	#

        apache_config_data=`grep -v "${INCLUDE_DIRECTIVE}" ${APACHE_CONF}`
        if [ $? -ne 0 ]; then
            # mod_cimprov not configured in Apache
	    echo "Apache not currently configured to include mod_cimprov.conf"
            return 0
    	fi
        #
        # Write it back (to the copy first, but leave backup "just in case")
        #
	[ $VERBOSE -ne 0 ] && echo "Creating backup of file ${APACHE_CONF}"
        cp -p ${APACHE_CONF} ${APACHE_CONF}.bak
        if [ $? -ne 0 ]; then
	   echo "Can't create backup of ${APACHE_CONF}"
	   return 1
        fi
	[ $VERBOSE -ne 0 ] && echo "Removing include directive from file ${APACHE_CONF}"
        cp -p ${APACHE_CONF} ${APACHE_CONF}.tmp
        echo "${apache_config_data}" > ${APACHE_CONF}.tmp
        if [ $? -ne 0 ]; then
            echo "Can't write to ${APACHE_CONF}.tmp"
            return 1
        fi
        mv ${APACHE_CONF}.tmp ${APACHE_CONF}
        if [ $? -ne 0 ]; then
            echo "Can't replace file ${APACHE_CONF}"
            return 1
        fi
	# Unconfiguraiton complete
	RESTART_APACHE=0
        return 0
    else
        echo "Internal error unconfiguring Apache; please report this problem to support services" 1>&2
        echo "    Conf file:  ${APACHE_CONF}" 1>&2
        echo "    Conf style: ${CONF_STYLE}" 1>&2
        return 1;
    fi
}

#
# Routines to stop, start, and restart Apache service
#

FindApacheService()
{
    if [ -x /bin/systemctl ]; then
        # Will match either "active" or "inactive", but not "unknown"
       	/bin/systemctl status httpd | head -2 | tail -1 | grep "Loaded: loaded" > /dev/null 2>&1
       	if [ $? -eq 0 ]; then 
       	    APACHE_SERVICE="httpd"
            SERVICE_TYPE="systemd"
        else
            # Try for apache2; if not found, last ditch effort for a local install
            /bin/systemctl status apache2 | head -2 | tail -1 | grep "Loaded: loaded" > /dev/null 2>&1
            if [ $? -eq 0 ]; then
                APACHE_SERVICE="apache2"
                SERVICE_TYPE="systemd"
            elif [ -x  /usr/local/apache2/bin/httpd ]; then
                APACHE_SERVICE="none" 
                SERVICE_TYPE="legacy"
            else
                echo "Unable to determine Apache service name under systemd; Apache must be restarted manually" 1>&2
            fi
        fi
    elif [ -x /etc/init.d/httpd ]; then
        APACHE_SERVICE="httpd"
        SERVICE_TYPE="legacy"
    elif [ -x /etc/init.d/apache2 ]; then
        APACHE_SERVICE="apache2"
        SERVICE_TYPE="legacy"
    elif [ -x  /usr/local/apache2/bin/httpd ]; then
        APACHE_SERVICE="none" 
        SERVICE_TYPE="legacy"
    else
        echo "Unable to determine Apache service name; Apache must be restarted manually" 1>&2
    fi
}

StopApacheServer()
{
    FindApacheService
    if [ -z "${APACHE_SERVICE}" ]; then
        echo "No service defined for Apache; please stop Apache manually"
        return 0
    fi

    if [ "${SERVICE_TYPE}" = "systemd" -a -n "${APACHE_SERVICE}" ]; then
        echo "Stopping Apache server ..."
        /bin/systemctl stop ${APACHE_SERVICE}
    elif [ "${APACHE_SERVICE}" = "none" -a -x /usr/local/apache2/bin/httpd ]; then
        echo "Stopping Apache server ..."
        /usr/local/apache2/bin/httpd -k stop
    elif [ -x /usr/sbin/invoke-rc.d ]; then
        /usr/sbin/invoke-rc.d ${APACHE_SERVICE} stop
    elif [ -x /sbin/service ]; then
        service ${APACHE_SERVICE} stop
    else
        echo "Unrecognized Service Controller to stop Apache Service" 1>&2
	return 0
    fi

    # Apache may begin to shut down, but may not complete before we try to start up again
    COUNTER=5
    while [ $COUNTER -gt 0 ]; do
        PROCESSES=`ps -ef | egrep "apache2|httpd" | grep -v grep | wc -l`
        [ $PROCESSES -eq 0 ] && break

        echo "Waiting for Apache to shut down (${COUNTER}) ..."
        COUNTER=$(( ${COUNTER} - 1 ))
        sleep 1
    done	
}

StartApacheServer()
{
    FindApacheService
    if [ -z "${APACHE_SERVICE}" ]; then
        echo "No service defined for Apache; please start Apache manually"
        return 0;
    fi

    if [ "${SERVICE_TYPE}" = "systemd" -a -n "${APACHE_SERVICE}" ]; then
        echo "Starting Apache server ..."
        /bin/systemctl start ${APACHE_SERVICE}
    elif [ "${APACHE_SERVICE}" = "none" -a -x /usr/local/apache2/bin/httpd ]; then
        echo "Starting Apache server ..."
        /usr/local/apache2/bin/httpd -k start
    elif [ -x /usr/sbin/invoke-rc.d ]; then
        /usr/sbin/invoke-rc.d ${APACHE_SERVICE} start
    elif [ -x /sbin/service ]; then
        service ${APACHE_SERVICE} start
    else
        echo "Unrecognized Service Controller to start Apache Service" 1>&2
        exit 1
    fi
}

RestartApacheServer()
{
    StopApacheServer
    StartApacheServer
}

ShowUsage()
{
    echo "$0 [-h] [-q [-v] -c | -v" 1>&2
    echo "  -h: This message" 1>&2
    echo "  -q  Quiet output" 1>&2
    echo "  -v: Verbose output [default]" 1>&2
    echo 1>&2
    echo "  -c: Configure and restart Apache" 1>&2
    echo "  -u: Unconfigure and restart Apache" 1>&2
}

#
# Parse command line arguments
#

OPTIND=1

while getopts ":hqvcu" opt; do
    case "$opt" in
	\?)
	    echo "$0: illegal option -- $OPTARG, use -h for help" 1>&2
	    exit 1
	    ;;
	h)
            ShowUsage
            exit 1
            ;;
	q)
	    VERBOSE=0
	    ;;
	v)
	    VERBOSE=1
            ;;

	c)
	    OPERATION="C"
	    ;;

	u)
	    OPERATION="U"
	    ;;
    esac
done

shift $((OPTIND-1))
[ "$1" = "--" ] && shift

# Actually do the Apache configuration

if [ `id -u` -ne 0 ]; then
    echo "$0: This script requires root privileges" 1>&2
    exit 1
fi

FindApacheConfigFile

case "$OPERATION" in
    C)
	ConfigureApacheConf_File
	;;
    U)
	UnconfigureApacheConf_File
	;;
    *)
	echo "$0: Unknown operation; use -h for help" 1>&2
	exit 1
esac

if [ ${RESTART_APACHE} -ne 0 ]; then
    [ $VERBOSE -ne 0 ] && echo "Restarting Apache server ..."
    RestartApacheServer
fi
