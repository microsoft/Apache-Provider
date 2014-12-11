//-----------------------------------------------------------------------
// <copyright file="OMCleanupHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-srsant</author>
// <description></description>
// <history></history>
//-----------------------------------------------------------------------
namespace Scx.Test.Common
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.ServiceProcess;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// Helper class for cleaning up OperationsManager
    /// start/stop OM Services
    /// ClearCache
    /// Restore SQL database
    /// </summary>
     public class OMCleanupHelper
    {
        #region Private Fields
        
         /// <summary>
        /// SQL database name
        /// </summary>
        private string sqlDbName = "OperationsManager";

        /// <summary>
        /// OpsMgr Database backup file that needs to be restored
        /// </summary>
        private string sqlBkupFile = "OperationsManager.bak";

        /// <summary>
        /// SQL Backup File Path
        /// </summary>
        private string sqlPath = @"C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\";

        /// <summary>
        /// OpsMgr Cache Directory
        /// </summary>
        private string cacheDirectory = @"C:\Program Files\System Center 2012\Operations Manager\Server\Health Service State";

        /// <summary>
        /// OpsMgr Services
        /// </summary>
        private string[] services = new string[] { "healthservice", "omsdk", "cshost" };
         
        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;
     
        #endregion Private Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the OMCleanupHelper class.
        /// </summary>
        /// <param name="logger">ScxLogDelegate instance for logging purposes</param>
        public OMCleanupHelper(ScxLogDelegate logger)
        {
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Sets the log delegate method
        /// </summary>
        public ScxLogDelegate Logger
        {
            set
            {
                this.logger = value;
            }
        }

        /// <summary>
        /// Gets the OMServices list
        /// </summary>
         public string[] Services
        {
            get { return this.services; }
        }
        
         /// <summary>
         /// Gets or sets the SQL BackupFile Name
         /// </summary>
        public string SqlBkupFile
        {
            get { return this.sqlBkupFile; }
            set { this.sqlBkupFile = value; }
        }
        
         /// <summary>
         /// Gets or sets the SQL data path
         /// </summary>
        public string SqlPath
        {
            get { return this.sqlPath; }
            set { this.sqlPath = value; }
        }

            /// <summary>
         /// Gets or sets the sql database name
         /// </summary>
        public string SqlDbName
        {
            get { return this.sqlDbName; }
            set { this.sqlDbName = value; }
        }

        #endregion Properties

         #region Public Methods

       /// <summary>
        /// Starts the OpsMgr Service
       /// </summary>
       /// <param name="svcName">Service Name</param>
        public void StartService(string svcName)
        {
            if (string.IsNullOrEmpty(svcName))
            {
                throw new ArgumentNullException("Service Name is null or empty");
            }

            ServiceController sc = new ServiceController();
            sc.ServiceName = svcName;
            TimeSpan timeout = new TimeSpan(0, 0, 5, 0);
            while (sc.Status != ServiceControllerStatus.Running)
            {                             
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
                catch (System.InvalidOperationException)
                {
                    this.KillProcess(svcName);
                }
                catch (System.ServiceProcess.TimeoutException)
                {
                    this.KillProcess(svcName);
                }

                sc.Refresh();
            }

            this.logger(svcName + " service is " + sc.Status);
        }

        /// <summary>
        /// Stop the OpsMgr Service
        /// </summary>
        /// <param name="svcName">service name</param>
        public void StopService(string svcName)
        {
            if (string.IsNullOrEmpty(svcName))
            {
                throw new ArgumentNullException("Service Name is null or empty");
            }

            ServiceController sc = new ServiceController();
            sc.ServiceName = svcName;
            TimeSpan timeout = new TimeSpan(0, 0, 5, 0);
            while (sc.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                catch (System.InvalidOperationException)
                {
                    this.KillProcess(svcName);
                }
                catch (System.ServiceProcess.TimeoutException)
                {
                    this.KillProcess(svcName);
                }

                sc.Refresh();
            }

            this.logger(svcName + " service is " + sc.Status);
        }
                  
        /// <summary>
        /// Clears the Cache Directory
        /// </summary>
        public void ClearCacheDir()
        {
            this.logger("Stopping all the OMServices(Health/SDK/Config)");
            foreach (string svc in this.services)
            {
                this.StopService(svc);
            }

            this.logger(@"Deleting all the cache files from C:\Program Files\System Center 2012\Operations Manager\Server\Health Service State");
            Directory.Delete(Path.Combine(this.cacheDirectory, "Connector Configuration Cache"), true);
            Directory.Delete(Path.Combine(this.cacheDirectory, "Health Service Store"), true);
            Directory.Delete(Path.Combine(this.cacheDirectory, "Management Packs"), true);
            this.logger("Starting all the OMServices(Health/SDK/Config)");
            foreach (string svc in this.services)
            {
                this.StartService(svc);
            }
        }
        
        /// <summary>
        /// Restores the OpsMgr SQL database.Assuming OperationsManager db
        /// </summary>
        public void RestoreOMSQL()
        {
            this.logger("Stopping all the OMServices(Health/SDK/Config)");
            foreach (string svc in this.services)
            {
                this.StopService(svc);
            }
            
            string backupFullPath = Path.Combine(Path.Combine(this.sqlPath, "Backup"), this.sqlBkupFile);
            if (File.Exists(backupFullPath) == false)
            {
                throw new InvalidOperationException(this.sqlBkupFile + " file doesnot exist in the path : " + this.sqlPath);
            }

            // Connecting to SQLServer
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder["Data Source"] = "(local)";
            connectionStringBuilder["integrated Security"] = true;
            connectionStringBuilder["Initial Catalog"] = this.sqlDbName;
            string sqlConnectionString = connectionStringBuilder.ConnectionString;
            this.logger("sqlConnectionString: " + sqlConnectionString);

            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            ServerConnection serverConnection = new ServerConnection(sqlConnection);
            Server sqlServer = new Server(serverConnection);
            Database sqlDb = sqlServer.Databases[this.sqlDbName];
            if (sqlDb != null)
            {
                sqlServer.KillAllProcesses(this.sqlDbName);
            }

            Restore sqlRestore = new Restore();
            sqlRestore.Database = this.sqlDbName;
            BackupDeviceItem deviceItem = new BackupDeviceItem(backupFullPath, DeviceType.File);
            sqlRestore.Devices.Add(deviceItem);
            sqlRestore.Action = RestoreActionType.Database;
            sqlRestore.ReplaceDatabase = true;
            sqlRestore.PercentCompleteNotification = 10;
            sqlRestore.PercentComplete += new PercentCompleteEventHandler(this.SqlRestore_PercentComplete);
            sqlRestore.Complete += new ServerMessageEventHandler(this.SqlRestore_Complete);

            sqlRestore.SqlRestore(sqlServer);
            sqlDb = sqlServer.Databases[this.sqlDbName];
            sqlDb.SetOnline();
            sqlServer.Refresh();
                   
            string commandText = "ALTER DATABASE OperationsManager SET ENABLE_BROKER";
            SqlCommand command = new SqlCommand(commandText, sqlConnection);
            command.ExecuteNonQuery();
            this.logger("SQL Broker is Enabled now");
            
            this.logger("Starting all the OMServices(Health/SDK/Config)");
            foreach (string svc in this.services)
            {
                this.StartService(svc);
            }
        }
          
        /// <summary>
        /// Clears the UICache
        /// </summary>
        public void ClearCacheUI()
        {
            this.logger("Closing all the UI console");
            Process[] processList = Process.GetProcessesByName("Microsoft.EnterpriseManagement.Monitoring.Console.exe");
            foreach (Process process in processList)
            {
                process.Kill();
            }

            this.logger("Opening New Console with ClearCache option");
            Process.Start(Path.Combine(System.Environment.GetEnvironmentVariable("ProgramFiles"), "System Center 2012\\Operations Manager\\Console\\Microsoft.EnterpriseManagement.Monitoring.Console.exe"), "/ClearCache");
        }

        /// <summary>
        /// Check if the OMServices are running
        /// </summary>
        public void CheckServices()
        {
            this.logger("Checking to make sure Health/SDK/Config Services are running");
            foreach (string svc in this.services)
            {
                this.StartService(svc);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Event Handler          
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">ServerMessage EventArgs</param>
        private void SqlRestore_Complete(object sender, ServerMessageEventArgs e)
        {
            this.logger(e.ToString() + " Complete");
        }

        /// <summary>
        /// Eventhandler for notifying the restore percentcomplete
        /// </summary>
        /// <param name="sender">object Sender</param>
        /// <param name="e">PercentComplete EventArgs</param>
        private void SqlRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            this.logger(e.Percent.ToString() + "% Complete");
        }

        /// <summary>
        /// Kills the process associated with the OMServices
        /// </summary>
        /// <param name="svcName">Service Name</param>
        private void KillProcess(string svcName)
        {
            string processName = null;

            if (svcName.Equals("omsdk"))
            {
                processName = "Microsoft.Mom.Sdk.ServiceHost";
            }
            else if (svcName.Equals("cshost"))
            {
                processName = "Microsoft.Mom.ConfigServiceHost";
            }
            else if (svcName.Equals("healthservice"))
            {
                processName = "HealthService";
            }

            this.logger("Killing Process by Name: " + processName);

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals(processName))
                {
                    process.Kill();
                    this.logger("Kill Process completed");
                    break;
                }
            }
        }

        #endregion Private Methods
    }
}
