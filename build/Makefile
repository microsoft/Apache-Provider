# -*- mode: Makefile; -*- 
#--------------------------------- START OF LICENSE ----------------------------
#
# Apache Cimprov ver. 1.0
#
# Copyright (c) Microsoft Corporation
#
# All rights reserved. 
#
# Licensed under the Apache License, Version 2.0 (the License); you may not use
# this file except in compliance with the license. You may obtain a copy of the
# License at http://www.apache.org/licenses/LICENSE-2.0 
#
# THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
# ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
# WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
# MERCHANTABLITY OR NON-INFRINGEMENT.
#
# See the Apache Version 2.0 License for specific language governing permissions
# and limitations under the License.
#
#---------------------------------- END OF LICENSE -----------------------------

#-------------------------------------------------------------------------------
# 2014-01-17
#-------------------------------------------------------------------------------

BASE_DIR := $(subst /build,,$(PWD))
OMI_ROOT := $(shell cd ../../omi/Unix; pwd -P)

PF_POSIX := 1

include $(BASE_DIR)/build/config.mak
include $(BASE_DIR)/build/Makefile.version

ifndef ENABLE_DEBUG
$(error "ENABLE_DEBUG is not set.  Please re-run configure")
endif

ifndef SCX_BUILDVERSION_STATUS
$(error "Is Makefile.version missing?  Please re-run configure")
endif

ifeq ($(OPENSOURCE_DISTRO),0)

SCXPAL_DIR := $(shell cd ../../pal; pwd -P)

include $(SCXPAL_DIR)/build/config.mak
include $(SCXPAL_DIR)/build/Makefile.pal

else

RM=rm -f
RMDIR=rm -rf
MKPATH=mkdir -p
ECHO=@echo
COPY=cp
COPYDIR=cp -r
pf_fwrite=echo $(1) > $(2)
pf_fappend=echo $(1) >> $(2)
SOFTLINK=ln -sf
NOOP=echo
MV=mv

endif # ifeq ($(OPENSOURCE_DISTRO),0)

ifeq ($(ULINUX),1)
HTTPD_BASE_V22 := /usr/local_httpd_2.2.0
# Apache v2.2 uses installed version of APR (compatible with Apache v2.2 on SLES 10)

HTTPD_BASE_V24 := /usr/local_httpd_2.4.1
APR_BASE_V24 := /usr/local_apr_1.4.2
APRUTIL_BASE_V24 := /usr/local_apr_util_1.3.0
endif

SOURCE_DIR := $(BASE_DIR)/source/code
TEST_DIR := $(BASE_DIR)/test/code

PROVIDER_DIR := $(SOURCE_DIR)/providers
PROVIDER_TEST_DIR := $(TEST_DIR)/providers
PROVIDER_TEST_SUPPORT_DIR := $(PROVIDER_TEST_DIR)/support
PAL_TESTUTILS_DIR := $(SCXPAL_DIR)/test/code/testutils

TARGET_DIR := $(BASE_DIR)/target/$(BUILD_CONFIGURATION)
PROVIDER_LIBRARY := $(TARGET_DIR)/libApacheHttpdProvider.so
APACHE_MODULE_BASENAME := mod_cimprov
APACHE_MODULE := $(TARGET_DIR)/$(APACHE_MODULE_BASENAME).so

INSTALLER_TMPDIR := $(TARGET_DIR)/installer_tmp

# If building with Apache source code, handle appropriately
APACHE_SOURCE_DIR := $(wildcard /usr/local/apache2)
ifeq ($(APACHE_SOURCE_DIR),)
APACHE_SOURCE_INCLUDE_PATH_OPTION :=
APACHE_SOURCE_LIB_PATH_OPTION :=
else
APACHE_SOURCE_INCLUDE_PATH_OPTION := -I$(APACHE_SOURCE_DIR)/include
APACHE_SOURCE_LIB_PATH_OPTION := -L$(APACHE_SOURCE_DIR)/lib
endif

# Find the appropriate version of 'apxs' to build with
# (When built from source, apxs doesn't change it's name, ULINUX uses source build)
APXS := apxs

ifeq ($(ULINUX),1)
    APACHE_SOURCE_INCLUDE_PATH_OPTION :=
    APACHE_SOURCE_LIB_PATH_OPTION :=
else
    ifeq ($(shell (type apxs > /dev/null 2>&1 && echo 0) || echo 1),1)
        APXS := apxs2
    endif
endif

# Include files

INCLUDE_VHOST := $(SOURCE_DIR)/include/mmap_region.h
INCLUDE_VERSION := $(TARGET_DIR)/buildversion.h
INCLUDE_DEFINES := $(TARGET_DIR)/defines.h

# Compiler flags

OMI_INCLUDE_FLAGS := -I$(OMI_ROOT)/output/include
PROVIDER_INCLUDE_FLAGS := -I/usr/include/apr-1 -I$(PROVIDER_DIR)/support -I$(TARGET_DIR)

# Note: MODULE_INCLUDE_FLAGS always included prior to PROVIDER_INCLUDE_FLAGS for provider and module; we depend on this
ifeq ($(ULINUX),1)

MODULE_INCLUDE_FLAGS_v22 := -I$(SOURCE_DIR)/include -I$(TARGET_DIR) -I$(HTTPD_BASE_V22)/include -DHAVE_TIMES
MODULE_INCLUDE_FLAGS_v24 := -I$(SOURCE_DIR)/include -I$(TARGET_DIR) -I$(HTTPD_BASE_V24)/include -I$(APR_BASE_V24)/include/apr-1 -I$(APRUTIL_BASE_V24)/include/apr-1 -DHAVE_TIMES
LINK_LIBRARIES_v24 := -L$(APR_BASE_V24)/lib -L$(APRUTIL_BASE_V24)/lib

else

MODULE_INCLUDE_FLAGS := -I$(SOURCE_DIR)/include -I$(TARGET_DIR) $(APACHE_SOURCE_INCLUDE_PATH_OPTION) -DHAVE_TIMES

endif

PROVIDER_TEST_INCLUDE_FLAGS := -Wmissing-include-dirs -Wno-non-virtual-dtor -I$(SCXPAL_DIR)/source/code/include -I$(SOURCE_DIR)/include -I$(TARGET_DIR) -I$(SCXPAL_DIR)/test/ext/include -I$(OMI_ROOT)/output/include -I$(OMI_ROOT) -I$(OMI_ROOT)/common -I$(SCXPAL_DIR)/test/code/include $(PROVIDER_INCLUDE_FLAGS) -I$(PROVIDER_DIR) -I$(PROVIDER_TEST_SUPPORT_DIR)

ifeq ($(ENABLE_DEBUG),1)
PROV_DEBUG_FLAGS := -g
endif

COMPILE_FLAGS := $(PROV_DEBUG_FLAGS) -D_REENTRANT -fstack-protector-all -Wall -fno-nonansi-builtins  -Woverloaded-virtual -Wformat -Wformat-security -Wcast-align -Wswitch-enum -Wshadow -Wwrite-strings -Wredundant-decls -Werror -Wno-cast-qual -fPIC

LINK_LIBRARIES := -Wl,-rpath=/opt/omi/lib -L$(OMI_ROOT)/output/lib $(APACHE_SOURCE_LIB_PATH_OPTION) -lmicxx -lapr-1
PROVIDER_TEST_LINK_LIBRARIES := -lbase -lpal -L$(SCXPAL_TARGET_DIR) -lscxcore $(SCXPAL_DIR)/test/ext/lib/linux/$(ARCH)/cppunit/libcppunit.a -lpthread -lrt

SHARED_FLAGS := -shared

# Support for installbuilder

STAGING_DIR := $(TARGET_DIR)/staging

#--------------------------------------------------------------------------------
# Build targets

ifeq ($(ULINUX),1)
all : $(OMI_ROOT)/output $(PROVIDER_LIBRARY)_v22 $(PROVIDER_LIBRARY)_v24 $(APACHE_MODULE)_v22 $(APACHE_MODULE)_v24 kit
else
all : $(OMI_ROOT)/output $(PROVIDER_LIBRARY) $(APACHE_MODULE)
endif

clean :
	$(RMDIR) $(BASE_DIR)/build/Makefilie.version $(BASE_DIR)/build/cppunit_result.* $(BASE_DIR)/build/scxtestrunner.log
	$(RMDIR) $(BASE_DIR)/installer/intermediate $(BASE_DIR)/target
	$(RMDIR) $(PROVIDER_TEST_DIR)/providertestutils.cpp
	-find $(BASE_DIR) -name \*~ -exec rm {} \;

distclean : clean
	$(RM) $(BASE_DIR)/build/config.mak
	-make -C $(OMI_ROOT) distclean
	-$(RMDIR) $(OMI_ROOT)/output*
ifeq ($(OPENSOURCE_DISTRO),0)
	-make -C $(SCXPAL_DIR)/build clean
	-$(RM) $(SCXPAL_DIR)/build/config.mak
	-$(RM) $(SCXPAL_DIR)/build/Makefile.config_cache
	-$(RMDIR) $(SCXPAL_DIR)/intermediate $(SCXPAL_DIR)/target
endif # ifeq ($(OPENSOURCE_DISTRO),0)

ifeq ($(OPENSOURCE_DISTRO),0)

# Install both the Apache module and the OMI provider
# This assumes that:
#   1) Apache is already configured to load mod_cimprov.so with a line like:
#          LoadModule cimprov_module /usr/lib64/httpd/modules/mod_cimprov.so
#   2) OMI already has registration files for libApacheHttpdProvider.so

install : /opt/omi/lib/libApacheHttpdProvider.so /opt/microsoft/apache-cimprov/lib/$(APACHE_MODULE_BASENAME).so
	@[ -e $(TARGET_DIR)/tmp_start_http ] && rm $(TARGET_DIR)/tmp_start_http && sudo /etc/init.d/httpd start || exit 0
	@[ -e $(TARGET_DIR)/tmp_start_omi  ] && rm $(TARGET_DIR)/tmp_start_omi  && sudo /etc/init.d/omiserverd start || exit 0

/opt/microsoft/apache-cimprov/lib/$(APACHE_MODULE_BASENAME).so : $(APACHE_MODULE)
	@echo "========================= Performing Installing Apache module"
	sudo /etc/init.d/httpd stop
	touch $(TARGET_DIR)/tmp_start_http
	sudo cp -v $(APACHE_MODULE) /opt/microsoft/apache-cimprov/lib/

/opt/omi/lib/libApacheHttpdProvider.so : $(PROVIDER_LIBRARY)
	@echo "========================= Performing Installing OMI provider"
	-sudo /etc/init.d/omiserverd stop
	touch $(TARGET_DIR)/tmp_start_omi
	sudo cp -v $(PROVIDER_LIBRARY) /opt/omi/lib/

restart :
	@-sudo /etc/init.d/omiserverd stop
	@sudo /etc/init.d/httpd stop
	@sudo /etc/init.d/httpd start
	@sudo /etc/init.d/omiserverd start

else

# Install for open source distributions:

# We copy all required files to appropriate places, but we do NOT configure apache.
# That must be done manually by the system administrator when Apache can be restarted.

install : $(PROVIDER_LIBRARY) $(APACHE_MODULE)
	$(MKPATH) /etc/opt/microsoft/apache-cimprov/conf
	$(MKPATH) /opt/microsoft/apache-cimprov/bin
	$(MKPATH) /opt/microsoft/apache-cimprov/lib
	$(MKPATH) /var/opt/microsoft/apache-cimprov/run

	$(COPY) ../installer/conf/mod_cimprov.conf /etc/opt/microsoft/apache-cimprov/conf

	$(COPY) ../installer/conf/apache_config.sh /opt/microsoft/apache-cimprov/bin

	$(COPY) $(TARGET_DIR)/libApacheHttpdProvider.so /opt/microsoft/apache-cimprov/lib 
	$(COPY) $(TARGET_DIR)/mod_cimprov.so /opt/microsoft/apache-cimprov/lib

	chmod 644 /etc/opt/microsoft/apache-cimprov/conf/mod_cimprov.conf
	chmod 755 /opt/microsoft/apache-cimprov/bin/apache_config.sh
	chmod 755 /opt/microsoft/apache-cimprov/lib/mod_cimprov.so

	$(SOFTLINK) /opt/microsoft/apache-cimprov/lib/libApacheHttpdProvider.so /opt/omi/lib

endif # ifeq ($(OPENSOURCE_DISTRO),0)

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
# PAL build
#
# Build the PAL (Platform Abstraction Layer)
#
# Technically, we should go to build PAL all the time. But I'd rather not spend
# the time doing it here EVERY TIME, when we never normally change PAL. This is
# a good tradeoff (build if not built, otherwise assume all is well).
#
# Doing a 'make clean' in PAL directory will force us to rebuild.
#
# Note: The PAL is only needed for the unit tests, and will only be built (if
# needed) during the "make test" target.

$(SCXPAL_INTERMEDIATE_DIR) :
	@echo "========================= Performing Building PAL"
	make -C $(SCXPAL_DIR)/build

#--------------------------------------------------------------------------------
# Version information header file
#
# Build the version header file

$(INCLUDE_VERSION) : $(BASE_DIR)/build/Makefile.version
	-$(MKPATH) $(@D)
	@$(ECHO) "Creating $@" 
	@$(call pf_fwrite,"/*------------------------------------------------------------------------------",  $@)
	@$(call pf_fappend,"    Copyright (C) 2007-2014 Microsoft Corp.                                     ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"*/                                                                              ", $@)
	@$(call pf_fappend,"/**                                                                             ", $@)
	@$(call pf_fappend,"    \file                                                                       ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    \brief       Auto generated file containing build version information       ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    \author      Automated Build System                                         ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    DO NOT EDIT THIS FILE!                                                      ", $@)
	@$(call pf_fappend,"    DO NOT CHECK IN THIS FILE!                                                  ", $@)
	@$(call pf_fappend,"*/                                                                              ", $@)
	@$(call pf_fappend,"/*----------------------------------------------------------------------------*/", $@)
	@$(call pf_fappend,"#ifndef BUILDVERSION_H                                                          ", $@)
	@$(call pf_fappend,"#define BUILDVERSION_H                                                          ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#include <string>                                                               ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"/** Major version number */                                                     ", $@)
	@$(call pf_fappend,"const int CIMPROV_BUILDVERSION_MAJOR = $(SCX_BUILDVERSION_MAJOR);               ", $@)
	@$(call pf_fappend,"/** Minor version number */                                                     ", $@)
	@$(call pf_fappend,"const int CIMPROV_BUILDVERSION_MINOR = $(SCX_BUILDVERSION_MINOR);               ", $@)
	@$(call pf_fappend,"/** Patch version number */                                                     ", $@)
	@$(call pf_fappend,"const int CIMPROV_BUILDVERSION_PATCH = $(SCX_BUILDVERSION_PATCH);               ", $@)
	@$(call pf_fappend,"/** Build number */                                                             ", $@)
	@$(call pf_fappend,"const int CIMPROV_BUILDVERSION_BUILDNR = $(SCX_BUILDVERSION_BUILDNR);           ", $@)
	@$(call pf_fappend,"/** Build date */                                                               ", $@)
	@$(call pf_fappend,"const std::string CIMPROV_BUILDVERSION_DATE(\"$(SCX_BUILDVERSION_DATE)\");      ", $@)
	@$(call pf_fappend,"/** Build status */                                                             ", $@)
	@$(call pf_fappend,"const std::string CIMPROV_BUILDVERSION_STATUS(\"$(SCX_BUILDVERSION_STATUS)\");  ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#endif /* BUILDVERSION_H */                                                     ", $@)
	@$(call pf_fappend,"/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/", $@)

#================================================================================
# File depends.h (compiler dependencies)
#================================================================================

$(INCLUDE_DEFINES) : $(BASE_DIR)/build/config.mak
	-$(MKPATH) $(@D)
	@$(ECHO) "Creating $@" 
	@$(call pf_fwrite,"/*------------------------------------------------------------------------------",  $@)
	@$(call pf_fappend,"    Copyright (C) 2007 Microsoft Corp.                                          ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"*/                                                                              ", $@)
	@$(call pf_fappend,"/**                                                                             ", $@)
	@$(call pf_fappend,"    \file                                                                       ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    \brief       Auto generated file containing build definitions               ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    \author      Automated Build System                                         ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"    DO NOT EDIT THIS FILE!                                                      ", $@)
	@$(call pf_fappend,"    DO NOT CHECK IN THIS FILE!                                                  ", $@)
	@$(call pf_fappend,"*/                                                                              ", $@)
	@$(call pf_fappend,"/*----------------------------------------------------------------------------*/", $@)
	@$(call pf_fappend,"#ifndef DEFINES_H                                                               ", $@)
	@$(call pf_fappend,"#define DEFINES_H                                                               ", $@)
	@$(call pf_fappend,"                                                                                ", $@)
ifneq ($(PF_DISTRO),)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef PF_DISTRO_$(PF_DISTRO)                                                  ", $@)
	@$(call pf_fappend,"#define PF_DISTRO_$(PF_DISTRO)                                                  ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
endif
ifneq ($(PF_MAJOR),)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef PF_MAJOR                                                                 ", $@)
	@$(call pf_fappend,"#define PF_MAJOR $(PF_MAJOR)                                                    ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
endif
ifneq ($(PF_MINOR),)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef PF_MINOR                                                                 ", $@)
	@$(call pf_fappend,"#define PF_MINOR $(PF_MINOR)                                                    ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
endif
ifneq ($(ARCH),)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef $(ARCH)                                                                 ", $@)
	@$(call pf_fappend,"#define $(ARCH)                                                                 ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
endif
ifeq ($(BUILD_TYPE),Debug)
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef _DEBUG                                                                  ", $@)
	@$(call pf_fappend,"#define _DEBUG                                                                  ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
else
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#ifndef NDEBUG                                                                  ", $@)
	@$(call pf_fappend,"#define NDEBUG                                                                  ", $@)
	@$(call pf_fappend,"#endif                                                                          ", $@)
endif
	@$(call pf_fappend,"                                                                                ", $@)
	@$(call pf_fappend,"#endif /* DEFINES_H */                                                          ", $@)
	@$(call pf_fappend,"/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/", $@)

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
	$(PROVIDER_DIR)/support/datasampler.cpp \
	$(PROVIDER_DIR)/support/utils.cpp \
	$(PROVIDER_DIR)/support/temppool.cpp

PROVIDER_HEADERS = \
	$(PROVIDER_DIR)/support/apachebinding.h \
	$(PROVIDER_DIR)/support/cimconstants.h \
	$(PROVIDER_DIR)/support/datasampler.h \
	$(PROVIDER_DIR)/support/utils.h \
	$(PROVIDER_DIR)/support/temppool.h

ifeq ($(ULINUX),1)

$(PROVIDER_LIBRARY)_v22: $(STATIC_PROVIDERLIB_SRCFILES) $(INCLUDE_VHOST) $(INCLUDE_VERSION) $(PROVIDER_HEADERS)
	@echo "========================= Performing Building provider (w/HTTP v2.2)"
	$(MKPATH) $(TARGET_DIR)
	g++ $(COMPILE_FLAGS) $(SHARED_FLAGS) $(OMI_INCLUDE_FLAGS) $(MODULE_INCLUDE_FLAGS_v22) $(PROVIDER_INCLUDE_FLAGS) -o $@ $(STATIC_PROVIDERLIB_SRCFILES) $(LINK_LIBRARIES)

$(PROVIDER_LIBRARY)_v24: $(STATIC_PROVIDERLIB_SRCFILES) $(INCLUDE_VHOST) $(INCLUDE_VERSION) $(PROVIDER_HEADERS)
	@echo "========================= Performing Building provider (w/HTTP v2.4)"
	$(MKPATH) $(TARGET_DIR)
	g++ $(COMPILE_FLAGS) $(SHARED_FLAGS) $(OMI_INCLUDE_FLAGS) $(MODULE_INCLUDE_FLAGS_v24) $(PROVIDER_INCLUDE_FLAGS) -o $@ $(STATIC_PROVIDERLIB_SRCFILES) $(LINK_LIBRARIES_v24) $(LINK_LIBRARIES)

else

$(PROVIDER_LIBRARY): $(STATIC_PROVIDERLIB_SRCFILES) $(INCLUDE_VHOST) $(INCLUDE_VERSION) $(PROVIDER_HEADERS)
	@echo "========================= Performing Building provider"
	$(MKPATH) $(TARGET_DIR)
	g++ $(COMPILE_FLAGS) $(SHARED_FLAGS) $(OMI_INCLUDE_FLAGS) $(MODULE_INCLUDE_FLAGS) $(PROVIDER_INCLUDE_FLAGS) -o $@ $(STATIC_PROVIDERLIB_SRCFILES) $(LINK_LIBRARIES)

endif

ifeq ($(OPENSOURCE_DISTRO),0)

#--------------------------------------------------------------------------------
# Apache Provider Unit Tests
#
# Build and run the Provider Unit Tests

# For now, providertestutils.cpp lives in PAL, but is built in OM.  This creates
# some build issues.  To resolve, create a link to the real file.

$(PROVIDER_TEST_DIR)/providertestutils.cpp: $(PAL_TESTUTILS_DIR)/providertestutils.cpp
# We need to recreate the symlink if the link is older than what it points to.
	$(RM) $(PROVIDER_TEST_DIR)/providers/providertestutils.cpp
	ln -s $(PAL_TESTUTILS_DIR)/providertestutils.cpp $(PROVIDER_TEST_DIR)/providertestutils.cpp

STATIC_PROVIDER_UNITFILES_INCLUDE = \
	$(PROVIDER_TEST_SUPPORT_DIR)/mmap_builder.h \
	$(PROVIDER_TEST_SUPPORT_DIR)/testableapache.h

STATIC_PROVIDER_UNITFILES = \
	$(PROVIDER_TEST_DIR)/server_test.cpp \
	\
	$(PROVIDER_TEST_DIR)/providertestutils.cpp \
	$(PROVIDER_TEST_SUPPORT_DIR)/mmap_builder.cpp \
	$(PROVIDER_TEST_SUPPORT_DIR)/productdependencies.cpp \
	$(PAL_TESTUTILS_DIR)/scxassert_cppunit.cpp \
	$(PAL_TESTUTILS_DIR)/testrunner.cpp \
	$(PAL_TESTUTILS_DIR)/testrunnerlogpolicy.cpp

$(TARGET_DIR)/testrunner : $(STATIC_PROVIDER_UNITFILES_INCLUDE) $(STATIC_PROVIDER_UNITFILES) $(STATIC_PROVIDERLIB_SRCFILES) $(INCLUDE_VHOST) $(INCLUDE_DEFINES) $(INCLUDE_VERSION) $(PROVIDER_HEADERS) $(PROVIDER_TEST_DIR)/providertestutils.cpp
	@echo "========================= Performing Building provider tests"
	$(MKPATH) $(TARGET_DIR)
	g++ $(COMPILE_FLAGS) $(PROVIDER_TEST_INCLUDE_FLAGS) -o $@ $(STATIC_PROVIDER_UNITFILES) $(STATIC_PROVIDERLIB_SRCFILES) $(LINK_LIBRARIES) $(PROVIDER_TEST_LINK_LIBRARIES)

testrun : test

test : $(SCXPAL_INTERMEDIATE_DIR) $(TARGET_DIR)/testrunner
	@echo "========================= Performing Apache testrun execution"
	LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$(OMI_ROOT)/output/lib $(TARGET_DIR)/testrunner

endif # ifeq ($(OPENSOURCE_DISTRO),0)

#--------------------------------------------------------------------------------
# Apache Module

APACHE_REAL_SOURCE := $(SOURCE_DIR)/module/$(APACHE_MODULE_BASENAME).c
APACHE_TARGET_SOURCE := $(TARGET_DIR)/module/$(APACHE_MODULE_BASENAME).c

$(APACHE_TARGET_SOURCE) : $(APACHE_REAL_SOURCE)
	$(MKPATH) $(TARGET_DIR)/module
	ln -s  $(APACHE_REAL_SOURCE) $(APACHE_TARGET_SOURCE)

ifeq ($(ULINUX),1)

$(APACHE_MODULE)_v22 : $(APACHE_TARGET_SOURCE) $(INCLUDE_VHOST) $(INCLUDE_VERSION)
	@echo "========================= Performing Building module (w/HTTP v2.2)"
	cd $(TARGET_DIR)/module && $(HTTPD_BASE_V22)/bin/$(APXS) -c $(MODULE_INCLUDE_FLAGS_v22) $(APACHE_MODULE_BASENAME).c && cp -v .libs/$(APACHE_MODULE_BASENAME).so $@

$(APACHE_MODULE)_v24 : $(APACHE_TARGET_SOURCE) $(INCLUDE_VHOST) $(INCLUDE_VERSION)
	@echo "========================= Performing Building module (w/HTTP v2.4)"
	cd $(TARGET_DIR)/module && $(HTTPD_BASE_V24)/bin/$(APXS) -c $(MODULE_INCLUDE_FLAGS_v24) $(APACHE_MODULE_BASENAME).c && cp -v .libs/$(APACHE_MODULE_BASENAME).so $@

else

$(APACHE_MODULE) : $(APACHE_TARGET_SOURCE) $(INCLUDE_VHOST) $(INCLUDE_VERSION)
	@echo "========================= Performing Building module"
	cd $(TARGET_DIR)/module && $(APXS) -c $(MODULE_INCLUDE_FLAGS) $(APACHE_MODULE_BASENAME).c && cp -v .libs/$(APACHE_MODULE_BASENAME).so $@

endif

ifeq ($(OPENSOURCE_DISTRO),0)

#--------------------------------------------------------------------------------
# Build the distribution kit
#
# Build the packages via installbuilder
#
# While the "formal build" only builds ULINUX, we may build something else for DEV purposes.
# Assume we ALWAYS build RPM, but only build DPKG if --enable-ulinux is speified in configure.

ifeq ($(ULINUX),1)
APACHE_MODULE_TARGETS := $(APACHE_MODULE)_v22 $(APACHE_MODULE)_v24
PROVIDER_LIBRARY_TARGETS := $(PROVIDER_LIBRARY)_v22 $(PROVIDER_LIBRARY)_v24
else
APACHE_MODULE_TARGETS := $(APACHE_MODULE)
PROVIDER_LIBRARY_TARGETS := $(PROVIDER_LIBRARY)
endif

kit : $(OMI_ROOT)/output $(PROVIDER_LIBRARY_TARGETS) $(APACHE_MODULE_TARGETS)
ifeq ($(ULINUX),1)

	@echo "========================= Performing Building RPM and DPKG packages"
	$(MKPATH) $(INSTALLER_TMPDIR)
	sudo $(RMDIR) $(STAGING_DIR)
	$(MKPATH) $(TARGET_DIR)/apache_22
	python $(SCXPAL_DIR)/installer/InstallBuilder/installbuilder.py \
		--BASE_DIR=$(BASE_DIR) \
		--TARGET_DIR=$(TARGET_DIR) \
		--INTERMEDIATE_DIR=$(INSTALLER_TMPDIR) \
		--STAGING_DIR=$(STAGING_DIR) \
		--BUILD_TYPE=$(BUILD_TYPE) \
		--BUILD_CONFIGURATION=$(BUILD_CONFIGURATION) \
		--PFARCH=$(PF_ARCH) \
		--PFDISTRO=$(PF_DISTRO) \
		--PFMAJOR=$(PF_MAJOR) \
		--PFMINOR=$(PF_MINOR) \
		--VERSION=$(SCX_BUILDVERSION_MAJOR).$(SCX_BUILDVERSION_MINOR).$(SCX_BUILDVERSION_PATCH) \
		--RELEASE=$(SCX_BUILDVERSION_BUILDNR) \
		--DATAFILE_PATH=$(BASE_DIR)/installer/datafiles \
		--HTTP_VERSION=22 \
		base_apache.data linux.data linux_rpm.data

	# The kit has a name including .universalr., and the 'r' is only desired for OM
	sed -e 's/\.universalr\./.universal./' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/`cat $(TARGET_DIR)/package_filename` $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_file.tmp`
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	sudo $(RMDIR) $(STAGING_DIR)
	$(MKPATH) $(TARGET_DIR)/apache_22
	python $(SCXPAL_DIR)/installer/InstallBuilder/installbuilder.py \
		--BASE_DIR=$(BASE_DIR) \
		--TARGET_DIR=$(TARGET_DIR) \
		--INTERMEDIATE_DIR=$(INSTALLER_TMPDIR) \
		--STAGING_DIR=$(STAGING_DIR) \
		--BUILD_TYPE=$(BUILD_TYPE) \
		--BUILD_CONFIGURATION=$(BUILD_CONFIGURATION) \
		--PFARCH=$(PF_ARCH) \
		--PFDISTRO=$(PF_DISTRO) \
		--PFMAJOR=$(PF_MAJOR) \
		--PFMINOR=$(PF_MINOR) \
		--VERSION=$(SCX_BUILDVERSION_MAJOR).$(SCX_BUILDVERSION_MINOR).$(SCX_BUILDVERSION_PATCH) \
		--RELEASE=$(SCX_BUILDVERSION_BUILDNR) \
		--DPKG_LOCATION=$(SCXPAL_DIR)/installer/InstallBuilder/tools/bin/dpkg-deb-$(PF_ARCH) \
		--DATAFILE_PATH=$(BASE_DIR)/installer/datafiles \
		--HTTP_VERSION=22 \
		base_apache.data linux.data linux_dpkg.data

	# The kit has a name including .universald., and the 'd' is only desired for OM
	sed -e 's/\.universald\./.universal./' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/`cat $(TARGET_DIR)/package_filename` $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_file.tmp`
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	$(MKPATH) $(INSTALLER_TMPDIR)
	sudo $(RMDIR) $(STAGING_DIR)
	$(MKPATH) $(TARGET_DIR)/apache_24
	python $(SCXPAL_DIR)/installer/InstallBuilder/installbuilder.py \
		--BASE_DIR=$(BASE_DIR) \
		--TARGET_DIR=$(TARGET_DIR) \
		--INTERMEDIATE_DIR=$(INSTALLER_TMPDIR) \
		--STAGING_DIR=$(STAGING_DIR) \
		--BUILD_TYPE=$(BUILD_TYPE) \
		--BUILD_CONFIGURATION=$(BUILD_CONFIGURATION) \
		--PFARCH=$(PF_ARCH) \
		--PFDISTRO=$(PF_DISTRO) \
		--PFMAJOR=$(PF_MAJOR) \
		--PFMINOR=$(PF_MINOR) \
		--VERSION=$(SCX_BUILDVERSION_MAJOR).$(SCX_BUILDVERSION_MINOR).$(SCX_BUILDVERSION_PATCH) \
		--RELEASE=$(SCX_BUILDVERSION_BUILDNR) \
		--DATAFILE_PATH=$(BASE_DIR)/installer/datafiles \
		--HTTP_VERSION=24 \
		base_apache.data linux.data linux_rpm.data

	# The kit has a name including .universalr., and the 'r' is only desired for OM
	sed -e 's/\.universalr\./.universal./' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/`cat $(TARGET_DIR)/package_filename` $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_file.tmp`
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	sudo $(RMDIR) $(STAGING_DIR)
	$(MKPATH) $(TARGET_DIR)/apache_24
	python $(SCXPAL_DIR)/installer/InstallBuilder/installbuilder.py \
		--BASE_DIR=$(BASE_DIR) \
		--TARGET_DIR=$(TARGET_DIR) \
		--INTERMEDIATE_DIR=$(INSTALLER_TMPDIR) \
		--STAGING_DIR=$(STAGING_DIR) \
		--BUILD_TYPE=$(BUILD_TYPE) \
		--BUILD_CONFIGURATION=$(BUILD_CONFIGURATION) \
		--PFARCH=$(PF_ARCH) \
		--PFDISTRO=$(PF_DISTRO) \
		--PFMAJOR=$(PF_MAJOR) \
		--PFMINOR=$(PF_MINOR) \
		--VERSION=$(SCX_BUILDVERSION_MAJOR).$(SCX_BUILDVERSION_MINOR).$(SCX_BUILDVERSION_PATCH) \
		--RELEASE=$(SCX_BUILDVERSION_BUILDNR) \
		--DPKG_LOCATION=$(SCXPAL_DIR)/installer/InstallBuilder/tools/bin/dpkg-deb-$(PF_ARCH) \
		--DATAFILE_PATH=$(BASE_DIR)/installer/datafiles \
		--HTTP_VERSION=24 \
		base_apache.data linux.data linux_dpkg.data

	# The kit has a name including .universald., and the 'd' is only desired for OM
	sed -e 's/\.universald\./.universal./' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/`cat $(TARGET_DIR)/package_filename` $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_file.tmp`
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	# Strip the package extension from the package filename
	sed -re 's/.rpm$$|.deb$$//' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	# The MP needs architecture of i686 (for x86) and x86_64 (for x64).
	sed -e 's/\.x86$$/\.i686/' $(TARGET_DIR)/package_filename | sed 's/\.x64$$/.x86_64/' > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_filename`.rpm $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_file.tmp`.rpm
	mv $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_filename`.deb $(TARGET_DIR)/apache_22/`cat $(TARGET_DIR)/package_file.tmp`.deb
	mv $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_filename`.rpm $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_file.tmp`.rpm
	mv $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_filename`.deb $(TARGET_DIR)/apache_24/`cat $(TARGET_DIR)/package_file.tmp`.deb
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	# Build the tar file containing both .rpm and .deb packages
	cd $(TARGET_DIR); tar cvf `cat $(TARGET_DIR)/package_filename`.tar apache_22/`cat $(TARGET_DIR)/package_filename`.rpm apache_22/`cat $(TARGET_DIR)/package_filename`.deb apache_24/`cat $(TARGET_DIR)/package_filename`.rpm apache_24/`cat $(TARGET_DIR)/package_filename`.deb

else

	@echo "========================= Performing Building RPM and DPKG packages"
	$(MKPATH) $(INSTALLER_TMPDIR)
	sudo $(RMDIR) $(STAGING_DIR)
	python $(SCXPAL_DIR)/installer/InstallBuilder/installbuilder.py \
		--BASE_DIR=$(BASE_DIR) \
		--TARGET_DIR=$(TARGET_DIR) \
		--INTERMEDIATE_DIR=$(INSTALLER_TMPDIR) \
		--STAGING_DIR=$(STAGING_DIR) \
		--BUILD_TYPE=$(BUILD_TYPE) \
		--BUILD_CONFIGURATION=$(BUILD_CONFIGURATION) \
		--PFARCH=$(PF_ARCH) \
		--PFDISTRO=$(PF_DISTRO) \
		--PFMAJOR=$(PF_MAJOR) \
		--PFMINOR=$(PF_MINOR) \
		--VERSION=$(SCX_BUILDVERSION_MAJOR).$(SCX_BUILDVERSION_MINOR).$(SCX_BUILDVERSION_PATCH) \
		--RELEASE=$(SCX_BUILDVERSION_BUILDNR) \
		--DATAFILE_PATH=$(BASE_DIR)/installer/datafiles \
		base_apache.data linux.data linux_rpm.data

	# Strip the package extension from the package filename
	sed -re 's/.rpm$$|.deb$$//' $(TARGET_DIR)/package_filename > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	# The MP needs architecture of i686 (for x86) and x86_64 (for x64).
	sed -e 's/\.x86$$/\.i686/' $(TARGET_DIR)/package_filename | sed 's/\.x64$$/.x86_64/' > $(TARGET_DIR)/package_file.tmp
	mv $(TARGET_DIR)/`cat $(TARGET_DIR)/package_filename`.rpm $(TARGET_DIR)/`cat $(TARGET_DIR)/package_file.tmp`.rpm
	mv $(TARGET_DIR)/package_file.tmp $(TARGET_DIR)/package_filename

	# Build the tar file containing the .rpm package
	cd $(TARGET_DIR); tar cvf `cat $(TARGET_DIR)/package_filename`.tar `cat $(TARGET_DIR)/package_filename`.rpm

endif

	../installer/bundle/create_bundle.sh $(PF)_$(PF_DISTRO) \
                                             $(TARGET_DIR) \
                                             `cat $(TARGET_DIR)/package_filename`

endif # ifeq ($(OPENSOURCE_DISTRO),0)

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
	$(MKPATH) $(TARGET_DIR)/omi
	cd $(TARGET_DIR)/omi; $(OMIGEN) $(MOF_DIR)/apache.mof -d $(PROVIDER_DIR) $(CLASSES) --cpp -I $(OMI_DIR)/share/omischema
	$(RM) ~/.omigenrc

omi_reg :
	$(MKPATH) $(TARGET_DIR)/omi
	$(MKPATH) $(OMI_DIR)/etc/omiregister/root-apache
	export LD_LIBRARY_PATH=$(LD_LIBRARY_PATH):$(OMI_LIBDIR); cd $(TARGET_DIR)/omi; $(OMI_BINDIR)/omireg -n root/apache -o root $(PROVIDER_LIBRARY)
	cp -r $(OMI_DIR)/etc/omiregister/root-apache $(TARGET_DIR)/omi

endif # $(PERFORM_OMI_MAKEINSTALL),1
