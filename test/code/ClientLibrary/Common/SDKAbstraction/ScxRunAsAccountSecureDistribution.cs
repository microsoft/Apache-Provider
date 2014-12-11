// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScxRunAsAccountSecureDistribution.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Collections.Generic;

    public class ScxRunAsAccountSecureDistribution
    {
        /// <summary>
        ///     Constructs a default ScxRunAsAccountSecureDistribution, which has high
        ///     security and no approved health services.
        /// </summary>
        public ScxRunAsAccountSecureDistribution()
        {
            SecureDistribution = true;
            HealthServices = new Dictionary<Guid, string>();
        }

        /// <summary>
        ///     Constructs an ScxRunAsAccountSecureDistribution from a collection of
        ///     approved health services.
        /// </summary>
        /// <param name="secureDistribution">
        ///     The approved health services distribution.  A null distribution
        ///     indicates that all health services are approved, i.e. the "Less
        ///     Secure" option in the UI.  An empty list (i.e. <c>secureDistribution.Count() == 0</c>)
        ///     indicates that no health services are approved.  Otherwise, the
        ///     list should contain the entity ID for each health services approved
        ///     to use this Run As account.
        /// </param>
        public ScxRunAsAccountSecureDistribution(IEnumerable<Guid> secureDistribution)
            : this()
        {
            if (secureDistribution == null)
            {
                SecureDistribution = false;
            }
            else
            {
                foreach (Guid id in secureDistribution)
                {
                    HealthServices.Add(id, id.ToString());
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the distribution is using the most secure distribution.
        /// </summary>
        public bool SecureDistribution { get; set; }

        /// <summary>
        /// Gets or sets the health services associated with a RunAs account.
        /// </summary>
        public IDictionary<Guid, string> HealthServices { get; set; }
    }
}
