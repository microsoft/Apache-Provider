# Apache_CIMPROV
Apache HTTP Server OMI Provider for OMI Server

##Prequisites
1. At least one modern Linux system with:
  1. root login capability
  2. These build tools:
    1. `GNU Make`
    2. `g++`
    3. `openssl-devel`
2. Open Management Infrastructure (OMI) 1.0.8.(Download from: http://theopengroup.org/software/omi). Note that you need BOTH the binary packages, omi-1.0.8.1.packages.tar.gz,as well as the source distribution file (omi-1.0.8-1.tar.gz).
3. For compiling provider, you need the Apache development package for your distribution. For example, the Apache development package is `httpd-devel` (on Redhat):

4. For Installing/running the provider you need httpd server installed which is either the package `httpd` or `apache2` depending on your distribution
 
##Building the Apache OMI Provider
####1. Extract oss-apache.tar.gz into a directory that you will build it from.
   This will create a "apache" directory like this:
```
apache/apache     (directory with contents)
apache/omi       (no contents under this directory)
README          (this file)
```
####2. Change directory (cd) into directory apache/omi and extract the OMI source
   file downloaded above. After extraction, you should have a single directory
   named "omi-1.0.8". Rename this directory to "Unix" (capital U is important).

   From the base apache directory, things should now look like:
```
apache/apache/...
apache/omi/Unix/...
README          (this file)
```
####3. Building the Apache Provider
  4. `cd apache/apache/build`
  5. `./configure`
  6. `make`

####4. Installing
1. Install the appropriate kit for your system from the binary file,
      omi-1.0.8.1.packages.tar.gz (downloaded earlier).
2.  Install apache provider:
  1. `cd apache/apache/build`
  2. `sudo bash`
  3. `make install`

3. Configure OMI
  4. Locate OMI registration directory. Depending on what version of OMI you installed, the registry directory could be in one of two places
    * `/opt/omi/etc/omiregister`
    * `/etc/opt/omi/conf/omiregister`
  5. Create directory root-apache in this OMI registration directory
  6. Copy Apache's OMI registration file, from Apache base directory, file
           `installer/conf/omi/ApacheHttpdProvider.reg`, to the directory that you
           just created above.
  7. Restart OMI (see OMI documentation on how to do this)
4. Configure Apache:
  5. Run: `/opt/microsoft/apache-cimprov/bin/apache_config.sh -c`
  * Note: If you encounter permission problems loading the mod_cimprov.so
           library, this may be due to SELINUX permissions problems. Be certain
           that either SELINUX permits mod_cimprov.so to be loaded by Apache, or
           disable SELINUX on your system.
  
####5. Test the provider
1. To test the provider, run a command like: `/opt/omi/bin/omicli ei root/apache Apache_HTTPDServer` If the command succeeds, then the Apache provider is running properly. The complete list of classes that the Apache provider can enumerate are as follows:
 - Apache_HTTPDServer*
 - Apache_HTTPDServerStatistics
 - Apache_HTTPDVirtualHost
 - Apache_HTTPDVirtualHostCertificate
 - Apache_HTTPDVirtualHostStatistics
