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
