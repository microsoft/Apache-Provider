# -*- mode: Makefile; -*- 
#--------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation.  All rights reserved.
#--------------------------------------------------------------------------------
# 2014-01-17
#--------------------------------------------------------------------------------

BASE_DIR := $(subst /build,,$(PWD))
OMI_ROOT := $(shell cd ../../omi/distro; pwd -P)

HTTP_LIBDIR := $(shell apxs -q LIBEXECDIR)

include $(BASE_DIR)/build/config.mak

ifndef ENABLE_DEBUG
$(error "ENABLE_DEBUG is not set.  Please re-run configure")
endif

SOURCE_DIR := $(BASE_DIR)/source/code
PROVIDER_DIR := $(SOURCE_DIR)/providers

TARGET_DIR := $(BASE_DIR)/target
PROVIDER_LIBRARY := $(TARGET_DIR)/libApacheHttpdProvider.so
APACHE_MODULE_BASENAME := mod_cimprov
APACHE_MODULE := $(TARGET_DIR)/$(APACHE_MODULE_BASENAME).so

# Include files

INCLUDE_VHOST := $(SOURCE_DIR)/include/mmap_region.h

# Compiler flags

OMI_INCLUDE_FLAGS := -I$(OMI_ROOT)/output/include
MODULE_INCLUDE_FLAGS := -I$(SOURCE_DIR)/include
PROVIDER_INCLUDE_FLAGS := -I/usr/include/apr-1 -I$(PROVIDER_DIR)/support

ifeq ($(ENABLE_DEBUG),1)
PROV_DEBUG_FLAGS := -g
endif

COMPILE_FLAGS := $(PROV_DEBUG_FLAGS) -D_REENTRANT -fstack-protector-all -Wall -fno-nonansi-builtins  -Woverloaded-virtual -Wformat -Wformat-security   -Wfloat-equal -Wcast-align -Wswitch-enum -Wundef -Wshadow -Wwrite-strings -Wredundant-decls -Werror  -Wcast-qual -fPIC

LINK_LIBRARIES := -L$(OMI_ROOT)/output/lib -lmicxx -lapr-1

SHARED_FLAGS := -shared

#--------------------------------------------------------------------------------
# Build targets

all : $(OMI_ROOT)/output $(PROVIDER_LIBRARY) $(APACHE_MODULE)

clean :
	-rm -rf $(TARGET_DIR)

distclean : clean
	-rm $(BASE_DIR)/build/config.mak
	-find $(BASE_DIR) -name \*~ -exec rm {} \;
	-make -C $(OMI_ROOT) distclean
	-rm -rf $(OMI_ROOT)/output*

# Install both the Apache module and the OMI provider
# This assumes that:
#   1) Apache is already configured to load mod_cimprov.so with a line like:
#          LoadModule cimprov_module /usr/lib64/httpd/modules/mod_cimprov.so
#   2) OMI already has registration files for libApacheHttpdProvider.so
install : $(PROVIDER_LIBRARY) $(APACHE_MODULE)
	@echo "========================= Performing Installing module"
	sudo cp -v $(APACHE_MODULE) $(HTTP_LIBDIR)
	sudo cp -v $(PROVIDER_LIBRARY) /opt/microsoft/omi/lib/
	sudo /etc/init.d/omiserverd restart
	sudo /etc/init.d/httpd restart

restart :
	sudo /etc/init.d/omiserverd restart
	sudo /etc/init.d/httpd restart


#--------------------------------------------------------------------------------
# OMI build
#
# Build the OMI distribution
#
# Technically, we should go to build OMI all the time. But I'd rather not spend
# the time doing it here EVERY TIME, when we never normally change OMI. This is
# a good tradeoff (build if not built, otherwise assume all is well).
#
# Doing a 'make clean' in OMI directory will force us to rebuild.

$(OMI_ROOT)/output : $(OMI_ROOT)/output/lib/libmicxx.so

$(OMI_ROOT)/output/lib/libmicxx.so :
	@echo "========================= Performing Building OMI"
	make -C $(OMI_ROOT)
ifeq ($(PERFORM_OMI_MAKEINSTALL),1)
	make -C $(OMI_ROOT) install
endif

#--------------------------------------------------------------------------------
# Apache Provider
#
# Build the Provider Library 

STATIC_PROVIDERLIB_SRCFILES = \
	$(PROVIDER_DIR)/Apache_HTTPDServer_Class_Provider.cpp \
	$(PROVIDER_DIR)/Apache_HTTPDServerStatistics_Class_Provider.cpp \
	$(PROVIDER_DIR)/Apache_HTTPDVirtualHost_Class_Provider.cpp \
	$(PROVIDER_DIR)/Apache_HTTPDVirtualHostCertificate_Class_Provider.cpp \
	$(PROVIDER_DIR)/Apache_HTTPDVirtualHostStatistics_Class_Provider.cpp

STATIC_PROVIDERLIB_SRCFILES += \
	$(PROVIDER_DIR)/schema.c \
	$(PROVIDER_DIR)/stubs.cpp \
	$(PROVIDER_DIR)/module.cpp

STATIC_PROVIDERLIB_SRCFILES += \
	$(PROVIDER_DIR)/support/apachebinding.cpp \
	$(PROVIDER_DIR)/support/datasampler.cpp

PROVIDER_HEADERS = \
	$(PROVIDER_DIR)/support/apachebinding.h \
	$(PROVIDER_DIR)/support/datasampler.h

$(PROVIDER_LIBRARY): $(STATIC_PROVIDERLIB_SRCFILES) $(INCLUDE_VHOST) $(PROVIDER_HEADERS)
	@echo "========================= Performing Building provider"
	mkdir -p $(TARGET_DIR)
	g++ $(COMPILE_FLAGS) $(SHARED_FLAGS) $(OMI_INCLUDE_FLAGS) $(MODULE_INCLUDE_FLAGS) $(PROVIDER_INCLUDE_FLAGS) -o $@ $(STATIC_PROVIDERLIB_SRCFILES) $(LINK_LIBRARIES)

#--------------------------------------------------------------------------------
# Apache Module

APACHE_REAL_SOURCE := $(SOURCE_DIR)/module/$(APACHE_MODULE_BASENAME).c
APACHE_TARGET_SOURCE := $(TARGET_DIR)/module/$(APACHE_MODULE_BASENAME).c

$(APACHE_TARGET_SOURCE) : $(APACHE_REAL_SOURCE)
	mkdir -p $(TARGET_DIR)/module
	ln -s  $(APACHE_REAL_SOURCE) $(APACHE_TARGET_SOURCE)

$(APACHE_MODULE) : $(APACHE_TARGET_SOURCE) $(INCLUDE_VHOST)
	@echo "========================= Performing Building module"
	cd $(TARGET_DIR)/module && apxs -c $(MODULE_INCLUDE_FLAGS) $(APACHE_MODULE_BASENAME).c && cp -v .libs/$(APACHE_MODULE_BASENAME).so $(APACHE_MODULE)



#
# The following is all for OMI "bootstrap" stuff (allowing omigen and omireg to run)
#

ifeq ($(PERFORM_OMI_MAKEINSTALL),1)

CLASSES = \
	Apache_HTTPDServer \
	Apache_HTTPDServerStatistics \
	Apache_HTTPDVirtualHost \
	Apache_HTTPDVirtualHostCertificate \
	Apache_HTTPDVirtualHostStatistics

OMI_DIR = $(OMI_ROOT)/output/install_local
OMI_BINDIR = $(OMI_DIR)/bin
OMI_LIBDIR = $(OMI_DIR)/lib
OMIGEN = $(OMI_BINDIR)/omigen

MOF_DIR = $(SOURCE_DIR)/mof

# For these targets to run, configure MUST be run with the --enable-omi-tools qualifier, like this:
#   ./configure --enable-debug --enable-omi-tools

schema_gen :
	# The omigen facility is unhappy if no ~/.omigenrc file exists, so create one for this run
	touch ~/.omigenrc
	mkdir -p $(TARGET_DIR)/omi
	cd $(TARGET_DIR)/omi; $(OMIGEN) $(MOF_DIR)/apache.mof -d $(PROVIDER_DIR) $(CLASSES) --cpp -I $(OMI_DIR)/share/omischema
	rm ~/.omigenrc

omi_reg :
	mkdir -p $(TARGET_DIR)/omi
	mkdir -p $(OMI_DIR)/etc/omiregister/root-apache
	export LD_LIBRARY_PATH=$(LD_LIBRARY_PATH):$(OMI_LIBDIR); cd $(TARGET_DIR)/omi; $(OMI_BINDIR)/omireg -n root/apache -o root $(PROVIDER_LIBRARY)
	cp -r $(OMI_DIR)/etc/omiregister/root-apache $(TARGET_DIR)/omi

endif # $(PERFORM_OMI_MAKEINSTALL),1
