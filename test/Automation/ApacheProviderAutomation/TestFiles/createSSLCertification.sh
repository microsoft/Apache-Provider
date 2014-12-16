#!/bin/sh
# Copyright (c) Microsoft Corporation.  All rights reserved.
# author: v-jeyin

defaultCrtFile=/tmp/localhost.crt
defaultKeyFile=/tmp/localhost.key

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

openssl req -x509 -nodes -days 10 -newkey rsa:2048 -subj "/C=US/ST=Washington/L=Seattle/O=Microsoft/OU=OSTC/CN=$hostname" -keyout $defaultKeyFile -out $defaultCrtFile

exit 0
