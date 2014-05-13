/*--------------------------------------------------------------------------------
 *        Copyright (c) Microsoft Corporation. All rights reserved. See license.txt for license information.
 *     
 *        */
 /**
      \file        temppool.h

      \brief       Class to manage a temporary Apache APR memory pool

      \date        05-09-14
*/
/*----------------------------------------------------------------------------*/

#ifndef TEMPPOOL_APACHE_H
#define TEMPPOOL_APACHE_H

// Apache Portable Runtime definitions
#include <apr.h>
#include <apr_shm.h>

class TemporaryPool
{
private:
    apr_pool_t* m_pool;
    apr_pool_t* m_temppool;

public:
    TemporaryPool(apr_pool_t* pool);

    ~TemporaryPool();

    apr_pool_t* Get();

private:
    TemporaryPool();

    TemporaryPool(TemporaryPool&);
};

#endif /* TEMPPOOL_APACHE_H */
