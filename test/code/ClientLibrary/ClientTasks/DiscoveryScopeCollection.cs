// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryScopeCollection.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>sknapp</author>
// <summary>
//   Represents a dynamic data collection that provides notifications when DiscoveryScopes get added, removed, or when the whole list is refreshed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary>
    /// Represents a dynamic data collection that provides notifications when DiscoveryScopes get added, removed, or when the whole list is refreshed.
    /// </summary>
    [CLSCompliant(false)]
    public class DiscoveryScopeCollection : ObservableCollection<IDiscoveryScope>, ICloneable
    {
        #region Methods

        /// <summary>
        /// Adds a <see cref="DiscoveryScopeBuilder"/> to the end of the collection. 
        /// </summary>
        /// <param name="item">
        /// The DiscoveryScope item.
        /// </param>
        /// <returns>
        /// true if the element is added to the collection object; false if the element is already present. 
        /// </returns>
        public new bool Add(IDiscoveryScope item)
        {
            if (Contains(item))
            {
                return false;
            }

            base.Add(item);

            return true;
        }

        #endregion Methods

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            DiscoveryScopeCollection copy = new DiscoveryScopeCollection();

            foreach (IDiscoveryScope discoveryScope in this)
            {
                copy.Add((IDiscoveryScope)discoveryScope.Clone());
            }

            return copy;
        }

        #endregion
    }
}
