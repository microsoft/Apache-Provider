//-----------------------------------------------------------------------
// <copyright file="OverrideHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>4/3/2009 11:41:33 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;    
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;
    using Scx.Test.Common;

    /// <summary>
    /// Allows running OM SDK tasks
    /// </summary>
    public class OverrideHelper
    {
        #region Private Fields

        /// <summary>
        /// Contains information about the local OM Server installation
        /// </summary>
        private OMInfo info;

        /// <summary>
        /// Monitor helper class
        /// </summary>
        private MonitorHelper monitorHelper;

        /// <summary>
        /// Management Pack helper class instance
        /// </summary>
        private ManageMP manageMP;

        /// <summary>
        /// Name of the current management pack
        /// </summary>
        private string managementPackName;

        /// <summary>
        /// Instance of the current management pack
        /// </summary>
        private ManagementPack managementPack;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OverrideHelper class.
        /// </summary>
        /// <param name="logDlg">Log delegate method</param>
        /// <param name="info">Contains information about the local OM Server installation</param>
        /// <param name="managementPackName">Name of the management pack to use</param>
        public OverrideHelper(ScxLogDelegate logDlg, OMInfo info, string managementPackName)
        {
            this.info = info;

            this.logger = logDlg;

            this.monitorHelper = new MonitorHelper(info);

            this.manageMP = new ManageMP(logDlg, info);

            this.SetManagementPack(managementPackName);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Set the name of the management pack used in this OverrideHelper instance
        /// </summary>
        /// <param name="managementPackName">Name of the management pack to use (for example, Microsoft.AIX.5.3)</param>
        public void SetManagementPack(string managementPackName)
        {
            this.managementPackName = managementPackName;

            ManagementPackCriteria query = new ManagementPackCriteria(string.Format("Name = '{0}'", managementPackName));
            IList<ManagementPack> mps = this.info.LocalManagementGroup.ManagementPacks.GetManagementPacks(query);

            if (mps == null || mps.Count == 0)
            {
                throw new OverrideHelperException("Management pack not found: " + managementPackName);
            }

            this.managementPack = mps[0];
        }

        /// <summary>
        /// Set the threshold paramater for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitorThreshold">Value to override the monitor's threshold parameter to</param>
        public void SetClientMonitorThreshold(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, double monitorThreshold)
        {
            this.SetClientMonitorParameter(
                computerObject, 
                monitorName, 
                monitorContext, 
                monitorTarget, 
                "Threshold", 
                string.Format("{0}", monitorThreshold));
        }

        /// <summary>
        /// Set the interval paramater for the given monitor in the given context on the given client machine. This is the interval, in seconds, between polls of the monitor state.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitorInterval">Value to override the monitor's interval parameter to</param>
        public void SetClientMonitorInterval(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, int monitorInterval)
        {
            this.SetClientMonitorParameter(
                computerObject, 
                monitorName, 
                monitorContext, 
                monitorTarget,
                "Interval",
                string.Format("{0}", monitorInterval));
        }

        /// <summary>
        /// Set whether a monitor is enabled on the given host
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitorEnabled">Value to override the monitor's 'enabled' parameter to</param>
        public void SetClientMonitorEnabled(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, bool monitorEnabled)
        {
            this.SetClientMonitorProperty(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                ManagementPackMonitorProperty.Enabled,
                monitorEnabled ? "true" : "false");
        }

        /// <summary>
        /// Set the threshold paramater for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="parameterName">Name of the monitor configuration parameter, e.g., 'Threshold'</param>
        /// <param name="parameterValue">Value to override the monitor configuration parameter to.</param>
        public void SetClientMonitorParameter(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, string parameterName, string parameterValue)
        {
            ManagementPackClass monitoringClass;
            ManagementPackMonitor monitor;
            Guid contextInstance;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetOverrideInfo(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                out monitoringClass,
                out monitor,
                out contextInstance,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackMonitorConfigurationOverride)
                {
                    ManagementPackMonitorConfigurationOverride mpConfigOverride =
                        mpOverride as ManagementPackMonitorConfigurationOverride;

                    if (mpConfigOverride.ContextInstance.Equals(contextInstance) &&
                       mpConfigOverride.Monitor.Equals(monitor) &&
                       mpConfigOverride.Parameter.Equals(parameterName))
                    {
                        if (!mpConfigOverride.Value.Equals(parameterValue))
                        {
                            mpConfigOverride.Value = parameterValue;
                            mpConfigOverride.Status = ManagementPackElementStatus.PendingUpdate;

                            this.logger(string.Format("Updating override: host={0}, monitor={1}, parameter={2}, value={3}", computerObject.Name, monitorName, parameterName, mpConfigOverride.Value));
                            
                            // Save the changes into the management pack.
                            this.managementPack.AcceptChanges();
                        }

                        return;
                    }
                }
            }

            string hostNameAlpha = this.ConvertToAlphanumeric(computerObject.Name);
            Guid guid = Guid.NewGuid();
            string overrideName = string.Format("{0}.Override.{1}.{2}", monitorName, hostNameAlpha, ((ulong)guid.GetHashCode()));
            this.logger(string.Format("Creating new override: host={0}, overrideName = {1}, parameter={2}, value={3}", computerObject.Name, overrideName, parameterName, parameterValue));

            ManagementPackMonitorConfigurationOverride monitorOverride =
                new ManagementPackMonitorConfigurationOverride(this.managementPack, overrideName)
            {
                Monitor = monitor,
                Parameter = parameterName,
                Value = parameterValue,
                Context = monitoringClass,
                ContextInstance = contextInstance,
                DisplayName = overrideName
            };
            
            // Save the changes into the management pack.
            this.managementPack.AcceptChanges();
        }

        /// <summary>
        /// Set the paramater for the given collection rule in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="ruleName">Name of the collection rule to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>        
        /// <param name="parameterName">Name of the monitor configuration parameter, e.g., 'Threshold'</param>
        /// <param name="parameterValue">Value to override the monitor configuration parameter to.</param>
        public void SetClientCollectionRuleParameter(MonitoringObject computerObject, string ruleName, string monitorContext, string parameterName, string parameterValue)
        {
            ManagementPackClass monitoringClass;
            ManagementPackRule rule;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetRuleOverrideInfo(
                computerObject,
                ruleName,
                monitorContext,
                out monitoringClass,
                out rule,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackRuleConfigurationOverride)
                {
                    ManagementPackRuleConfigurationOverride mpConfigOverride =
                        mpOverride as ManagementPackRuleConfigurationOverride;

                    if (mpConfigOverride.Rule.Equals(rule) &&
                       mpConfigOverride.Parameter.Equals(parameterName))
                    {
                        if (!mpConfigOverride.Value.Equals(parameterValue))
                        {
                            mpConfigOverride.Value = parameterValue;
                            mpConfigOverride.Status = ManagementPackElementStatus.PendingUpdate;

                            this.logger(string.Format("Updating override: host={0}, monitor={1}, parameter={2}, value={3}", computerObject.Name, ruleName, parameterName, mpConfigOverride.Value));

                            // Save the changes into the management pack.
                            this.managementPack.AcceptChanges();
                        }

                        return;
                    }
                }
            }

            string hostNameAlpha = this.ConvertToAlphanumeric(computerObject.Name);
            Guid guid = Guid.NewGuid();
            string overrideName = string.Format("{0}.Override.{1}.{2}", ruleName, hostNameAlpha, ((ulong)guid.GetHashCode()));
            this.logger(string.Format("Creating new override: host={0}, overrideName = {1}, parameter={2}, value={3}", computerObject.Name, overrideName, parameterName, parameterValue));

            ManagementPackRuleConfigurationOverride ruleOverride = new ManagementPackRuleConfigurationOverride(this.managementPack, overrideName)
            {
                Rule = rule,
                Parameter = parameterName,
                Value = parameterValue,
                Context = monitoringClass,
                DisplayName = overrideName,
                Module = "DS"
            };
            
            // Save the changes into the management pack.
            this.managementPack.AcceptChanges();
        }

        /// <summary>
        /// Delete the override for the threshold paramater for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        public void DeleteClientMonitorThreshold(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget)
        {
            this.DeleteClientMonitorParameter(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                "Threshold");
        }

        /// <summary>
        /// Delete the override for the interval paramater for the given monitor in the given context on the given client machine. This is the interval, in seconds, between polls of the monitor state.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        public void DeleteClientMonitorInterval(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget)
        {
            this.DeleteClientMonitorParameter(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                "Interval");
        }

        /// <summary>
        /// Delete the override for the whether the monitor is enabled on the given host
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        public void DeleteClientMonitorEnabled(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget)
        {
            this.DeleteClientMonitorProperty(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                ManagementPackMonitorProperty.Enabled);
        }

        /// <summary>
        /// Delete an override for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="parameterName">Name of the monitor configuration parameter, e.g., 'Threshold'</param>
        public void DeleteClientMonitorParameter(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, string parameterName)
        {
            ManagementPackClass monitoringClass;
            ManagementPackMonitor monitor;
            Guid contextInstance;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetOverrideInfo(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                out monitoringClass,
                out monitor,
                out contextInstance,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackMonitorConfigurationOverride)
                {
                    ManagementPackMonitorConfigurationOverride mpConfigOverride =
                        mpOverride as ManagementPackMonitorConfigurationOverride;

                    if (mpConfigOverride.ContextInstance.Equals(contextInstance) &&
                       mpConfigOverride.Monitor.Equals(monitor) &&
                       mpConfigOverride.Parameter.Equals(parameterName))
                    {
                        mpConfigOverride.Status = ManagementPackElementStatus.PendingDelete;

                        this.logger(string.Format("Deleting override: host={0}, monitor={1}, parameter={2}", computerObject.Name, monitorName, parameterName));
                        
                        // Save the changes into the management pack.
                        this.managementPack.AcceptChanges();

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Delete an override for the given collection rule in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="ruleName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>        
        /// <param name="parameterName">Name of the monitor configuration parameter, e.g., 'Threshold'</param>
        public void DeleteClientCollectionRuleParameter(MonitoringObject computerObject, string ruleName, string monitorContext, string parameterName)
        {
            ManagementPackClass monitoringClass;
            ManagementPackRule rule;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetRuleOverrideInfo(
                computerObject,
                ruleName,
                monitorContext,
                out monitoringClass,
                out rule,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackRuleConfigurationOverride)
                {
                    ManagementPackRuleConfigurationOverride mpConfigOverride =
                        mpOverride as ManagementPackRuleConfigurationOverride;

                    if (mpConfigOverride.Rule.Equals(rule) &&
                       mpConfigOverride.Parameter.Equals(parameterName))
                    {
                        mpConfigOverride.Status = ManagementPackElementStatus.PendingDelete;

                        this.logger(string.Format("Deleting override: host={0}, rule={1}, parameter={2}", computerObject.Name, ruleName, parameterName));

                        // Save the changes into the management pack.
                        this.managementPack.AcceptChanges();

                        return;
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Retrieve information required by any override action
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitoringClass">OUT paramater: the monitoring class</param>
        /// <param name="monitor">OUT parameter: the monitor, e.g. Microsoft.Linux.RHEL.5.OperatingSystem.Process.Cron.Monitor</param>
        /// <param name="contextInstance">OUT parameter: the context instance, the identifier unique to the monitoring context (i.e., unique to a specific host)</param>
        /// <param name="overrides">OUT paramater: a collection of overrides already active against the given monitor in the given context.</param>
        private void GetOverrideInfo(
            MonitoringObject computerObject,
            string monitorName,
            string monitorContext,
            string monitorTarget,
            out ManagementPackClass monitoringClass,
            out ManagementPackMonitor monitor,
            out Guid contextInstance,
            out ManagementPackElementCollection<ManagementPackOverride> overrides)
        {
            ManagementPackClassCriteria classCriteria = new ManagementPackClassCriteria(string.Format("Name='{0}'", monitorContext));

            IList<ManagementPackClass> monitoringClasses = this.info.LocalManagementGroup.EntityTypes.GetClasses(classCriteria);

            monitoringClass = monitoringClasses[0];

            MonitorInfo monitorInfo = this.monitorHelper.GetMonitorInfo(computerObject, monitorName, monitorTarget);

            contextInstance = monitorInfo.MonitoringObject.Id;

            ManagementPackMonitorCriteria monitorCriteria = new ManagementPackMonitorCriteria(string.Format("Name='{0}'", monitorName));
            monitor = this.info.LocalManagementGroup.Monitoring.GetMonitors(monitorCriteria)[0];

            overrides = this.managementPack.GetOverrides();
        }

        /// <summary>
        /// Retrieve information required by any override action
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="ruleName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>        
        /// <param name="monitoringClass">OUT paramater: the monitoring class</param>
        /// <param name="rule">OUT parameter: the monitor, e.g. Microsoft.Linux.RHEL.5.OperatingSystem.Process.Cron.Monitor</param>
        /// <param name="overrides">OUT paramater: a collection of overrides already active against the given monitor in the given context.</param>
        private void GetRuleOverrideInfo(
            MonitoringObject computerObject,
            string ruleName,
            string monitorContext,
            out ManagementPackClass monitoringClass,
            out ManagementPackRule rule,
            out ManagementPackElementCollection<ManagementPackOverride> overrides)
        {
            ManagementPackClassCriteria classCriteria = new ManagementPackClassCriteria(string.Format("Name='{0}'", monitorContext));

            IList<ManagementPackClass> monitoringClasses = this.info.LocalManagementGroup.EntityTypes.GetClasses(classCriteria);

            monitoringClass = monitoringClasses[0];

            ManagementPackRuleCriteria ruleCriteria = new ManagementPackRuleCriteria(string.Format("Name='{0}'", ruleName));
            rule = this.info.LocalManagementGroup.Monitoring.GetRules(ruleCriteria)[0];

            overrides = this.managementPack.GetOverrides();
        }  

        /// <summary>
        /// Set a property value for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitorProperty">Enum of the monitor property, e.g., ManagementPackMonitorProperty.Enabled</param>
        /// <param name="propertyValue">Value of the property, e.g. 'false'</param>
        private void SetClientMonitorProperty(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, ManagementPackMonitorProperty monitorProperty, string propertyValue)
        {
            ManagementPackClass monitoringClass;
            ManagementPackMonitor monitor;
            Guid contextInstance;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetOverrideInfo(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                out monitoringClass,
                out monitor,
                out contextInstance,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackMonitorPropertyOverride)
                {
                    ManagementPackMonitorPropertyOverride mpPropertyOverride =
                        mpOverride as ManagementPackMonitorPropertyOverride;

                    if (mpPropertyOverride.ContextInstance.Equals(contextInstance) &&
                       mpPropertyOverride.Monitor.Equals(monitor) &&
                       mpPropertyOverride.Property == monitorProperty)
                    {
                        if (!mpPropertyOverride.Value.Equals(propertyValue))
                        {
                            mpPropertyOverride.Value = propertyValue;
                            mpPropertyOverride.Status = ManagementPackElementStatus.PendingUpdate;

                            this.logger(string.Format("Updating override: host={0}, monitor={1}, property={2}, value={3}", computerObject.Name, monitorName, monitorProperty, mpPropertyOverride.Value));
                            
                            // Save the changes into the management pack.
                            this.managementPack.AcceptChanges();
                        }

                        return;
                    }
                }
            }

            string hostNameAlpha = this.ConvertToAlphanumeric(computerObject.Name);
            Guid guid = Guid.NewGuid();
            string overrideName = string.Format("{0}.Override.{1}.{2}", monitorName, hostNameAlpha, ((ulong)guid.GetHashCode()));
            this.logger(string.Format("Creating new override: host={0}, overrideName = {1}, property={2}, value={3}", computerObject.Name, overrideName, monitorProperty, propertyValue));

            ManagementPackMonitorPropertyOverride monitorOverride =
                new ManagementPackMonitorPropertyOverride(this.managementPack, overrideName)
                {
                    Monitor = monitor,
                    Property = monitorProperty,
                    Value = propertyValue,
                    Context = monitoringClass,
                    ContextInstance = contextInstance,
                    DisplayName = overrideName
                };

            // Save the changes into the management pack.
            this.managementPack.AcceptChanges();
        }

        /// <summary>
        /// Delete a property override for the given monitor in the given context on the given client machine.
        /// </summary>
        /// <param name="computerObject">Computer object representing the machine to apply an override for.</param>
        /// <param name="monitorName">Name of the monitor to override</param>
        /// <param name="monitorContext">Name of the monitor context, e.g. Microsoft.Linux.RHEL.5.OperatingSystem</param>
        /// <param name="monitorTarget">Target of the monitor, e.g. '/tmp' in the case of a logical disk monitor</param>
        /// <param name="monitorProperty">Enum of the monitor property, e.g., ManagementPackMonitorProperty.Enabled</param>
        private void DeleteClientMonitorProperty(MonitoringObject computerObject, string monitorName, string monitorContext, string monitorTarget, ManagementPackMonitorProperty monitorProperty)
        {
            ManagementPackClass monitoringClass;
            ManagementPackMonitor monitor;
            Guid contextInstance;
            ManagementPackElementCollection<ManagementPackOverride> overrides;

            this.GetOverrideInfo(
                computerObject,
                monitorName,
                monitorContext,
                monitorTarget,
                out monitoringClass,
                out monitor,
                out contextInstance,
                out overrides);

            foreach (ManagementPackOverride mpOverride in overrides)
            {
                if (mpOverride is ManagementPackMonitorPropertyOverride)
                {
                    ManagementPackMonitorPropertyOverride mpPropertyOverride =
                        mpOverride as ManagementPackMonitorPropertyOverride;

                    if (mpPropertyOverride.ContextInstance.Equals(contextInstance) &&
                       mpPropertyOverride.Monitor.Equals(monitor) &&
                       mpPropertyOverride.Property == monitorProperty)
                    {
                        mpPropertyOverride.Status = ManagementPackElementStatus.PendingDelete;

                        this.logger(string.Format("Deleting override: host={0}, monitor={1}, property={2}", computerObject.Name, monitorName, monitorProperty));

                        // Save the changes into the management pack.
                        this.managementPack.AcceptChanges();

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Convert an input string to one consisting of a-z, A-Z, 0-9, by omitting all other characters
        /// </summary>
        /// <param name="input">A string to be converted to omit non-alphanumeric characters</param>
        /// <returns>A string converted to omit non-alphanumeric characters</returns>
        private string ConvertToAlphanumeric(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            char[] arr = input.ToCharArray();
            char[] outputArr = new char[arr.Length];

            int outputLength = 0;

            foreach (char t in arr)
            {
                if (char.IsLetterOrDigit(t))
                {
                    outputArr[outputLength++] = t;
                }
            }

            string output = new string(outputArr, 0, outputLength);

            return output;
        }

        #endregion Private Methods

        #endregion Methods
    }

    /// <summary>
    /// An exception specific to the OverrideHelper class
    /// </summary>
    public class OverrideHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the OverrideHelperException class.
        /// </summary>
        /// <param name="msg">A message describing the nature of the problem.</param>
        public OverrideHelperException(string msg)
            : base(msg)
        {
        }
    }
}
