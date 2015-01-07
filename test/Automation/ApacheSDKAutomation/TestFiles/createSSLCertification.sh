#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

defaultCrtFile=localhost.crt
defaultKeyFile=localhost.key

while [ $# -ne 0 ]; do
   case "$1" in
        -c)
            defaultCrtFile=$2
            shift 2;;
        -k)
            defaultKeyFile=$2
	    shift 2;;
	*)
	    exit 0;;
    esac
done


hostname=`hostname`

openssl req -x509 -nodes -days 10 -newkey rsa:2048 -subj "/C=US/ST=Washington/L=Seattle/O=Microsoft/OU=OSTC/CN=$hostname" -keyout /tmp/$defaultKeyFile -out /tmp/$defaultCrtFile

mkdir -p /etc/ssl/crt
cp /tmp/$defaultKeyFile /etc/ssl/crt/$defaultKeyFile
cp /tmp/$defaultCrtFile /etc/ssl/crt/$defaultCrtFile
exit 0
