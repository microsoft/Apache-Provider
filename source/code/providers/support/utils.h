/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        utils.h

      \brief       Utility functions for dealing with Apache interface

      \date        05-27-14
*/
/*----------------------------------------------------------------------------*/

#ifndef UTILS_H
#define UTILS_H

#include <string>

const char* GetApacheComponentVersion(const char* versionString, const char* component);
std::string StrToLower(const std::string& str);

#endif /* UTILS_H */
