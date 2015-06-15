/*
 *--------------------------------- START OF LICENSE ----------------------------
 *
 * Apache Cimprov ver. 1.0
 *
 * Copyright (c) Microsoft Corporation
 *
 * All rights reserved. 
 *
 * Licensed under the Apache License, Version 2.0 (the License); you may not use
 * this file except in compliance with the license. You may obtain a copy of the
 * License at http://www.apache.org/licenses/LICENSE-2.0 
 *
 * THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
 * WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
 * MERCHANTABLITY OR NON-INFRINGEMENT.
 *
 * See the Apache Version 2.0 License for specific language governing permissions
 * and limitations under the License.
 *
 *---------------------------------- END OF LICENSE -----------------------------
 */
/**
      \file        utils.h

      \brief       Utility functions for dealing with Apache interface

      \date        05-27-14
*/
/*----------------------------------------------------------------------------*/

#include "utils.h"
#include <string.h>
#include <apr_strings.h>

static char s_buf[32];

/* Get a component version from the Apache version string.
 * The version string consists of space-separated fields of the form:
 *     Component/Version
 * Version is typically a.b.c, which is compatible with the CIM version
 * number standard.
 */

const char* GetApacheComponentVersion(
    const char* versionString,
    const char* component)
{
    char componentHdr[32];
    const char* ptr;
    size_t n;

    apr_cpystrn(componentHdr, component, sizeof componentHdr - 1);
    strcat(componentHdr, "/");
    ptr = strstr(versionString, componentHdr);
    if (ptr == NULL)
    {
        return "0.0.0";
    }
    ptr += strlen(componentHdr);
    n = 0;
    while ((unsigned char)*(ptr + n) > ' ')
    {
        n++;
    }
    strncpy(s_buf, ptr, n);
    s_buf[n]= '\0';
    return s_buf;
}


/*----------------------------------------------------------------------------*/
/**
   Convert string to all lowercase

   \param       str  String to convert

   \returns          Converted string
*/
std::string StrToLower(const std::string& str)
{
    std::string tmp_str(str);

    for (std::string::size_type i = 0; i < tmp_str.size(); i++)
    {
        tmp_str[i] = tolower( (int) (unsigned char) tmp_str[i] );
    }

    return tmp_str;
}
