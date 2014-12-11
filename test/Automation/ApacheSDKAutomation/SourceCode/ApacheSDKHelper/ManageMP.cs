//-----------------------------------------------------------------------
// <copyright file="ManageMP.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <author>a-brdust</author>
// <description></description>
// <history>3/24/2009 Created</history>
//-----------------------------------------------------------------------

namespace Scx.Test.Apache.SDK.ApacheSDKHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Microsoft.EnterpriseManagement;
    using Microsoft.EnterpriseManagement.Configuration;
    using Microsoft.EnterpriseManagement.Configuration.IO;
    using Microsoft.EnterpriseManagement.Packaging;
    using Scx.Test.Common;

    /// <summary>
    /// Class to manage Operations Manager Management Packs
    /// </summary>
    public class ManageMP
    {
        #region Private Fields

        /// <summary>
        /// Default management pack directory
        /// </summary>
        private string managementPackDirectory = @"C:\BVT\MPs\";

        /// <summary>
        /// Management pack file storage
        /// </summary>
        private ManagementPackFileStore managementPackFileStore = null;

        /// <summary>
        /// The local management group
        /// </summary>
        private ManagementGroup managementGroup;

        /// <summary>
        /// Logger function
        /// </summary>
        private ScxLogDelegate logger = ScxMethods.ScxNullLogDelegate;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ManageMP class.
        /// </summary>
        /// <param name="logDlg">Log delegate method</param>
        /// <param name="info">Information needed to connect to the Operations Manager installation.</param>
        public ManageMP(ScxLogDelegate logDlg, OMInfo info)
        {
            this.managementPackFileStore = new ManagementPackFileStore();
            this.managementPackFileStore.AddDirectory(this.managementPackDirectory);
            
            this.managementGroup = info.LocalManagementGroup;

            this.logger = logDlg;
        }

        /// <summary>
        /// Initializes a new instance of the ManageMP class.
        /// </summary>
        /// <param name="info">Information needed to connect to the Operations Manager installation.</param>
        public ManageMP(OMInfo info)
        {
            this.managementPackFileStore = new ManagementPackFileStore();
            this.managementPackFileStore.AddDirectory(this.managementPackDirectory);

            this.managementGroup = info.LocalManagementGroup;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the default management pack directory.
        /// </summary>
        public string ManagementPackDirectory
        {
            get 
            { 
                return this.managementPackDirectory; 
            }

            set
            {
                this.managementPackFileStore.RemoveDirectory(this.managementPackDirectory);
                this.managementPackDirectory = value;
                this.managementPackFileStore.AddDirectory(this.managementPackDirectory);
            }
        }

        /// <summary>
        /// Gets a collection of the imported management packs
        /// </summary>
        public IList<ManagementPack> ManagementPacks
        {
            get { return this.managementGroup.ManagementPacks.GetManagementPacks(); }
        }

        /// <summary>
        /// Gets an array of the names of imported management packs
        /// </summary>
        public string[] ManagementPackNames
        {
            get
            {
                int i = 0;
                IList<ManagementPack> managementPacks = this.managementGroup.ManagementPacks.GetManagementPacks();
                string[] results = new string[managementPacks.Count];
                foreach (ManagementPack mp in managementPacks)
                {
                    results[i++] = mp.Name;
                }

                return results;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sort ManagementPack array in an order whereby the first element should will have no dependencies,
        /// and later elements may only depend upon earlier elements.  Microsoft.Linux.Library.mp for example will
        /// always be located earlier in the array than Microsoft.Linux.RedHat.Library.mp
        /// </summary>
        /// <param name="mpsIn">Array of management packs to sort</param>
        /// <returns>Array of management packs sorted by dependency</returns>
        public static ManagementPack[] SortMpsInDependencyOrder(ManagementPack[] mpsIn)
        {
            Hashtable hashedMPs = new Hashtable();
            ManagementPack[] mpsOut = (ManagementPack[])mpsIn.Clone();
            int i = 0;
            for (i = 0; i < mpsOut.Length; i++)
            {
                hashedMPs.Add(mpsOut[i].Name, i);
            }

            for (i = 0; i < mpsOut.Length; i++)
            {
                bool foundDep = false;

                do
                {
                    foundDep = false;

                    ManagementPackReferenceCollection dependencies = mpsOut[i].References;

                    foreach (ManagementPackReference depMP in dependencies.Values)
                    {
                        string mpName = mpsOut[i].Name;
                        int mpIdx = i;
                        string depName = depMP.Name;

                        if (hashedMPs.ContainsKey(depName) && (int)hashedMPs[depName] > mpIdx)
                        {
                            int depIdx = (int)hashedMPs[depName];

                            ManagementPack tempMP = mpsOut[mpIdx];
                            mpsOut[mpIdx] = mpsOut[depIdx];
                            mpsOut[depIdx] = tempMP;

                            hashedMPs[mpName] = depIdx;
                            hashedMPs[depName] = mpIdx;

                            foundDep = true;
                            break;
                        }
                    }
                }
                while (foundDep == true);
            }

            return mpsOut;
        }

        /// <summary>
        /// Imports all the management packs (.mp) in the management pack directory
        /// </summary>
        public void ImportManagementPacks()
        {
            this.ImportManagementPacks("*.mp");
        }

        /// <summary>
        /// Imports all the management packs in the management pack directory matching a given pattern
        /// </summary>
        /// <param name="filePattern">File patter to use in importing management packs, for example, "*.mp"</param>
        public void ImportManagementPacks(string filePattern)
        {
            DirectoryInfo dir = new DirectoryInfo(this.managementPackDirectory);
            FileInfo[] files = dir.GetFiles(filePattern);
            ManagementPack[] mps = new ManagementPack[files.Length];
            int i = 0;

            foreach (FileInfo file in files)
            {
                mps[i++] = new ManagementPack(file.FullName, this.managementPackFileStore);
            }

            mps = SortMpsInDependencyOrder(mps);

            for (i = 0; i < mps.Length; i++)
            {
                this.ImportManagementPack(mps[i]);
            }
        }

        /// <summary>
        /// Import a specific management pack
        /// </summary>
        /// <param name="fileName">Name of management pack file (.mp or .xml)</param>
        public void ImportManagementPack(string fileName)
        {
            string filePath = Path.Combine(this.managementPackDirectory, fileName);

            ManagementPack mp = new ManagementPack(filePath, this.managementPackFileStore);

            this.ImportManagementPack(mp);    
        }

        /// <summary>
        /// Import a specific management pack
        /// </summary>
        /// <param name="mp">ManagementPack object</param>
        public void ImportManagementPack(ManagementPack mp)
        {
            bool managementPackImported = true;

            try
            {
                ManagementPack tempMP = this.managementGroup.ManagementPacks.GetManagementPack(mp.Id);
            }
            catch (Exception)
            {
                managementPackImported = false;
            }

            if (!managementPackImported)
            {
                this.logger("Importing MP:  " + mp.Name);
                this.managementGroup.ManagementPacks.ImportManagementPack(mp);
            }
        }

        /// <summary>
        /// Uninstall all management packs matching the given substring, e.g., 'Microsoft.Linux'
        /// </summary>
        /// <param name="managementPackNameSubstring">Uninstall management packs whose names include the given string</param>
        public void UninstallManagementPacks(string managementPackNameSubstring)
        {
            this.logger(string.Format("UninstallManagementPacks({0})", managementPackNameSubstring));

            IList<ManagementPack> managementPacks = this.managementGroup.ManagementPacks.GetManagementPacks();
            int mpIndex = 0;

            foreach (ManagementPack managementPack in managementPacks)
            {
                if (managementPack.Name.Contains(managementPackNameSubstring))
                {
                    mpIndex++;
                }
            }

            ManagementPack[] matchingMPs = new ManagementPack[mpIndex];
            mpIndex = 0;

            foreach (ManagementPack managementPack in managementPacks)
            {
                if (managementPack.Name.Contains(managementPackNameSubstring))
                {
                    matchingMPs[mpIndex++] = managementPack;
                }
            }

            matchingMPs = SortMpsInDependencyOrder(matchingMPs);

            for (mpIndex = matchingMPs.Length - 1; mpIndex >= 0; mpIndex--)
            {
                this.UninstallManagementPack(matchingMPs[mpIndex]);
            }
        }

        /// <summary>
        /// Uninstall a particular management pack
        /// </summary>
        /// <param name="managementPackName">Name of the management pack.  This must not include any file extension.</param>
        public void UninstallManagementPack(string managementPackName)
        {
            string query = "Name = '" + managementPackName + "'";
            ManagementPackCriteria managementPackCriteria = new ManagementPackCriteria(query);
            IList<ManagementPack> managementPacks = this.managementGroup.ManagementPacks.GetManagementPacks(managementPackCriteria);
            if (managementPacks.Count != 1)
            {
                throw new ManageMPException("No Management Pack found with " + query);
            }

            this.UninstallManagementPack(managementPacks[0]);
        }

        /// <summary>
        /// Uninstall a specific management pack
        /// </summary>
        /// <param name="mp">Management Pack to uninstall</param>
        public void UninstallManagementPack(ManagementPack mp)
        {
            this.logger("Uninstalling MP: " + mp.Name);
            this.managementGroup.ManagementPacks.UninstallManagementPack(mp);
        }

        /// <summary>
        /// Uninstall a specific management pack bundle
        /// </summary>
        /// <param name="mpBundle">Management Pack Bundle to uninstall</param>
        public void UninstallBundle(ManagementPackBundle mpBundle)
        {
            this.logger("Uninstalling the bundle of following MPs: ");
            foreach (ManagementPack mp in mpBundle.ManagementPacks)
            {
                this.logger(mp.Name);
            }

            this.managementGroup.ManagementPacks.UninstallBundle(mpBundle);
        }

        /// <summary>
        /// Uninstall all management packs matching the given substring, e.g., 'Microsoft.Linux'
        /// </summary>
        /// <param name="managementPackBundleNameSubstring">Uninstall management packs whose names include the given string</param>
        public void UninstallBundle(string managementPackBundleNameSubstring)
        {
            this.logger(string.Format("UninstallManagementPackBundles ({0})", managementPackBundleNameSubstring));

            IList<ManagementPack> managementPacks = this.managementGroup.ManagementPacks.GetManagementPacks();

            int mpIndex = 0;

            foreach (ManagementPack managementPack in managementPacks)
            {
                if (managementPack.Name.Contains(managementPackBundleNameSubstring))
                {
                    mpIndex++;
                }
            }

            ManagementPack[] matchingMPs = new ManagementPack[mpIndex];
            mpIndex = 0;

            foreach (ManagementPack managementPack in managementPacks)
            {
                if (managementPack.Name.Contains(managementPackBundleNameSubstring))
                {
                    matchingMPs[mpIndex++] = managementPack;
                }
            }

            matchingMPs = SortMpsInDependencyOrder(matchingMPs);

            ManagementPackBundle mpBundle = this.managementGroup.ManagementPacks.GetBundle(matchingMPs);

            this.UninstallBundle(mpBundle);            
        }

        /// <summary>
        /// Imports all the management pack bundles in the management pack directory matching a given pattern
        /// </summary>
        /// <param name="filePattern">File patter to use in importing management pack bundle, for example, "*.mpb"</param>
        public void ImportBundle(string filePattern)
        {
            DirectoryInfo dir = new DirectoryInfo(this.managementPackDirectory);

            if (!filePattern.EndsWith(".mpb"))
            {
                throw new System.ArgumentException("The filePattern is not right. The one likes \"*.mpb\" is needed...");
            }

            FileInfo[] files = dir.GetFiles(filePattern);
            ManagementPackBundle[] mpBundles = new ManagementPackBundle[files.Length];
            int i = 0;
            ManagementPackBundleReader mpBundleReader = ManagementPackBundleFactory.CreateBundleReader();
            foreach (FileInfo file in files)
            {
                mpBundles[i++] = mpBundleReader.Read(file.FullName, this.managementGroup);
            }

            for (i = 0; i < mpBundles.Length; i++)
            {
                try
                {
                    this.managementGroup.ManagementPacks.ImportBundle(mpBundles[i]);
                }
                catch
                {
                    throw new InvalidOperationException("The managment pack bundle imported unsucessfully...");
                }
            }
        }

        /// <summary>
        /// Imports all the management pack bundles in the management pack directory matching a given pattern
        /// </summary>
        public void ImportBundle()
        {
            this.ImportBundle("*.mpb");
        }    

        #endregion
    }

    /// <summary>
    /// Exception specific to this class.
    /// </summary>
    public class ManageMPException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ManageMPException class.
        /// </summary>
        /// <param name="msg">Error message</param>
        public ManageMPException(string msg)
            : base(msg)
        {
        }
    }
}
