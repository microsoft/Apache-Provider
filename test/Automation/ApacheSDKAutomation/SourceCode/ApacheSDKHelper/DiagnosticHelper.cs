//-----------------------------------------------------------------------
// <copyright file="DiagnosticHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-srinia</author>
// <description></description>
// <history>3/25/2009 2:37:02 PM: Created</history>
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

    using Scx.Test.Common;

    /// <summary>
    /// Halper class for managing MonitoringDiagnostic class
    /// </summary>
    public class DiagnosticHelper
    {
        #region Private Fields

        /// <summary>
        /// Information about OM under test
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Store the local management group
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        /// The diagnostic task this helper is created for
        /// </summary>
        private ManagementPackDiagnostic diagnostic;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DiagnosticHelper class
        /// </summary>
        /// <param name="info">Inofrmation about the OM server installation</param>
        /// <param name="diagnosticName">Name of the diagnostic</param>
        public DiagnosticHelper(OMInfo info, string diagnosticName)
        {
            this.info = info;
            this.managementGroup = info.LocalManagementGroup;

            string query = string.Format("Name = '{0}'", diagnosticName);
            ManagementPackDiagnosticCriteria diagnosticsCriteria = new ManagementPackDiagnosticCriteria(query);
            IList<ManagementPackDiagnostic> monitoringDiagnostics = 
                this.managementGroup.Monitoring.GetDiagnostics(diagnosticsCriteria);

            if (1 > monitoringDiagnostics.Count)
            {
                throw new DiagnosticHelperException("No diagnostic found with name " + diagnosticName);
            }

            this.diagnostic = monitoringDiagnostics[0];
        }

        #endregion Constructors

        #region Methods

        #region Public Methods
        /// <summary>
        /// Get the corresponding monitoring diagnostic
        /// </summary>
        /// <returns>the ManagementPackDiagnostic object</returns>
        public ManagementPackDiagnostic GetDiagnostic()
        {
            return this.diagnostic;
        }

        /// <summary>
        /// Executes this diagnostic on a single state change event
        /// </summary>
        /// <param name="computerObject">Object representing the monitored client</param>
        /// <param name="monitoringClassName">Monitoring class name</param>
        /// <param name="monitoringObjectFullName">Monitoring object full name</param>
        /// <param name="monitorName">Monitor name</param>
        /// <returns>Result of the diagnostic output</returns>
        public DiagnosticResult SubmitDiagnosticOn(
                        MonitoringObject computerObject,
                        string monitoringClassName,
                        string monitoringObjectFullName,
                        string monitorName)
        {
            return this.SubmitDiagnosticOn(
                computerObject,
                monitoringClassName,
                monitoringObjectFullName,
                monitorName,
                string.Empty);
        }

        /// <summary>
        /// Executes this diagnostic on a single state change event
        /// </summary>
        /// <param name="computerObject">Object representing the monitored client</param>
        /// <param name="monitoringClassName">Monitoring class name</param>
        /// <param name="monitoringObjectFullName">Monitoring object full name</param>
        /// <param name="monitorName">Monitor name</param>
        /// <param name="monitorTarget">Monitoring target, for example '/tmp'</param>
        /// <returns>Result of the diagnostic output</returns>
        public DiagnosticResult SubmitDiagnosticOn(
                        MonitoringObject computerObject,
                        string monitoringClassName,
                        string monitoringObjectFullName,
                        string monitorName,
                        string monitorTarget)
        {
            DiagnosticResult result = null;

            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", monitoringClassName));
            IList<ManagementPackClass> monitoringClasses =
                this.managementGroup.EntityTypes.GetClasses(classesQuery);

            if (1 > monitoringClasses.Count)
            {
                throw new RecoveryHelperException("No monitoring classes found for " + monitoringClassName);
            }

            MonitorHelper monitorHelper = new MonitorHelper(this.info);

            MonitorInfo[] monitorInfoArr = monitorHelper.GetMonitorInfoArray(computerObject);

            MonitoringState monitorState = null;

            foreach (MonitorInfo monitorInfo in monitorInfoArr)
            {
                if (monitorInfo.Name == monitorName &&
                    (string.IsNullOrEmpty(monitorTarget) || monitorInfo.Target == monitorTarget))
                {
                    monitorState = monitorInfo.MonitoringState;
                }
            }

            if (monitorState == null)
            {
                throw new DiagnosticHelperException("No monitoring state found for " + monitorName);
            }

            IList<MonitoringStateChangeEvent> stateChangeEvents = monitorState.GetStateChangeEvents();

            if (0 < stateChangeEvents.Count)
            {
                result = stateChangeEvents[0].ExecuteDiagnostic(this.diagnostic);
            }

            if (result == null)
            {
                throw new DiagnosticHelperException("Diagnostic result not found for " + monitorName);
            }

            return result;
        }
        
        #endregion Public Methods

        #endregion Methods
    }

    /// <summary>
    /// Exception class for DiagnosticHelper
    /// </summary>
    public class DiagnosticHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DiagnosticHelperException class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DiagnosticHelperException(string message)
            : base(message)
        {
        }
    }
}
