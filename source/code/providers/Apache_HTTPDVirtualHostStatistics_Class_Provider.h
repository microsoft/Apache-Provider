/* @migen@ */
#ifndef _Apache_HTTPDVirtualHostStatistics_Class_Provider_h
#define _Apache_HTTPDVirtualHostStatistics_Class_Provider_h

#include "Apache_HTTPDVirtualHostStatistics.h"
#ifdef __cplusplus
# include <micxx/micxx.h>
# include "module.h"

MI_BEGIN_NAMESPACE

/*
**==============================================================================
**
** Apache_HTTPDVirtualHostStatistics provider class declaration
**
**==============================================================================
*/

class Apache_HTTPDVirtualHostStatistics_Class_Provider
{
/* @MIGEN.BEGIN@ CAUTION: PLEASE DO NOT EDIT OR DELETE THIS LINE. */
private:
    Module* m_Module;

public:
    Apache_HTTPDVirtualHostStatistics_Class_Provider(
        Module* module);

    ~Apache_HTTPDVirtualHostStatistics_Class_Provider();

    void Load(
        Context& context);

    void Unload(
        Context& context);

    void EnumerateInstances(
        Context& context,
        const String& nameSpace,
        const PropertySet& propertySet,
        bool keysOnly,
        const MI_Filter* filter);

    void GetInstance(
        Context& context,
        const String& nameSpace,
        const Apache_HTTPDVirtualHostStatistics_Class& instance,
        const PropertySet& propertySet);

    void CreateInstance(
        Context& context,
        const String& nameSpace,
        const Apache_HTTPDVirtualHostStatistics_Class& newInstance);

    void ModifyInstance(
        Context& context,
        const String& nameSpace,
        const Apache_HTTPDVirtualHostStatistics_Class& modifiedInstance,
        const PropertySet& propertySet);

    void DeleteInstance(
        Context& context,
        const String& nameSpace,
        const Apache_HTTPDVirtualHostStatistics_Class& instance);

    void Invoke_ResetSelectedStats(
        Context& context,
        const String& nameSpace,
        const Apache_HTTPDVirtualHostStatistics_Class& instanceName,
        const Apache_HTTPDVirtualHostStatistics_ResetSelectedStats_Class& in);

/* @MIGEN.END@ CAUTION: PLEASE DO NOT EDIT OR DELETE THIS LINE. */
};

MI_END_NAMESPACE

#endif /* __cplusplus */

#endif /* _Apache_HTTPDVirtualHostStatistics_Class_Provider_h */

