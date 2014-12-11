// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScxRunAsAccount.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;

    public class ScxRunAsAccount : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScxRunAsAccount"/> class.
        /// </summary>
        public ScxRunAsAccount()
        {
            this.Credential = new CredentialSet();
            this.DisplayName = string.Empty;
            this.Description = string.Empty;
        }

        /// <summary>
        /// Gets or set the credential set.
        /// </summary>
        public CredentialSet Credential { get; set; }

        /// <summary>
        /// Gets or set the RunAs account name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or set the RunAs account description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or set the last time the RunAs account is modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or set the RunAs account id.
        /// </summary>
        public Guid RunAsAccountId { get; set; }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            var copy = new ScxRunAsAccount
                {
                    Credential = (CredentialSet)this.Credential.Clone(),
                    DisplayName = String.Copy(this.DisplayName),
                    Description = String.Copy(this.Description),
                    LastModified = this.LastModified,
                    RunAsAccountId = this.RunAsAccountId
                };

            return copy;
        }
    }
}
