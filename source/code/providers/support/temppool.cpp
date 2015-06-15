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
