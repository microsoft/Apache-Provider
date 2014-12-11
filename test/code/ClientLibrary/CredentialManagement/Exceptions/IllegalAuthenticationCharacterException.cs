// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IllegalAuthenticationCharacterException.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    [Serializable]
    public class IllegalAuthenticationCharacterException : ArgumentException
    {
        public int Index { get; set; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Index", Index);
        }

        public IllegalAuthenticationCharacterException(string paramName, int index)
            : this(string.Format(Resources.IllegalAuthenticationCharacterException_DefaultMessage, paramName, index + 1), null, index, null)
        {
        }

        public IllegalAuthenticationCharacterException(string message, string paramName, int index)
            : this(message, paramName, index, null)
        {
        }

        public IllegalAuthenticationCharacterException(string message, int index, Exception innnerException)
            : this(message, null, index, innnerException)
        {
        }

        public IllegalAuthenticationCharacterException(string message, string paramName, int index, Exception innnerException)
            : base(message, paramName, innnerException)
        {
            Index = index;
        }

        protected IllegalAuthenticationCharacterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Index = info.GetInt32("Index");
        }
    }
}

