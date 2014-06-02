/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
*/
/**
    \file        mmap_builder.h

    \brief       Test class to assist in building "fake" memory map for unit test purposes

    \date        2014-05-29 08:42:00 AM
*/
/*----------------------------------------------------------------------------*/

#include <string.h>
#include <vector>

#include "apr.h"

#include "mmap_region.h"
#include "temppool.h"

class TestStringTable
{
public:
    TestStringTable() { m_data.push_back('\0'); }
    ~TestStringTable() {}

    apr_size_t InsertString(const char *string);
    const char *GetString(apr_size_t offset) { return &m_data[offset]; }

    mmap_string_table* GenerateStringTable(apr_pool_t* p);

    // For test purposes only
    apr_size_t size() { return m_data.size(); }

private:
    std::vector<char> m_data;
};

class TestServerData
{
public:
    TestServerData(TestStringTable& stringTable)
        : m_generatedServerData(NULL), m_stringTable(stringTable)
    {
        memset(&m_server, '\0', sizeof(m_server));
    }
    ~TestServerData() {}

    void SetConfigFile(const char *configFile) { SetStringHelper(configFile, m_server.configFileOffset); }
    void SetProcessName(const char *processName) { SetStringHelper(processName, m_server.processNameOffset); }
    void SetServerVersion(const char *serverVersion) { SetStringHelper(serverVersion, m_server.serverVersionOffset); }
    void SetServerRoot(const char *root) { SetStringHelper(root, m_server.serverRootOffset); }
    void SetServerID(const char *id) { SetStringHelper(id, m_server.serverIDOffset); }
    void SetOperatingStatus(int operatingStatus) { m_server.operatingStatus = operatingStatus; }

    const char* GetConfigFile() { return m_stringTable.GetString(m_server.configFileOffset); }
    const char* GetProcessName() { return m_stringTable.GetString(m_server.processNameOffset); }
    const char* GetServerVersion() { return m_stringTable.GetString(m_server.serverVersionOffset); }
    const char* GetServerRoot() { return m_stringTable.GetString(m_server.serverRootOffset); }
    const char* GetServerID() { return m_stringTable.GetString(m_server.serverIDOffset); }

    void AddModule(const char *name);
    apr_size_t GetModule(apr_size_t entry);

    mmap_server_data* GenerateServerMap(apr_pool_t* p);

private:
    void SetStringHelper(const char *str, apr_size_t& offset);

    mmap_server_data m_server;
    std::vector<mmap_server_modules> m_modules;

    mmap_server_data *m_generatedServerData;

    TestStringTable& m_stringTable;
};

// Generate some sample server data for test purposes
void GenerateSampleServerData(TestServerData& s);

// Mock the memory map that's normally supplied by Apache
void GenerateMemoryMap(TemporaryPool &p, TestServerData& svr, TestStringTable& str);
