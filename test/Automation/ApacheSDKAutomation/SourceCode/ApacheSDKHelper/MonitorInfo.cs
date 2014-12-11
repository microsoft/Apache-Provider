//-----------------------------------------------------------------------
// <copyright file="MonitorInfo.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/27/2009 1:29:09 PM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;

    /// <summary>
    /// A class to hold information about a monitor within a single instance, so that the name of the monitor,
    /// its target, and its status, are accessible.
    /// </summary>
    public class MonitorInfo
    {
        /// <summary>
        /// The MonitoringObject associated with this monitor.
        /// </summary>
        private MonitoringObject monitorObject;

        /// <summary>
        /// The MonitoringState associated with this monitor.
        /// </summary>
        private MonitoringState monitoringState;

        /// <summary>
        /// Initializes a new instance of the MonitorInfo class.
        /// </summary>
        /// <param name="mg">The management group to which this monitor is related</param>
        /// <param name="state">A monitoring state from which information about the monitor can be derived</param>
        public MonitorInfo(ManagementGroup mg, MonitoringState state)
        {
            Collection<Guid> guidCollection = new Collection<Guid> { state.ObjectId };
            IObjectReader<MonitoringObject> monitorObjects = mg.EntityObjects.GetObjectReader<MonitoringObject>(guidCollection, ObjectQueryOptions.Default);

            if (monitorObjects.Count < 1)
            {
                throw new MonitorHelperException("Could not find a monitoring object for monitoring state" + state.ToString());
            }
           
            this.monitoringState = state;
            this.monitorObject = monitorObjects.GetData(0);
        }

        /// <summary>
        /// Gets the name of the monitor, as used in the management pack, for example, Microsoft.Linux.RHEL.5.Process.Syslog.Monitor
        /// </summary>
        public string Name
        {
            get { return this.monitoringState.MonitorName; }
        }

        /// <summary>
        /// Gets the target of the monitor in question, for example, "/var", for a logical disk monitor.
        /// </summary>
        public string Target
        {
            get { return this.monitorObject.Name; }
        }

        /// <summary>
        /// Gets the operational status of the monitor.  This may contain a more specific description of its condition.
        /// </summary>
        public string OperationalStatus
        {
            get { return this.monitoringState.OperationalState; }
        }

        /// <summary>
        /// Gets the health state of the monitor
        /// </summary>
        public HealthState Health
        {
            get { return this.monitoringState.HealthState; }
        }

        /// <summary>
        /// Gets the monitoring object
        /// </summary>
        public MonitoringObject MonitoringObject
        {
            get { return this.monitorObject; }
        }

        /// <summary>
        /// Gets the monitoring state
        /// </summary>
        public MonitoringState MonitoringState
        {
            get { return this.monitoringState; }
        }

        /// <summary>
        /// Prints a short summary of the monitor and its state
        /// </summary>
        /// <returns>A summary of the monitor name, target, and health status.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Target))
            {
                return String.Format("[{0}] | {1}", this.Health.ToString(), this.Name);
            }
            else
            {
                return String.Format("[{0}] | {1} -> {2}  ", this.Health.ToString(), this.Name, this.Target);
            }
        }
    }
}