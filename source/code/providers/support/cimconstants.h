/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        cimconstants.h

      \brief       Constants for CIM classes that describe computer environment

      \date        05-23-14
*/
/*----------------------------------------------------------------------------*/

#ifndef CIMCONSTANTS_H
#define CIMCONSTANTS_H

// Value of the vendor ID for Apache

#define APACHE_VENDOR_ID "Apache Software Foundation"

// Values for software element state

// #define CIM_SOFTWARE_ELEMENT_STATE_DEPLOYABLE 0
// #define CIM_SOFTWARE_ELEMENT_STATE_INSTALLABLE 1
// #define CIM_SOFTWARE_ELEMENT_STATE_EXECUTABLE 2
#define CIM_SOFTWARE_ELEMENT_STATE_RUNNING 3

// Values for target operating system

#define CIM_TARGET_OPERATING_SYSTEM_UNKNOWN 0
// #define CIM_TARGET_OPERATING_SYSTEM_HPUX 8
// #define CIM_TARGET_OPERATING_SYSTEM_AIX 9
// #define CIM_TARGET_OPERATING_SYSTEM_WINDOWS_NT 18
// #define CIM_TARGET_OPERATING_SYSTEM_WINDOWS_CE 19
// #define CIM_TARGET_OPERATING_SYSTEM_SOLARIS 29
#define CIM_TARGET_OPERATING_SYSTEM_LINUX 36
// #define CIM_TARGET_OPERATING_SYSTEM_BSD 41
// #define CIM_TARGET_OPERATING_SYSTEM_FREE_BSD 42
// #define CIM_TARGET_OPERATING_SYSTEM_NET_BSD 43
 
#if defined(linux)
# define CIM_TARGET_OPERATING_SYSTEM CIM_TARGET_OPERATING_SYSTEM_LINUX
#else
# define CIM_TARGET_OPERATING_SYSTEM CIM_TARGET_OPERATING_SYSTEM_UNKNOWN
#endif

#endif /* CIMCONSTANTS_H */

/*----------------------------E-N-D---O-F---F-I-L-E---------------------------*/
