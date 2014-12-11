//-----------------------------------------------------------------------
// <copyright file="MonitorHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/26/2009 9:27:35 AM: Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Common;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Monitoring;

    /// <summary>
    /// Description for ManageMonitors.
    /// </summary>
    public class MonitorHelper
    {
        #region Private Fields

        /// <summary>
        /// Store the local management group
        /// </summary>
        private ManagementGroup managementGroup;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MonitorHelper class.
        /// </summary>
        /// <param name="info">OMInfo instance containing the imformation needed to connect to the OM SDK service.</param>
        public MonitorHelper(OMInfo info)
        {
            this.managementGroup = info.LocalManagementGroup;
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
            ManagementPackClass computerClass = this.GetMonitoringClass("Microsoft.Unix.Computer");

            IObjectReader<MonitoringObject> computerObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(computerClass, ObjectQueryOptions.Default);

            foreach (MonitoringObject computerObject in computerObjects)
            {
                if (string.Compare(displayName, computerObject.DisplayName, true) == 0)
                {
                    return computerObject;
                }
            }

            throw new MonitorHelperException("Failed to find computer object with DisplayName: " + displayName);
        }

        /// <summary>
        /// Returns an instance of a monitoring object by specified Monitoring Class and Name.
        /// </summary>
        /// <param name="monitoringClass">A monitor class name</param>
        /// <param name="name">A monitoring object name, e.g. :Microsoft.JEE.WebSphere.Windows.Profile</param>
        /// <returns>A MonitoringObject instance: e.g. AppSrv01.SCXOMT-WS7-08Node01Cell.SCXOMT-WS7-08Node01</returns>
        public MonitoringObject GetMonitoringObject(string monitoringClass, string name)
        {
            ManagementPackClass managementPackClass = this.GetMonitoringClass(monitoringClass);

            IObjectReader<MonitoringObject> monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(managementPackClass, ObjectQueryOptions.Default);
            foreach (MonitoringObject monitoringObject in monitoringObjects)
            {
                if (string.Compare(name, monitoringObject.Name, true) == 0 || string.Compare(name, monitoringObject.Path, true) == 0)
                {
                    return monitoringObject;
                }
            }

            throw new MonitorHelperException(string.Format("Failed to find monitoring object with Class '{0}', Name:'{1}' ", monitoringClass, name));
        }

        /// <summary>
        /// Returns an instance of a monitoring object by specified Monitoring Class, DisplayName and Path.
        /// </summary>
        /// <param name="monitoringClass">A monitor class name, e.g. :Microsoft.JEE.Tomcat.Managed.Windows.Configuration</param>
        /// <param name="displayName">A monitoring object displayName, e.g. :Managed-Tomcat|C:\apache-tomcat-6.0.29</param>
        /// <param name="path">A monitoring object path, e.g. :SCXOMT-WS7-25.SCX.com;c:\apache-tomcat-6.0.29</param>
        /// <returns>Return the monitoring object</returns>
        public MonitoringObject GetMonitoringObject(string monitoringClass, string displayName, string path)
        {
            ManagementPackClass managementPackClass = this.GetMonitoringClass(monitoringClass);

            IObjectReader<MonitoringObject> monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(managementPackClass, ObjectQueryOptions.Default);
            foreach (MonitoringObject monitoringObject in monitoringObjects)
            {
                if (string.Compare(displayName, monitoringObject.DisplayName, true) == 0 && string.Compare(path, monitoringObject.Path, true) == 0)
                {
                    return monitoringObject;
                }
            }

            throw new MonitorHelperException(string.Format("Failed to find monitoring object with Class '{0}', DisplayName: '{1}', Path: '{2}' ", monitoringClass, displayName, path));
        }

        /// <summary>
        /// Returns an instance of a monitoring object by specified Monitoring Class and Name.
        /// </summary>
        /// <param name="monitoringClass">A monitor class name</param>
        /// <param name="displayName">A monitoring object displayName, e.g. :Microsoft.JEE.WebSphere.Windows.Profile</param>
        /// <returns>A MonitoringObject instance: e.g. AppSrv01.SCXOMT-WS7-08Node01Cell.SCXOMT-WS7-08Node01</returns>
        public MonitoringObject GetMonitoringObjectByDisplayName(string monitoringClass, string displayName)
        {
            ManagementPackClass managementPackClass = this.GetMonitoringClass(monitoringClass);

            IObjectReader<MonitoringObject> monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(managementPackClass, ObjectQueryOptions.Default);
            foreach (MonitoringObject monitoringObject in monitoringObjects)
            {
                if (string.Compare(displayName, monitoringObject.DisplayName, true) == 0)
                {
                    return monitoringObject;
                }
            }

            throw new MonitorHelperException(string.Format("Failed to find monitoring object with Class '{0}', DisplayName:'{1}' ", monitoringClass, displayName));
        }

        /// <summary>
        /// Returns an instance of a monitoring object by specified Monitoring Class and Name.
        /// </summary>
        /// <param name="monitoringClass">A monitor class name</param>
        /// <param name="displayName">A monitoring object displayName, e.g. :Microsoft.JEE.WebSphere.Windows.Profile</param>
        /// <param name="hostName">A monitoring object hostName, e.g. :scxjet-ws7-26</param>
        /// <returns>A MonitoringObject instance: e.g. AppSrv01.SCXOMT-WS7-08Node01Cell.SCXOMT-WS7-08Node01</returns>
        public MonitoringObject GetMonitoringObjectByDisplayName(string monitoringClass, string displayName, string hostName)
        {
            ManagementPackClass managementPackClass = this.GetMonitoringClass(monitoringClass);

            IObjectReader<MonitoringObject> monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(managementPackClass, ObjectQueryOptions.Default);
            foreach (MonitoringObject monitoringObject in monitoringObjects)
            {
                if (string.Equals(displayName, monitoringObject.DisplayName, StringComparison.CurrentCultureIgnoreCase) && monitoringObject.FullName.ToLower().Contains(hostName.ToLower()))
                {
                    return monitoringObject;
                }
            }

            throw new MonitorHelperException(string.Format("Failed to find monitoring object with Class '{0}', DisplayName: '{1}', HostName: '{2}' ", monitoringClass, displayName, hostName));
        }

        /// <summary>
        /// Returns monitoring object list by specified Monitoring Class. 
        /// </summary>
        /// <param name="monitoringClass">A monitor class name</param>
        /// <returns>Returns monitoring object list</returns>
        public IObjectReader<MonitoringObject> GetMonitoringObjectList(string monitoringClass)
        {
            ManagementPackClass managementPackClass = this.GetMonitoringClass(monitoringClass);

            IObjectReader<MonitoringObject> monitoringObjects = this.managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(managementPackClass, ObjectQueryOptions.Default);

            return monitoringObjects;
        }

        /// <summary>
        /// Retrieves an array of MonitorInfo objects which provide a succinct description of the names, targets, and status of
        /// all monitors which exist on the given monitored computer.
        /// </summary>
        /// <param name="computerObject">A computer object from which to retrieve monitors</param>
        /// <returns>An array of MonitorInfo instances</returns>
        public MonitorInfo[] GetMonitorInfoArray(MonitoringObject computerObject)
        {
            HierarchyNode<MonitoringState> monitorTree = computerObject.GetStateHierarchy();
            List<MonitoringState> monitorStates = this.GetAllMonitoringStates(monitorTree);
            MonitorInfo[] results = new MonitorInfo[monitorStates.Count];
            int i = 0;

            foreach (MonitoringState monitorState in monitorStates)
            {
                results[i++] = new MonitorInfo(this.managementGroup, monitorState);
            }

            return results;
        }

        /// <summary>
        /// Gets the monitor info for a given computer and monitor name.  If more than one 
        /// monitor exists which match the monitor name, the first one will found will be used.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.5.Process.Syslog.Monitor' </param>
        /// <returns>The health state of the monitor.</returns>
        public MonitorInfo GetMonitorInfo(MonitoringObject computerObject, string monitorName)
        {
            MonitorInfo[] monitorsInfo = this.GetMonitorInfoArray(computerObject);

            foreach (MonitorInfo monitorInfo in monitorsInfo)
            {
                if (monitorInfo.Name.Equals(monitorName))
                {
                    return monitorInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the monitor info for a given computer, monitor name, and target
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.5.LogicalDisk.DiskHealth.Monitor' </param>
        /// <param name="target">The target of the monitor, for example, '/var'</param>
        /// <returns>The health state of the monitor.</returns>
        public MonitorInfo GetMonitorInfo(MonitoringObject computerObject, string monitorName, string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return this.GetMonitorInfo(computerObject, monitorName);
            }

            MonitorInfo[] monitorsInfo = this.GetMonitorInfoArray(computerObject);

            foreach (MonitorInfo monitorInfo in monitorsInfo)
            {
                if (monitorInfo.Name.Equals(monitorName) &&
                    monitorInfo.Target.Equals(target))
                {
                    return monitorInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the monitor health state for a given computer and monitor name.  If more than one 
        /// monitor exists which match the monitor name, the first one will found will be used.
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.5.Process.Syslog.Monitor' </param>
        /// <returns>The health state of the monitor.</returns>
        public HealthState GetMonitorHealthState(MonitoringObject computerObject, string monitorName)
        {
            MonitorInfo[] monitorsInfo = this.GetMonitorInfoArray(computerObject);

            foreach (MonitorInfo monitorInfo in monitorsInfo)
            {
                if (monitorInfo.Name.Equals(monitorName))
                {
                    return monitorInfo.Health;
                }
            }

            /* there is no way to distinguish between an uninitialized monitor in an external rollup,
             * and a nonexistant monitor. */
            return HealthState.Uninitialized;
        }

        /// <summary>
        /// Gets the monitor health state for a given computer, monitor name, and target
        /// </summary>
        /// <param name="computerObject">A computer object</param>
        /// <param name="monitorName">The name of the monitor, for example, 
        /// 'Microsoft.Linux.RHEL.5.LogicalDisk.DiskHealth.Monitor' </param>
        /// <param name="target">The target of the monitor, for example, '/var'</param>
        /// <returns>The health state of the monitor.</returns>
        public HealthState GetMonitorHealthState(MonitoringObject computerObject, string monitorName, string target)
        {
            MonitorInfo[] monitorsInfo = this.GetMonitorInfoArray(computerObject);

            foreach (MonitorInfo monitorInfo in monitorsInfo)
            {
                if (monitorInfo.Name.Equals(monitorName) &&
                    monitorInfo.Target.Equals(target))
                {
                    return monitorInfo.Health;
                }
            }

            /* there is no way to distinguish between an uninitialized monitor in an external rollup,
             * and a nonexistant monitor. */
            return HealthState.Uninitialized;
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
                throw new MonitorHelperException("Failed to find monitoring class " + className);
            }

            return monitoringClasses[0];
        }

        /// <summary>
        /// Gets a list of all monitoring states within the given monitoring heirarchy, whether they
        /// are located within the normal heirarchy or within an embedded external rollup.
        /// </summary>
        /// <param name="tree">A monitoring heirarchy</param>
        /// <returns>The monitoring heirarchy flattened into a single list of MonitoringState instances.  
        /// This list is similar to the list shown graphically in the health explorer in the OM Console.</returns>
        private List<MonitoringState> GetAllMonitoringStates(HierarchyNode<MonitoringState> tree)
        {
            List<MonitoringState> recursiveChildren = new List<MonitoringState> { tree.Item };

            foreach (HierarchyNode<MonitoringState> child in tree.ChildNodes)
            {
                recursiveChildren.AddRange(this.GetAllMonitoringStates(child));
            }

            if (tree.Item is ExternalRollupMonitoringState)
            {
                ExternalRollupMonitoringState ext = (ExternalRollupMonitoringState)tree.Item;

                IList<HierarchyNode<MonitoringState>> hierarchies =
                    ext.GetExternalStateHierarchies();

                foreach (HierarchyNode<MonitoringState> hierarchy in hierarchies)
                {
                    foreach (HierarchyNode<MonitoringState> extendedChild in hierarchy.ChildNodes)
                    {
                        recursiveChildren.AddRange(this.GetAllMonitoringStates(extendedChild));
                    }
                }
            }

            return recursiveChildren;
        }

        #endregion Private Methods

        #endregion Methods
    }

    /// <summary>
    /// An exception specific to the ManageMonitors class
    /// </summary>
    public class MonitorHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MonitorHelperException class.
        /// </summary>
        /// <param name="msg">A message describing the nature of the problem.</param>
        public MonitorHelperException(string msg)
            : base(msg)
        {
        }
    }
}