/*--------------------------------------------------------------------------------
    Copyright (c) Microsoft Corporation.  All rights reserved.

    Created date    2014-05-23 08:31:00

    Apache_HTTPDServer_Class_Provider unit tests.

    Only tests the functionality of the provider class.

*/
/*----------------------------------------------------------------------------*/

#include <scxcorelib/scxcmn.h>
#include <scxcorelib/scxexception.h>
#include <scxcorelib/stringaid.h>
#include <testutils/scxunit.h>
#include <testutils/providertestutils.h>

#include "Apache_HTTPDServer_Class_Provider.h"
#include "apachebinding.h"
#include "testableapache.h"
#include "mmap_builder.h"

#include <iostream> // for cout

class TestableDataCollectorDepsFailsLoadMemoryMap : public TestableApacheDataCollectorDependencies
{
    virtual apr_status_t LoadMemoryMap(mmap_server_data** svr,
                                       mmap_vhost_data** vhost,
                                       mmap_certificate_data** cert,
                                       mmap_string_table** str)
    { return APR_ENOENT; }
};

class TestableFactoryWithLoadMemoryMapFailure : public TestableApacheFactory
{
public:
    virtual ApacheDataCollector DataCollectorFactory()
    {
        TestableDataCollectorDepsFailsLoadMemoryMap* pDeps = new TestableDataCollectorDepsFailsLoadMemoryMap();
        return ApacheDataCollector( pDeps );
    }
};


class TestableInitDepsFailsCroakedApache : public TestableApacheInitDependencies
{
    virtual apr_status_t ValidateSharedMemory(ApacheDataCollector& data) { return APR_ENOENT; }
    virtual bool IsSharedMemoryValid() { return false; }
};

class TestableFactoryWithCroakedApache : public TestableApacheFactory
{
public:
    virtual ApacheDataCollector DataCollectorFactory()
    {
        TestableDataCollectorDepsFailsLoadMemoryMap* pDeps = new TestableDataCollectorDepsFailsLoadMemoryMap();
        return ApacheDataCollector( pDeps );
    }

    virtual ApacheInitialization* InitializationFactory()
    { return new ApacheInitialization( new TestableInitDepsFailsCroakedApache() ); }
};


class Apache_HTTPDServer_Test : public CPPUNIT_NS::TestFixture
{
    CPPUNIT_TEST_SUITE( Apache_HTTPDServer_Test );

    // Tests for low-level test functions - this is as good a place as any
    CPPUNIT_TEST( testInsertStringIntoTable );
    CPPUNIT_TEST( testInsertModules );

    // Now test the actual production code
    CPPUNIT_TEST( TestGetConfigFile );
    CPPUNIT_TEST( TestAttachFailsIfLoadMemoryMapFails );
    CPPUNIT_TEST( TestEnumerateInstancesKeysOnly );
    CPPUNIT_TEST( TestEnumerateInstancesWithDeadApacheServer );
/*
    CPPUNIT_TEST( TestEnumerateInstances );
    CPPUNIT_TEST( TestVerifyKeyCompletePartial );
    CPPUNIT_TEST( TestGetInstance );

    SCXUNIT_TEST_ATTRIBUTE(TestEnumerateInstancesKeysOnly, SLOW);
    SCXUNIT_TEST_ATTRIBUTE(TestEnumerateInstances, SLOW);
    SCXUNIT_TEST_ATTRIBUTE(TestVerifyKeyCompletePartial, SLOW);
    SCXUNIT_TEST_ATTRIBUTE(TestGetInstance, SLOW);
*/
    CPPUNIT_TEST_SUITE_END();

private:
    std::vector<std::wstring> m_keyNames;

public:
    void setUp(void)
    {
        g_pFactory = new TestableApacheFactory();

        std::wstring errMsg;
        TestableContext context;
        SetUpAgent<mi::Apache_HTTPDServer_Class_Provider>(context, CALL_LOCATION(errMsg));
        CPPUNIT_ASSERT_EQUAL_MESSAGE(ERROR_MESSAGE, true, context.WasRefuseUnloadCalled() );

        m_keyNames.push_back(L"ProductIdentifyingNumber");
        m_keyNames.push_back(L"ProductName");
        m_keyNames.push_back(L"ProductVendor");
        m_keyNames.push_back(L"ProductVersion");
        m_keyNames.push_back(L"SystemID");
        m_keyNames.push_back(L"CollectionID");
    }

    void tearDown(void)
    {
        std::wstring errMsg;
        TestableContext context;
        TearDownAgent<mi::Apache_HTTPDServer_Class_Provider>(context, CALL_LOCATION(errMsg));
        CPPUNIT_ASSERT_EQUAL_MESSAGE(ERROR_MESSAGE, false, context.WasRefuseUnloadCalled() );

        delete g_pFactory;
        g_pFactory = NULL;
    }

    void testInsertStringIntoTable()
    {
        TestStringTable t;
        size_t oldSize, offset;

        // Validate empty table is empty
        oldSize = t.size();
        CPPUNIT_ASSERT_EQUAL(static_cast<size_t>(1), oldSize);

        // Insert one string, make sure all is good
        offset = t.InsertString("0123456789ABCDEF");
        CPPUNIT_ASSERT_EQUAL(static_cast<size_t>(1), offset);
        CPPUNIT_ASSERT_EQUAL(static_cast<size_t>(18), t.size());
        CPPUNIT_ASSERT_EQUAL(0, strcmp("0123456789ABCDEF", t.GetString(offset)));

        // Insert additional string, make sure new string is good
        offset = t.InsertString("GHIJKL");
        CPPUNIT_ASSERT_EQUAL(static_cast<size_t>(25), t.size());
        CPPUNIT_ASSERT_EQUAL(0, strcmp("GHIJKL", t.GetString(offset)));

        // And verify that original string is still good too
        CPPUNIT_ASSERT_EQUAL(0, strcmp("0123456789ABCDEF", t.GetString(1)));
    }

    void testInsertModules()
    {
        TestStringTable t;
        TestServerData s(t);

        // Insert some data just to populate string table
        s.SetConfigFile("/etc/httpd/conf/httpd.conf");
        s.SetServerVersion("Apache/1.2.3");

        CPPUNIT_ASSERT_EQUAL(std::string("/etc/httpd/conf/httpd.conf"), s.GetConfigFile() );
        CPPUNIT_ASSERT_EQUAL(std::string("Apache/1.2.3"), s.GetServerVersion() );

        // Verify that no modules return ULONG_MAX
        CPPUNIT_ASSERT_EQUAL(ULONG_MAX, s.GetModule(0));
        CPPUNIT_ASSERT_EQUAL(ULONG_MAX, s.GetModule(1));

        // Add a few modules to the server
        s.AddModule("mod_cimprov.c");
        s.AddModule("mod_ssl.c");
        s.AddModule("mod_dnssd.c");
        s.AddModule("mod_version.c");
        s.AddModule("mod_cgi.c");

        // Now make sure we can get 'em back
        CPPUNIT_ASSERT_EQUAL(ULONG_MAX, s.GetModule(5));
        CPPUNIT_ASSERT_EQUAL(0, strcmp("mod_cgi.c", t.GetString(s.GetModule(4))));
        CPPUNIT_ASSERT_EQUAL(0, strcmp("mod_version.c", t.GetString(s.GetModule(3))));
        CPPUNIT_ASSERT_EQUAL(0, strcmp("mod_dnssd.c", t.GetString(s.GetModule(2))));
        CPPUNIT_ASSERT_EQUAL(0, strcmp("mod_ssl.c", t.GetString(s.GetModule(1))));
        CPPUNIT_ASSERT_EQUAL(0, strcmp("mod_cimprov.c", t.GetString(s.GetModule(0))));
    }

    void TestGetConfigFile()
    {
        // Test production code version of GetServerConfigFile (verify NULL not returned)
        TemporaryPool pool(g_pFactory->GetInit()->GetPool());
        ApacheInitDependencies deps;

        const char* configFile = deps.GetServerConfigFile(pool.Get());
        std::cout << ": " << ( configFile ? configFile : "NULL" );

        CPPUNIT_ASSERT(configFile != NULL);
    }

    void TestAttachFailsIfLoadMemoryMapFails()
    {
        TestableFactoryWithLoadMemoryMapFailure* pFactory = new TestableFactoryWithLoadMemoryMapFailure();

        // Watch scoping; ApacheDataCollector must destruct prior to deleting the factory
        {
            ApacheDataCollector data = pFactory->DataCollectorFactory();
            apr_status_t status = data.Attach("TestAttachFailsIfLoadMemoryMapFails");
            CPPUNIT_ASSERT_EQUAL(APR_ENOENT, status);
        }

        delete pFactory;
    }

    void TestEnumerateInstancesKeysOnly()
    {
        TemporaryPool pool(g_pFactory->GetInit()->GetPool());

        TestStringTable strTab;
        TestServerData serverTab(strTab);
        GenerateSampleServerData(serverTab);
        GenerateMemoryMap(pool, serverTab, strTab);

        std::wstring errMsg;
        TestableContext context;
        StandardTestEnumerateKeysOnly<mi::Apache_HTTPDServer_Class_Provider>(
            m_keyNames, context, CALL_LOCATION(errMsg));
        CPPUNIT_ASSERT_EQUAL(1u, context.Size());

        CPPUNIT_ASSERT_EQUAL(std::wstring(L"1"),
                             context[0].GetKey(L"ProductIdentifyingNumber", CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL(std::wstring(L"/etc/httpd/conf/httpd.conf-fake"),
                             context[0].GetKey(L"ProductName", CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL(std::wstring(L"Apache Software Foundation"),
                             context[0].GetKey(L"ProductVendor", CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL(std::wstring(L"1.2.3"),
                             context[0].GetKey(L"ProductVersion", CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL(std::wstring(L"jeffcof64-rhel6-01.fake.com"),
                             context[0].GetKey(L"SystemID", CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL(std::wstring(L"/etc/httpd-fake"),
                             context[0].GetKey(L"CollectionID", CALL_LOCATION(errMsg)));
    }

    void TestEnumerateInstancesWithDeadApacheServer()
    {
        // We have our own factory for this ...
        // Squirrel away the existing global pointer, restore later
        ApacheFactory* saved_g_pFactory = g_pFactory;
        g_pFactory = new TestableFactoryWithCroakedApache();

        // Watch scoping; ApacheDataCollector must destruct prior to deleting the factory
        {
            TemporaryPool pool(g_pFactory->GetInit()->GetPool());

            std::wstring errMsg;
            TestableContext context;
            StandardTestEnumerateKeysOnly<mi::Apache_HTTPDServer_Class_Provider>(
                m_keyNames, context, CALL_LOCATION(errMsg));
            CPPUNIT_ASSERT_EQUAL(1u, context.Size());

            CPPUNIT_ASSERT_EQUAL(std::wstring(L"1"),
                                 context[0].GetKey(L"ProductIdentifyingNumber", CALL_LOCATION(errMsg)));
            CPPUNIT_ASSERT_EQUAL(std::wstring(L"Unknown"),
                                 context[0].GetKey(L"ProductName", CALL_LOCATION(errMsg)));
            CPPUNIT_ASSERT_EQUAL(std::wstring(L"Apache Software Foundation"),
                                 context[0].GetKey(L"ProductVendor", CALL_LOCATION(errMsg)));
            CPPUNIT_ASSERT_EQUAL(std::wstring(L"Unknown"),
                                 context[0].GetKey(L"ProductVersion", CALL_LOCATION(errMsg)));
            CPPUNIT_ASSERT_EQUAL(std::wstring(L"Unknown"),
                                 context[0].GetKey(L"SystemID", CALL_LOCATION(errMsg)));
            CPPUNIT_ASSERT_EQUAL(std::wstring(L"Unknown"),
                                 context[0].GetKey(L"CollectionID", CALL_LOCATION(errMsg)));
        }

        delete g_pFactory;
        g_pFactory = saved_g_pFactory;
    }

/*
    void TestEnumerateInstances()
    {
        std::wstring errMsg;
        TestableContext context;
        StandardTestEnumerateInstances<mi::SCX_MemoryStatisticalInformation_Class_Provider>(
            m_keyNames, context, CALL_LOCATION(errMsg));
        CPPUNIT_ASSERT_EQUAL(1u, context.Size());
        ValidateInstance(context, CALL_LOCATION(errMsg));
    }

    void TestVerifyKeyCompletePartial()
    {
        std::wstring errMsg;
        StandardTestVerifyGetInstanceKeys<mi::SCX_MemoryStatisticalInformation_Class_Provider,
                mi::SCX_MemoryStatisticalInformation_Class>(m_keyNames, CALL_LOCATION(errMsg));
    }

    void TestGetInstance()
    {
        std::wstring errMsg;

        std::vector<std::wstring> keyValues;
        keyValues.push_back(L"Memory");
        TestableContext context;
        CPPUNIT_ASSERT_EQUAL(MI_RESULT_OK, (GetInstance<mi::SCX_MemoryStatisticalInformation_Class_Provider,
            mi::SCX_MemoryStatisticalInformation_Class>(m_keyNames, keyValues, context, CALL_LOCATION(errMsg))));
        ValidateInstance(context, CALL_LOCATION(errMsg));
    }

    void ValidateInstance(const TestableContext& context, std::wstring errMsg)
    {
        CPPUNIT_ASSERT_EQUAL(1u, context.Size());// This provider has only one instance.
        const TestableInstance &instance = context[0];
        
        CPPUNIT_ASSERT_EQUAL_MESSAGE(ERROR_MESSAGE, m_keyNames[0], instance.GetKeyName(0, CALL_LOCATION(errMsg)));
        CPPUNIT_ASSERT_EQUAL_MESSAGE(ERROR_MESSAGE, L"Memory", instance.GetKeyValue(0, CALL_LOCATION(errMsg)));

        std::wstring tmpExpectedProperties[] = {L"Caption",
                                                  L"Description",
                                                  L"Name",
                                                  L"IsAggregate",
                                                  L"AvailableMemory",
                                                  L"PercentAvailableMemory",
                                                  L"UsedMemory",
                                                  L"PercentUsedMemory",
                                                  L"PagesPerSec",
                                                  L"PagesReadPerSec",
                                                  L"PagesWrittenPerSec",
                                                  L"AvailableSwap",
                                                  L"PercentAvailableSwap",
                                                  L"UsedSwap",
                                                  L"PercentUsedSwap",
                                                  L"PercentUsedByCache"};

        const size_t numprops = sizeof(tmpExpectedProperties) / sizeof(tmpExpectedProperties[0]);
        VerifyInstancePropertyNames(instance, tmpExpectedProperties, numprops, CALL_LOCATION(errMsg));

        // Test that the percentages add up to about 100%.
        TestableInstance::PropertyInfo info;
        CPPUNIT_ASSERT_EQUAL(MI_RESULT_OK, instance.FindProperty(L"PercentAvailableMemory", info));
        MI_Uint8 percentAvailableMemory = info.GetValue_MIUint8(CALL_LOCATION(errMsg));
        CPPUNIT_ASSERT_EQUAL(MI_RESULT_OK, instance.FindProperty(L"PercentUsedMemory", info));
        MI_Uint8 percentUsedMemory = info.GetValue_MIUint8(CALL_LOCATION(errMsg));

        SCXUNIT_ASSERT_BETWEEN(static_cast<unsigned int>(percentAvailableMemory) + percentUsedMemory, 98, 102);
    }
*/
};

CPPUNIT_TEST_SUITE_REGISTRATION( Apache_HTTPDServer_Test );
