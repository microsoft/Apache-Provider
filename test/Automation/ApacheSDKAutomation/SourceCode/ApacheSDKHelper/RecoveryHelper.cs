//-----------------------------------------------------------------------
// <copyright file="RecoveryHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-srinia</author>
// <description></description>
// <history>3/27/2009 12:49:07 PM: Created
//          3/31/2009 2:40:00 PM: Added new function to Submit Recovery On
//          a single state change event 
// </history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Text;

    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Administration;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;

    /// <summary>
    /// Description for RunRecovery.
    /// </summary>
    public class RecoveryHelper
    {
        #region Private Fields

        /// <summary>
        /// Recovery Task to be run
        /// </summary>
        private ManagementPackRecovery recoveryOperation = null;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RecoveryHelper class
        /// </summary>
        /// <param name="recoveryName">Name of the recovery</param>
        /// <param name="mg">Management group representing the connection to ops manager</param>
        public RecoveryHelper(string recoveryName, ManagementGroup mg)
        {
            IList<ManagementPackRecovery> recoveryOperations;

            string query = string.Format("Name = '{0}'", recoveryName);
            ManagementPackRecoveryCriteria recoveryCriteria = new ManagementPackRecoveryCriteria(query);

            recoveryOperations = mg.Monitoring.GetRecoveries(recoveryCriteria);

            if (1 > recoveryOperations.Count)
            {
                throw new RecoveryHelperException("No recovery task found with " + query);
            }

            this.recoveryOperation = recoveryOperations[0];
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the accessibility value
        /// </summary>
        public ManagementPackAccessibility Accessibility
        {
            get
            {
                return this.recoveryOperation.Accessibility;
            }

            set
            {
                this.recoveryOperation.Accessibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the ExecuteOnDiagnostic value
        /// </summary>
        public ManagementPackElementReference<ManagementPackDiagnostic> ExecuteOnDiagnostic  
        {
            get
            {
                return this.recoveryOperation.ExecuteOnDiagnostic;
            }

            set
            {
                this.recoveryOperation.ExecuteOnDiagnostic = value;
            }
        }

        /// <summary>
        /// Gets or sets the ExecuteOnState value
        /// </summary>
        public HealthState? ExecuteOnState
        {
            get
            {
                return this.recoveryOperation.ExecuteOnState;
            }

            set
            {
                this.recoveryOperation.ExecuteOnState = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether HasNonCategoryOverride is set
        /// </summary>
        public bool HasNonCategoryOverride
        {
            get
            {
                return this.recoveryOperation.HasNonCategoryOverride;
            }
        }

        /// <summary>
        /// Gets the id value
        /// </summary>
        public Guid Id  
        {
            get
            {
                return (Guid)this.recoveryOperation.Id;
            }
        }

        /// <summary>
        /// Gets or sets the LanguageCode value
        /// </summary>
        public string LanguageCode 
        {
            get
            {
                return this.recoveryOperation.LanguageCode;
            }

            set
            {
                this.recoveryOperation.LanguageCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the category value
        /// </summary>
        public ManagementPackCategoryType Category
        {
            get
            {
                return this.recoveryOperation.Category;
            }

            set
            {
                this.recoveryOperation.Category = value;
            }
        }

        /// <summary>
        /// Gets or sets ConditionDetection value
        /// </summary>
        public ManagementPackConditionDetectionModule ConditionDetection
        {
            get
            {
                return this.recoveryOperation.ConditionDetection;
            }

            set
            {
                this.recoveryOperation.ConditionDetection = value;
            }
        }

        /// <summary>
        /// Gets or sets the description value
        /// </summary>
        public string Description 
        {
            get
            {
                return this.recoveryOperation.Description;
            }

            set
            {
                this.recoveryOperation.Description = value;
            }
        }

        /// <summary>
        /// Gets or sets the DisplayName value
        /// </summary>
        public string DisplayName  
        {
            get
            {
                return this.recoveryOperation.DisplayName;
            }

            set
            {
                this.recoveryOperation.DisplayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the Enabled value
        /// </summary>
        public ManagementPackMonitoringLevel Enabled
        {
            get
            {
                return this.recoveryOperation.Enabled;
            }

            set
            {
                this.recoveryOperation.Enabled = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Private Methods

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Gets monitoring recovery to be submitted on a state change evnet
        /// </summary>
        /// <returns>Monitoting recovery object</returns>
        public ManagementPackRecovery GetRecovery()
        {
            return this.recoveryOperation;
        }

        /// <summary>
        /// Submits this recovery on a single state change event
        /// </summary>
        /// <param name="monitoringClassName">Monitoring class name</param>
        /// <param name="monitoringObjectFullName">Monitoring object full name</param>
        /// <param name="monitorName">Monitor name</param>
        /// <returns>Monitoring recovery result</returns>
        public RecoveryResult SubmitRecoveryOn(
                        string monitoringClassName,
                        string monitoringObjectFullName,
                        string monitorName)
        {
            Debug.Assert(null != this.recoveryOperation, "RecoveryHelper not initialized");
            RecoveryResult result = null;
            EnterpriseManagementGroup mg = this.recoveryOperation.ManagementGroup;

            if (!mg.IsConnected)
            {
                // Try to connect
                EnterpriseManagementGroup.Connect(mg.ConnectionSettings);
            }

            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", monitoringClassName));
            IList<ManagementPackClass> monitoringClasses = mg.EntityTypes.GetClasses(classesQuery);
            if (1 > monitoringClasses.Count)
            {
                throw new RecoveryHelperException("No monitoring classes found for " + monitoringClassName);
            }

            string monitorQuery = string.Format("Name = '{0}'", monitorName);
            ManagementPackMonitorCriteria monCriteria = new ManagementPackMonitorCriteria(monitorQuery);
            IList<ManagementPackMonitor> allMonitors = mg.Monitoring.GetMonitors(monCriteria);
            if (1 > allMonitors.Count)
            {
                throw new RecoveryHelperException("No monitors found for " + monitorName);
            }

            IObjectReader<MonitoringObject> allMonitoringObjects = mg.EntityObjects.GetObjectReader<MonitoringObject>(monitoringClasses[0], ObjectQueryOptions.Default);

            if (allMonitoringObjects.Count < 1)
            {
                throw new RecoveryHelperException("No monitoring objects found for " + monitoringClasses[0].ToString());
            }

            bool monitoringStatesFound = false;
            bool stateChangeEventsFound = false;

            foreach (MonitoringObject monitoringObject in allMonitoringObjects)
            {
                if (monitoringObject.FullName.Equals(monitoringObjectFullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    IList<MonitoringState> monitoringStates = monitoringObject.GetMonitoringStates((IEnumerable<ManagementPackMonitor>)allMonitors);
                    if (0 < monitoringStates.Count)
                    {
                        monitoringStatesFound = true;

                        IList<MonitoringStateChangeEvent> stateChangeEvents = 
                            monitoringStates[0].GetStateChangeEvents();

                        if (0 < stateChangeEvents.Count)
                        {
                            stateChangeEventsFound = true;

                            result = stateChangeEvents[0].ExecuteRecovery(this.recoveryOperation, string.Empty);
                            break;
                        }
                    }
                }
            }

            if (!monitoringStatesFound)
            {
                throw new RecoveryHelperException("Recovery failed: No monitoring states found");
            }

            if (!stateChangeEventsFound)
            {
                throw new RecoveryHelperException("Recovery failed: No state change events found");
            }

            return result;
        }
                        
        #endregion Public Methods

        #endregion Methods
    }

    /// <summary>
    /// Exception class for RecoveryHelper
    /// </summary>
    public class RecoveryHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the RecoveryHelperException class
        /// </summary>
        /// <param name="message">Exception message</param>
        public RecoveryHelperException(string message)
            : base(message)
        {
        }
    }
}