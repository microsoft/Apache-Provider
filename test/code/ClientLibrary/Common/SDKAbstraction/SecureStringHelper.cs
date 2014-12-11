// -----------------------------------------------------------------------
// <copyright file="SecureStringHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.Common.SDKAbstraction
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    ///     This is a utility class that provides static methods to some common
    ///     <see cref="SecureString"/> functionality.
    /// </summary>
    public class SecureStringHelper
    {
        /// <summary>
        ///     An empty <see cref="SecureString"/>.
        /// </summary>
        /// <remarks>
        ///     This is a <see langword="readonly"/> reference.  Be aware of the
        ///     comparison semantics for <see cref="SecureString"/> references.  Two
        ///     references are equal only if they are the same reference.  However,
        ///     this reference can be <see cref="Decrypt">Decrypted</see> and
        ///     compared using normal <see cref="string"/> comparison.  This
        ///     reference can also be <see cref="SecureString.Copy">copied</see>
        ///     to get a new, modifiable reference (this is the same as using
        ///     <c>new SecureString()</c>).
        /// </remarks>
        public static readonly SecureString Empty;
        

        /// <summary>
        ///     Encrypt the source into a <see cref="SecureString"/>.
        /// </summary>
        /// <param name="source">
        ///     The source string to encrypt.
        /// </param>
        /// <returns>
        ///     This method returns the source <see cref="string"/> as a <see cref="SecureString"/>.
        /// </returns>
        public static SecureString Encrypt(string source)
        {
            if (source == null)
            {
                return null;
            }

            using (SecureString result = new SecureString())
            {
                foreach (char ch in source)
                {
                    result.AppendChar(ch);
                }

                return result.Copy();
            }
        }


        /// <summary>
        ///     Encrypt the source into a <see cref="SecureString"/> and make it read-only.
        /// </summary>
        /// <param name="source">
        ///     The source string to encrypt.
        /// </param>
        /// <returns>
        ///     This method returns the source <see cref="string"/> as a read-only <see cref="SecureString"/>.
        /// </returns>
        public static SecureString EncryptAndMakeReadOnly(string source)
        {
            SecureString result = Encrypt(source);

            if (result != null)
            {
                result.MakeReadOnly();
            }

            return result;
        }


        /// <summary>
        ///     Decrypt the source into a <see cref="string"/>.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="SecureString"/> source to decrypt.
        /// </param>
        /// <returns>
        ///     This method returns <see cref="SecureString"/> source as a <see cref="string"/>.
        /// </returns>
        public static string Decrypt(SecureString source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(source);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }


        /// <summary>
        ///     This method initializes the <see cref="Empty"/> field and sets
        ///     the object instance's read-only attribute.
        /// </summary>
        static SecureStringHelper()
        {
            Empty = EncryptAndMakeReadOnly(string.Empty);
        }
    }
}
