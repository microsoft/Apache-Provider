/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
*/
/**
    \file        mmap_builder.cpp

    \brief       Test class to assist in building "fake" memory map for unit test purposes

    \date        2014-05-29 08:42:00 AM
*/
/*----------------------------------------------------------------------------*/

#include <string>

#include "testutils/scxunit.h"
#include "apachebinding.h"
#include "mmap_builder.h"


apr_size_t TestStringTable::InsertString(const char *string)
{
    apr_size_t offset = m_data.size();
    apr_size_t len = strlen(string) + 1;

    m_data.reserve(len);
    for (apr_size_t i = 0; i < len; i++ )
        m_data.push_back(string[i]);

    return offset;
}

mmap_string_table* TestStringTable::GenerateStringTable(apr_pool_t* p)
{
    apr_size_t tableSize = m_data.size() + sizeof(mmap_string_table);
    mmap_string_table* table = static_cast<mmap_string_table*>(apr_palloc(p, tableSize));

    table->total_length = m_data.size();
    memcpy( &table->data[0], &m_data[0], m_data.size() );

    return table;
}



void TestServerData::SetStringHelper(const char *str, apr_size_t& offset)
{
    // Verify that persistent data hasn't yet been generated
    CPPUNIT_ASSERT_EQUAL(static_cast<void *>(NULL), static_cast<void *>(m_generatedServerData));

    // Set the parameter
    offset = m_stringTable.InsertString(str);
}

void TestServerData::AddModule(const char *name)
{
    mmap_server_modules m;

    // Verify that persistent data hasn't yet been generated
    CPPUNIT_ASSERT_EQUAL(static_cast<void *>(NULL), static_cast<void *>(m_generatedServerData));

    // Add the module
    m.moduleNameOffset = m_stringTable.InsertString(name);
    m_modules.push_back(m);
}

apr_size_t TestServerData::GetModule(apr_size_t entry)
{
    // Out of range?
    if (m_modules.size() == 0 || entry >= m_modules.size())
    {
        return ULONG_MAX;
    }

    return m_modules[entry].moduleNameOffset;
}

mmap_server_data* TestServerData::GenerateServerMap(apr_pool_t* p)
{
    // Verify that persistent data hasn't yet been generated
    CPPUNIT_ASSERT_EQUAL(static_cast<void *>(NULL), static_cast<void *>(m_generatedServerData));

    apr_size_t moduleSize = m_modules.size() * sizeof(mmap_server_modules);
    apr_size_t tableSize = sizeof(m_server) + moduleSize;
    mmap_server_data* table = static_cast<mmap_server_data*>(apr_palloc(p, tableSize));

    memcpy( table, &m_server, sizeof(mmap_server_data) );
    memcpy( &table->modules, &m_modules[0], moduleSize );
    table->moduleCount = m_modules.size();

    m_generatedServerData = table;

    return table;
}



void GenerateSampleServerData(TestServerData& s)
{
    // Set some server information
    s.SetConfigFile("/etc/httpd/conf/httpd.conf-fake");
    s.SetProcessName("httpd-fake");
    s.SetServerVersion("Apache/1.2.3");
    s.SetServerRoot("/etc/httpd-fake");
    s.SetServerID("jeffcof64-rhel6-01.fake.com");

    // Add some modules (actual data from dev system)
    s.AddModule("mod_cimprov.c");
    s.AddModule("mod_ssl.c");
    s.AddModule("mod_dnssd.c");
    s.AddModule("mod_version.c");
    s.AddModule("mod_cgi.c");
    s.AddModule("mod_disk_cache.c");
    s.AddModule("mod_suexec.c");
    s.AddModule("mod_cache.c");
    s.AddModule("mod_proxy_connect.c");
    s.AddModule("mod_proxy_ajp.c");
    s.AddModule("mod_proxy_http.c");
    s.AddModule("mod_proxy_ftp.c");
    s.AddModule("mod_proxy_balancer.c");
    s.AddModule("mod_proxy.c");
    s.AddModule("mod_rewrite.c");
    s.AddModule("mod_substitute.c");
    s.AddModule("mod_alias.c");
    s.AddModule("mod_userdir.c");
    s.AddModule("mod_speling.c");
    s.AddModule("mod_actions.c");
    s.AddModule("mod_dir.c");
    s.AddModule("mod_negotiation.c");
    s.AddModule("mod_vhost_alias.c");
    s.AddModule("mod_dav_fs.c");
    s.AddModule("mod_info.c");
    s.AddModule("mod_autoindex.c");
    s.AddModule("mod_status.c");
    s.AddModule("mod_dav.c");
    s.AddModule("mod_mime.c");
    s.AddModule("mod_setenvif.c");
    s.AddModule("mod_usertrack.c");
    s.AddModule("mod_headers.c");
    s.AddModule("mod_deflate.c");
    s.AddModule("mod_expires.c");
    s.AddModule("mod_mime_magic.c");
    s.AddModule("mod_ext_filter.c");
    s.AddModule("mod_env.c");
    s.AddModule("mod_logio.c");
    s.AddModule("mod_log_config.c");
    s.AddModule("mod_include.c");
    s.AddModule("mod_authnz_ldap.c");
    s.AddModule("util_ldap.c");
    s.AddModule("mod_authz_default.c");
    s.AddModule("mod_authz_dbm.c");
    s.AddModule("mod_authz_groupfile.c");
    s.AddModule("mod_authz_owner.c");
    s.AddModule("mod_authz_user.c");
    s.AddModule("mod_authz_host.c");
    s.AddModule("mod_authn_default.c");
    s.AddModule("mod_authn_dbm.c");
    s.AddModule("mod_authn_anon.c");
    s.AddModule("mod_authn_alias.c");
    s.AddModule("mod_authn_file.c");
    s.AddModule("mod_auth_digest.c");
    s.AddModule("mod_auth_basic.c");
    s.AddModule("mod_so.c");
    s.AddModule("http_core.c");
    s.AddModule("prefork.c");
    s.AddModule("core.c");
}

void GenerateMemoryMap(TemporaryPool& p, TestServerData& svr, TestStringTable& str)
{
    apr_pool_t* pool = p.Get();

    mmap_server_data* serverMap = svr.GenerateServerMap(pool);
    mmap_string_table* stringTab = str.GenerateStringTable(pool);

    g_apache.SetMemoryMap(serverMap, stringTab);
}
