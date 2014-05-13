/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        temppool.cpp

      \brief       Class to manage a temporary Apache APR memory pool

      \date        05-09-14
*/
/*----------------------------------------------------------------------------*/

#include <apr.h>
#include <apr_shm.h>

#include "temppool.h"

TemporaryPool::TemporaryPool(apr_pool_t* pool)
    : m_pool(pool),
      m_temppool(NULL)
{
}

TemporaryPool::~TemporaryPool()
{
    if (m_temppool != NULL)
    {
        apr_pool_destroy(m_temppool);
    }
}

apr_pool_t* TemporaryPool::Get()
{
    if (m_temppool == NULL)
    {
        apr_status_t status = apr_pool_create(&m_temppool, m_pool);
        if (status != APR_SUCCESS)
        {
            return NULL;
        }
    }
    return m_temppool;
}
