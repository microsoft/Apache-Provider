/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
*/
/**
    \file        productdependencies.cpp

    \brief       Implements dummy product dependencies for logging subsystem of PAL

    \date        2013-02-26 13:38:00
*/
/*----------------------------------------------------------------------------*/

#include <scxcorelib/scxcmn.h>
#include <scxcorelib/stringaid.h>
#include <scxcorelib/scxproductdependencies.h>
#include <buildversion.h>

namespace SCXCoreLib
{
    namespace SCXProductDependencies
    {
        void WriteLogFileHeader( SCXHandle<std::wfstream> &stream, int logFileRunningNumber, SCXCalendarTime& procStartTimestamp )
        {
            std::wstringstream continuationLogMsg;
            if ( logFileRunningNumber > 1 )
            {
                continuationLogMsg << L"* Log file number: " << StrFrom(logFileRunningNumber) << std::endl;
            }

            (*stream) << L"*" << std::endl
                      << L"* Microsoft Apache Test Framework" << std::endl
#if !defined(WIN32)
                      << L"* Build number: " << CIMPROV_BUILDVERSION_MAJOR << L"." << CIMPROV_BUILDVERSION_MINOR
                      << L"." << CIMPROV_BUILDVERSION_PATCH
                      << L"-" << CIMPROV_BUILDVERSION_BUILDNR
                      << L" " << StrFromUTF8(CIMPROV_BUILDVERSION_STATUS) << std::endl
#endif
                      << L"* Process started: " << procStartTimestamp.ToExtendedISO8601() << std::endl
                      << continuationLogMsg.str() 
                      << L"*" << std::endl
                      << L"* Log format: <date> <severity>     [<code module>:<line number>:<process id>:<thread id>] <message>" << std::endl
                      << L"*" << std::endl;
        }

        void WrtieItemToLog( SCXHandle<std::wfstream> &stream, const SCXLogItem& item, const std::wstring& message )
        {
            (void) item;

            (*stream) << message << std::endl;
        }
    }
}
