//-----------------------------------------------------------------------
// <copyright file="ManagedObjectNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Exception thrown when a requested managed object instance could not be found.
    /// </summary>
    [Serializable]
    public class ManagedObjectNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManagedObjectNotFoundException class.
        /// </summary>
        /// <param name="displayName">Display name of requested object.</param>
        public ManagedObjectNotFoundException(string displayName)
        {
            this.DisplayName = displayName;
        }

        protected ManagedObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.DisplayName = info.GetString("DisplayName");
        }

        /// <summary>
        /// Gets the display name of the requested object.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, Strings.ManagedObjectNotFoundMessage, this.DisplayName);
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DisplayName", this.DisplayName);
        }
    }
}
