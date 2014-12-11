//-----------------------------------------------------------------------
// <copyright file="ManagedObjectRelationshipNotFoundException.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Exception thrown when a requested managed object relationship instance could not be found.
    /// </summary>
    [Serializable]
    public class ManagedObjectRelationshipNotFoundException : Exception
    {
        /// <summary>
        /// Constructor with the provided computer name.
        /// </summary>
        /// <param name="computerName">
        /// The computer Name.
        /// </param>
        public ManagedObjectRelationshipNotFoundException(string computerName)
        {
            ComputerName = computerName;
        }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format(Strings.ManagedObjectRelationshipNotFoundMessage, ComputerName);
            }
        }

        public string ComputerName { get; set; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ComputerName", ComputerName);
        }


        protected ManagedObjectRelationshipNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            ComputerName = info.GetString("ComputerName");
        }
    }
}