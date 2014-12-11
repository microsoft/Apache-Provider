//-----------------------------------------------------------------------
// <copyright file="SQLReadHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>7/6/2009 11:09:42 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// Helper class to retrieve information from database tables in the local MS SQL server instance using Windows authentication.
    /// </summary>
    public class SQLReadHelper
    {
        /// <summary>
        /// Connection handle to the SQL server instance and database table
        /// </summary>
        private SqlConnection sqlConnection;

        /// <summary>
        /// Log delegate method
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        /// <summary>
        /// Number of seconds to wait in attempting a connection or in attempting to run an SQL query
        /// </summary>
        private int connectionTimeout = 180;

        /// <summary>
        /// Number of attempts to make at querying the SQL database.
        /// </summary>
        private int maxSqlAttempts = 3;

        /// <summary>
        /// Initializes a new instance of the SQLReadHelper class.  Windows authentication is assumed.
        /// </summary>
        /// <param name="logger">Log delegate method.  This may be any method taking a string as argument, including ScxMethods.ScxNullLogDelegate, or Console.WriteLine</param>
        /// <param name="databaseServer">Hostname of the database server.  If empty, (local) will be used.</param>
        /// <param name="databaseName">The name of the database to connect to on the SQL Server instance, for example, OperationsManagerAC</param>
        public SQLReadHelper(ScxLogDelegate logger, string databaseServer, string databaseName)
        {
            this.logger = logger;

            string sqlConnectionString = this.CreateSQLConnectionString(databaseServer, databaseName);

            this.logger("sqlConnectionString: " + sqlConnectionString);

            this.sqlConnection = new SqlConnection(sqlConnectionString);
        }

        /// <summary>
        /// Initializes a new instance of the SQLReadHelper class.  Windows authentication with the local MS SQL database instance is assumed.
        /// </summary>
        /// <param name="logger">Log delegate method.  This may be any method taking a string as argument, including ScxMethods.ScxNullLogDelegate, or Console.WriteLine</param>
        /// <param name="databaseName">The name of the database to connect to on the SQL Server instance, for example, OperationsManagerAC</param>
        public SQLReadHelper(ScxLogDelegate logger, string databaseName)
        {
            this.logger = logger;

            string sqlConnectionString = this.CreateSQLConnectionString("(local)", databaseName);

            this.logger("sqlConnectionString: " + sqlConnectionString);

            this.sqlConnection = new SqlConnection(sqlConnectionString);
        }

        #region public methods

        /// <summary>
        /// Retrieve a System.Data.DataTable instance containing the complete table matching the given table name, including Row and Column collections.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <returns>DataTable object</returns>
        public DataTable GetTable(string tableName)
        {
            string selectString = string.Format("SELECT * FROM {0}", tableName);

            return this.GetTableWithQuery(selectString);
        }

        /// <summary>
        /// Retrieve a System.Data.DataTable instance containing the complete table matching the given table name and where clause, including Row and Column collections.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="whereClause">The contents of the WHERE clause.  This should be an SQL expression such as 'Value = True'</param>
        /// <returns>DataTable object</returns>
        public DataTable GetTable(string tableName, string whereClause)
        {
            string selectString = string.Format("SELECT * FROM {0} WHERE ({1})", tableName, whereClause);

            return this.GetTableWithQuery(selectString);
        }

        /// <summary>
        /// Retrieve a System.Data.DataTable instance containing the complete table matching the given 
        /// table name and where clause.  The results will include only those columns mentioned in fromClause.
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="whereClause">The contents of the WHERE clause.  This should be an SQL expression such as 'Value = True'</param>
        /// <param name="columnList">A list of columns to return, separated by parentheses.</param>
        /// <returns>DataTable object containing Row and Column collections</returns>
        public DataTable GetTable(string tableName, string whereClause, string columnList)
        {
            string selectString = string.Format("SELECT {0} FROM {1} WHERE ({2})", columnList, tableName, whereClause);

            return this.GetTableWithQuery(selectString);
        }

        /// <summary>
        /// Convert a System.Data.DataTable instance into a human-readable string representation of the table row and column elements.
        /// </summary>
        /// <param name="table">A table to convert</param>
        /// <returns>A human-readable string representing the table contents</returns>
        public string DataTableToString(DataTable table)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.AppendFormat("dataTable: {0} columns, {1} rows\n", table.Columns.Count, table.Rows.Count);

            foreach (DataColumn column in table.Columns)
            {
                strBuilder.AppendFormat("'{0}'  ", column.ColumnName);
            }

            strBuilder.AppendLine();
            strBuilder.AppendLine("==========================");

            foreach (DataRow row in table.Rows)
            {
                strBuilder.AppendLine(this.DataRowToString(row));
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Convert a System.Data.DataRow instance into a human-readable string representation of the row.
        /// </summary>
        /// <param name="row">A row to convert</param>
        /// <returns>A human-readable string representing the row contents</returns>
        public string DataRowToString(DataRow row)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (object o in row.ItemArray)
            {
                strBuilder.AppendFormat("{0},  ", o.ToString());
            }

            return strBuilder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a new SQL connection string.
        /// </summary>        
        /// <param name="databaseServer">Hostname of the database server.  If empty, (local) will be used.</param>
        /// <param name="databaseName">The name of the database to connect to on the SQL Server instance, for example, OperationsManagerAC</param>
        /// <returns>Connection String</returns>
        public string CreateSQLConnectionString(string databaseServer, string databaseName)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = databaseServer;
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.InitialCatalog = databaseName;
            connectionStringBuilder.ConnectTimeout = this.connectionTimeout;
            connectionStringBuilder.LoadBalanceTimeout = this.connectionTimeout;

            return connectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// Retrieve a System.Data.DataTable instance containing the complete table matching the given 
        /// SQL SELECT query.
        /// </summary>
        /// <param name="selectString">An SQL SELECT query string</param>
        /// <returns>DataTable object containing Row and Column collections</returns>
        private DataTable GetTableWithQuery(string selectString)
        {
            DataSet dataSet = new DataSet();

            DateTime queryStart = DateTime.Now;
            int queryAttempts = 0;
            bool querySuccess = false;

            SqlDataAdapter dataAdaptor = new SqlDataAdapter(selectString, this.sqlConnection);

            dataAdaptor.SelectCommand.CommandTimeout = this.connectionTimeout;

            for (queryStart = DateTime.Now; !querySuccess && queryAttempts < this.maxSqlAttempts; queryAttempts++)
            {
                try
                {
                    dataAdaptor.Fill(dataSet);
                    this.logger(string.Format("{1} consumed in running query '{0}' using dataAdaptor.Fill", selectString, DateTime.Now - queryStart));
                    querySuccess = true;
                }
                catch (Exception e)
                {
                    this.logger(string.Format("SQL query failed on attempt ({0}/{1}), using {2} : error message: {3}", queryAttempts, this.maxSqlAttempts, DateTime.Now - queryStart, e.Message));
                }
            }

            if (dataSet.Tables.Count >= 1)
            {
                return dataSet.Tables[0];
            }
            else
            {
                throw new Exception(string.Format("No tables found using query '{0}'", selectString));
            }
        }

        #endregion
    }
}