//-----------------------------------------------------------------------
// <copyright file="AlertsHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>viktol</author>
// <description></description>
// <history>3/31/2009 2:35:52 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Security;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// Description for AlertsHelper.
    /// </summary>
    public class AlertsHelper
    {
        #region Private Fields

        /// <summary>
        /// Information about the local OM server.
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Helper class for accessing monitors.
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Store the local management group
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        /// Closed Alert: ResolutionState = 255
        /// </summary>
        private byte closedAlertState = 255;

        /// <summary>
        /// Active Alert: ResolutionState = 0
        /// </summary>
        private byte activeAlertState = 0;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AlertsHelper class.
        /// </summary>
        /// <param name="info">OMInfo instance containing the imformation needed to connect to the OM SDK service.</param>
        public AlertsHelper(OMInfo info)
        {
            this.info = info;
            this.managementGroup = info.LocalManagementGroup;
            this.monitorHelper = new MonitorHelper(this.info);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns an instance of a computer object matching the given computer display name.
        /// </summary>
        /// <param name="displayName">A computer display name, for example, scxom-redhat15.scx.com</param>
        /// <returns>A MonitoringObject instance representing the given computer.</returns>
        public MonitoringObject GetComputerObject(string displayName)
        {
            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", "Microsoft.Unix.Computer"));
            IList<ManagementPackClass> computerClasses = this.managementGroup.EntityTypes.GetClasses(classesQuery);

            IObjectReader<MonitoringObject> computerObjects =
                this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(computerClasses[0], ObjectQueryOptions.Default);

            foreach (MonitoringObject computerObject in computerObjects)
            {
                if (string.Compare(displayName, computerObject.DisplayName, true) == 0)
                {
                    return computerObject;
                }
            }

            throw new AlertHelperException("Failed to find computer object with DisplayName: " + displayName);
        }

        /// <summary>
        /// Checks if the alert is active for a given computer and alert name.
        /// </summary>
        /// <param name="alert">The alert to be checked</param>
        /// <returns>True if the alert is in an active state</returns>
        public bool IsActive(MonitoringAlert alert)
        {
            if (alert == null)
            {
                return false;
            }

            if (alert.ResolutionState == this.activeAlertState)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the alert is closed for a given computer and alert name.
        /// </summary>
        /// <param name="alert">The alert to be closed</param>
        /// <returns>True if the alert is in a closed state</returns>
        public bool IsClosed(MonitoringAlert alert)
        {
            if (alert == null)
            {
                return false;
            }

            if (alert.ResolutionState == this.closedAlertState)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the Monitor-generated alert on the machine.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.4.Process.Cron.Monitor' </param>
        /// <returns>The alert generated by the given monitor</returns>
        public MonitoringAlert GetMonitorAlert(MonitoringObject computerObject, string monitorName)
        {
            return this.GetMonitorAlert(computerObject, monitorName, string.Empty);
        }

        /// <summary>
        /// Get the Monitor-generated alert on the machine.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.4.LogicalDisk.DiskHealth.Monitor' </param>
        /// <param name="monitorTarget">Target of the monitor, for example, '/boot'</param>
        /// <returns>The alert generated by the given monitor</returns>
        public MonitoringAlert GetMonitorAlert(MonitoringObject computerObject, string monitorName, string monitorTarget)
        {
            MonitoringAlert malert = null;

            List<MonitoringAlert> alerts = null;

            alerts = this.GetMonitorAlerts(computerObject, monitorName, monitorTarget);

            // Return null if we don't have any matching alerts
            if (alerts.Count == 0)
            {
                return null;
            }

            // Look throguh this list and return the latest alert
            malert = alerts[0];
            foreach (MonitoringAlert foundAlert in alerts)
            {
                if (malert.LastModified < foundAlert.LastModified)
                {
                    malert = foundAlert;
                }
            }

            return malert;
        }

        /// <summary>
        /// Get the Monitor-generated alert on the machine by specified alert name.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.4.LogicalDisk.DiskHealth.Monitor' </param>
        /// <param name="monitorTarget">Target of the monitor, for example, '/boot'</param>
        /// <param name="alertName">Name of the alter</param>
        /// <returns>The alert generated by the given monitor</returns>
        public MonitoringAlert GetMonitorAlert(MonitoringObject computerObject, string monitorName, string monitorTarget, string alertName)
        {
            List<MonitoringAlert> alerts = this.GetMonitorAlerts(computerObject, monitorName, monitorTarget);

            foreach (var alert in alerts)
            {
                if (string.Equals(alert.Name, alertName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return alert;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the rule-generated alert on the machine.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="ruleName">The name of the rule, for example, 
        /// 'Microsoft.Linux.RHEL.4.LogFile.Syslog.Auth.Critical.Alert' </param>
        /// <returns>The alert generated by the given rule</returns>
        public MonitoringAlert GetRuleAlert(MonitoringObject computerObject, string ruleName)
        {
            MonitoringAlert malert = null;

            List<MonitoringAlert> alerts = this.GetRuleAlerts(computerObject, ruleName);

            // Return null if we don't have any matching alerts
            if (alerts.Count == 0)
            {
                return null;
            }

            // Look throguh this list and return the latest alert
            malert = alerts[0];
            foreach (MonitoringAlert foundAlert in alerts)
            {
                if (malert.LastModified < foundAlert.LastModified)
                {
                    malert = foundAlert;
                }
            }

            return malert;
        }

        /// <summary>
        /// Get all monitor-generated alerts on the machine matching the given monitor.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.4.Process.Cron.Monitor' </param>
        /// <returns>A list of MonitoringAlert</returns>
        public List<MonitoringAlert> GetMonitorAlerts(MonitoringObject computerObject, string monitorName)
        {
            return this.GetMonitorAlerts(computerObject, monitorName, string.Empty);
        }

        /// <summary>
        /// Get all monitor-generated alerts on the machine matching the given monitor.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.4.Process.Cron.Monitor' </param>
        /// <param name="monitorTarget">The target of the monitor, for example '/boot'</param>
        /// <returns>A list of MonitoringAlert</returns>
        public List<MonitoringAlert> GetMonitorAlerts(MonitoringObject computerObject, string monitorName, string monitorTarget)
        {
            List<MonitoringAlert> currentAlerts = new List<MonitoringAlert>();

            MonitorInfo monitor = null;

            if (string.IsNullOrEmpty(monitorTarget))
            {
                monitor = this.monitorHelper.GetMonitorInfo(computerObject, monitorName);
            }
            else
            {
                monitor = this.monitorHelper.GetMonitorInfo(computerObject, monitorName, monitorTarget);
            }

            if (monitor == null || monitor.MonitoringObject == null)
            {
                return currentAlerts;
            }

            ReadOnlyCollection<MonitoringAlert> alerts = monitor.MonitoringObject.GetMonitoringAlerts();

            // Look throguh this list and select out the ones with matching name
            foreach (MonitoringAlert malert in alerts)
            {
                if (malert.ResolutionState != this.closedAlertState)
                {
                    currentAlerts.Add(malert);
                }
            }

            return currentAlerts;
        }

        /// <summary>
        /// Get all rule-generated alerts on the machine matching the given rule.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="ruleName">The name of the rule, for example, 
        /// 'Microsoft.Linux.RHEL.4.LogFile.Syslog.Auth.Critical.Alert' </param>
        /// <returns>A list of MonitoringAlert</returns>
        public List<MonitoringAlert> GetRuleAlerts(MonitoringObject computerObject, string ruleName)
        {
            List<MonitoringAlert> currentAlerts = new List<MonitoringAlert>();

            // Get all alerts on the specified machine            
            ReadOnlyCollection<MonitoringAlert> alerts = computerObject.GetMonitoringAlerts(
                Microsoft.EnterpriseManagement.Common.TraversalDepth.Recursive);

            IList<ManagementPackRule> rules = computerObject.ManagementGroup.Monitoring.GetRules();

            // Look throguh this list and select those open alerts which are generated by the given rule name.
            foreach (MonitoringAlert malert in alerts)
            {
                if (malert.ResolutionState != this.closedAlertState)
                {
                    foreach (ManagementPackRule rule in rules)
                    {
                        if (rule.Id == malert.RuleId &&
                            ruleName == rule.Name)
                        {
                            currentAlerts.Add(malert);
                        }
                    }
                }
            }

            return currentAlerts;
        }

        /// <summary>
        /// Close the given alert
        /// </summary>
        /// <param name="alert">The alert to be closed</param>
        public void CloseAlert(MonitoringAlert alert)
        {
            if (alert.ResolutionState != this.closedAlertState)
            {
                alert.ResolutionState = this.closedAlertState;
                alert.Update(string.Format("AlertsHelper.CloseAlert({0})", alert.Name));
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Retrieves the monitoring class matching a given class name.
        /// </summary>
        /// <param name="className">The name of the class to match, for example, Microsoft.unix.computer</param>
        /// <returns>The MonitoringClass instance</returns>
        private ManagementPackClass GetMonitoringClass(string className)
        {
            ManagementPackClassCriteria classesQuery = new ManagementPackClassCriteria(string.Format("Name = '{0}'", className));
            IList<ManagementPackClass> monitoringClasses =
                this.managementGroup.EntityTypes.GetClasses(classesQuery);

            if (monitoringClasses.Count == 0)
            {
                throw new AlertHelperException("Failed to find monitoring class " + className);
            }

            return monitoringClasses[0];
        }

        #endregion Private Methods

        #endregion Methods
    }

    /// <summary>
    /// An exception specific to the ManageMonitors class
    /// </summary>
    public class AlertHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the AlertHelperException class.
        /// </summary>
        /// <param name="msg">A message describing the nature of the problem.</param>
        public AlertHelperException(string msg)
            : base(msg)
        {
        }
    }
}
