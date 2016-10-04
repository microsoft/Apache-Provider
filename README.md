# Apache-Provider

The Apache-Provider project implements a CIMOM provider, based on
[OMI][], to return logging and statistical information for
[Apache][]. [Apache][] is an open-source HTTP server created by the
[Apache Software Foundation][]. The Apache-Provider runs on Linux
systems as far back as RedHat 5.0, SuSE 10.1, and Debian 5.0.

[Apache]: https://httpd.apache.org
[Apache Software Foundation]: https://www.apache.org/foundation/how-it-works.html
[OMI]: https://github.com/Microsoft/omi

The Apache-Provider project implements two independent components:

- An [Apache][] module to collect statistical information from the
HTTPD server and make it available to the [OMI][] provider, and
- An [OMI][] provider to report Apache information via OMI

The Apache-Provider provides the following classes:

- [Apache_HTTPDServer](#enumeration-of-apache_httpdserver)
- [Apache_HTTPDServerStatistics](#enumeration-of-apache_httpdserverstatistics)
- [Apache_HTTPDVirtualHostCertificate](#enumeration-of-apache_httpdvirtualhostcertificate)
- [Apache_HTTPDVirtualHost](#enumeration-of-apache_httpdvirtualhost)
- [Apache_HTTPDVirtualHostStatistics](#enumeration-of-apache_httpdvirtualhoststatistics)

-----

The following output shows the results of enumeration of classes:


### Enumeration of Apache_HTTPDServer

```
> /opt/omi/bin/omicli ei root/apache Apache_HTTPDServer
instance of Apache_HTTPDServer
{
    InstanceID=/usr/local/apache2/conf/httpd.conf
    [Key] ProductIdentifyingNumber=1
    [Key] ProductName=/usr/local/apache2/conf/httpd.conf
    [Key] ProductVendor=Apache Software Foundation
    [Key] ProductVersion=2.4.10
    [Key] SystemID=a24s64-cent7-01
    [Key] CollectionID=/usr/local/apache2
    ModuleVersion=1.0.1-7 (20160513)
    ConfigurationFile=/usr/local/apache2/conf/httpd.conf
    InstalledModules={mod_cimprov.c, mod_alias.c, mod_dir.c, mod_autoindex.c, mod_status.c, mod_unixd.c, mod_ssl.c, mod_version.c, mod_setenvif.c, mod_headers.c, mod_env.c, mod_log_config.c, mod_mime.c, mod_filter.c, mod_reqtimeout.c, mod_socache_shmcb.c, mod_auth_basic.c, mod_access_compat.c, mod_authz_core.c, mod_authz_user.c, mod_authz_groupfile.c, mod_authz_host.c, mod_authn_core.c, mod_authn_file.c, worker.c, http_core.c, mod_so.c, core.c}
    InstalledModulesFormatted=mod_cimprov.c, mod_alias.c, mod_dir.c, mod_autoindex.c, mod_status.c, mod_unixd.c, mod_ssl.c, mod_version.c, mod_setenvif.c, mod_headers.c, mod_env.c, mod_log_config.c, mod_mime.c, mod_filter.c, mod_reqtimeout.c, mod_socache_shmcb.c, mod_auth_basic.c, mod_access_compat.c, mod_authz_core.c, mod_authz_user.c, mod_authz_groupfile.c, mod_authz_host.c, mod_authn_core.c, mod_authn_file.c, worker.c, http_core.c, mod_so.c, core.c
    ProcessName=httpd
    ServiceName=_Unknown
    OperatingStatus=OK
}
```

Note that properties `InstalledModules` and `InstalledModulesFormatted`
are virtually identical. `InstalledModules` is an array of strings, which
is a data type natively supported by [OMI][]. Some software may have
difficulty consuming such a data structure, however. As a result, the same
data is available in `InstalledModulesFormatted`, which is formatted as a
comma-separated string.

Due to this formatting, module names with embedded commas may be
difficult to properly differentiate.

### Enumeration of Apache_HTTPDServerStatistics

```
> /opt/omi/bin/omicli ei root/apache Apache_HTTPDServerStatistics
instance of Apache_HTTPDServerStatistics
{
    [Key] InstanceID=/usr/local/apache2/conf/httpd.conf
    TotalPctCPU=0
    IdleWorkers=99
    BusyWorkers=1
    PctBusyWorkers=1
    ConfigurationFile=/usr/local/apache2/conf/httpd.conf
}
```

### Enumeration of Apache_HTTPDVirtualHostCertificate

```
> /opt/omi/bin/omicli ei root/apache Apache_HTTPDVirtualHostCertificate
instance of Apache_HTTPDVirtualHostCertificate
{
    InstanceID=server.crt*9ef7437d
    [Key] Name=server.crt*9ef7437d
    [Key] Version=1.0.1e-fips
    [Key] SoftwareElementState=3
    [Key] SoftwareElementID=server.crt*9ef7437d
    [Key] TargetOperatingSystem=36
    VirtualHost=a24s64-cent7-01,_default_:443,_default_:443
    ExpirationDate=20171004135953.000000+000
    DaysUntilExpiration=365
    FileName=/etc/ssl/crt/server.crt
}
```

### Enumeration of Apache_HTTPDVirtualHost

```
> /opt/omi/bin/omicli ei root/apache Apache_HTTPDVirtualHost
instance of Apache_HTTPDVirtualHost
{
    InstanceID=a24s64-cent7-01,_default_:0
    [Key] Name=a24s64-cent7-01,_default_:0
    [Key] Version=2.4.10
    [Key] SoftwareElementState=3
    [Key] SoftwareElementID=a24s64-cent7-01,_default_:0
    [Key] TargetOperatingSystem=36
    IPAddresses={_default_}
    IPAddressesFormatted=_default_
    Ports={0}
    PortsFormatted=0
    ServerName=a24s64-cent7-01
    ServerAlias={}
    ServerAliasFormatted=
    DocumentRoot=/usr/local/apache2/htdocs
    ServerAdmin=root@localhost
    ErrorLog=logs/error_log
    CustomLog=logs/access_log
    AccessLog=
}
instance of Apache_HTTPDVirtualHost
{
    InstanceID=a24s64-cent7-01,_default_:443,_default_:443
    [Key] Name=a24s64-cent7-01,_default_:443,_default_:443
    [Key] Version=2.4.10
    [Key] SoftwareElementState=3
    [Key] SoftwareElementID=a24s64-cent7-01,_default_:443,_default_:443
    [Key] TargetOperatingSystem=36
    IPAddresses={_default_, _default_}
    IPAddressesFormatted=_default_, _default_
    Ports={443, 443}
    PortsFormatted=443, 443
    ServerName=a24s64-cent7-01
    ServerAlias={}
    ServerAliasFormatted=
    DocumentRoot=/usr/local/apache2/htdocs
    ServerAdmin=root@localhost
    ErrorLog=/usr/local/apache2/logs/error_log
    CustomLog=/usr/local/apache2/logs/ssl_request_log
    AccessLog=/usr/local/apache2/logs/access_log
}
```

### Enumeration of Apache_HTTPDVirtualHostStatistics

```
> /opt/omi/bin/omicli ei root/apache Apache_HTTPDVirtualHostStatistics
instance of Apache_HTTPDVirtualHostStatistics
{
    [Key] InstanceID=a24s64-cent7-01,_default_:0
    ServerName=a24s64-cent7-01
    RequestsTotal=1527
    RequestsTotalBytes=745924849
    RequestsPerSecond=0
    KBPerRequest=30751
    KBPerSecond=4100
    ErrorCount400=0
    ErrorCount500=0
    ErrorsPerMinute400=0
    ErrorsPerMinute500=0
}
instance of Apache_HTTPDVirtualHostStatistics
{
    [Key] InstanceID=a24s64-cent7-01,_default_:443,_default_:443
    ServerName=a24s64-cent7-01
    RequestsTotal=1450
    RequestsTotalBytes=2679151627
    RequestsPerSecond=0
    KBPerRequest=30751
    KBPerSecond=4100
    ErrorCount400=0
    ErrorCount500=0
    ErrorsPerMinute400=0
    ErrorsPerMinute500=0
}
instance of Apache_HTTPDVirtualHostStatistics
{
    [Key] InstanceID=_Total
    ServerName=_Total
    RequestsTotal=2977
    RequestsTotalBytes=3425076476
    RequestsPerSecond=0
    KBPerRequest=30751
    KBPerSecond=8200
    ErrorCount400=0
    ErrorCount500=0
    ErrorsPerMinute400=0
    ErrorsPerMinute500=0
}
```
