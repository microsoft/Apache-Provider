#!/bin/bash

hostname=`hostname`

openssl req -x509 -nodes -days 10 -newkey rsa:2048 -subj "/C=US/ST=Washington/L=Seattle/O=Microsoft/OU=OSTC/CN=$hostname" -keyout /tmp/server.key -out /tmp/server.crt

mkdir -p /etc/ssl/crt;
cp /tmp/server.* /etc/ssl/crt
