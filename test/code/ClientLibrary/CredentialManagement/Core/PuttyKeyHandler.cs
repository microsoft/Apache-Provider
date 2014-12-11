// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PuttyKeyHandler.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security;
    using System.Text.RegularExpressions;

    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Exceptions;
    using Microsoft.SystemCenter.CrossPlatform.ClientLibrary.CredentialManagement.Properties;

    /// <summary>
    ///     Validates that a given file is a supported PuTTY private key for SSH user authentication.
    /// </summary>
    public class PuttyKeyHandler : IDisposable
    {
        /// <summary>
        /// Validate that the file associated with \c this object is a supported
        /// PuTTY private key for SSH user authentication.
        /// </summary>
        /// <exception cref="InvalidSshKeyException">
        /// Thrown when the SSH key cannot be validated.
        /// </exception>
        /// <returns>
        /// The validated key as a secure string.
        /// </returns>
        public SecureString Validate()
        {
            if (this.sshKey != null)
            {
                if (this.state != State.KeyIsValidated)
                {
                    throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnrecognizedFormat);
                }

                return sshKey;
            }

            this.sshKey = new SecureString();

            for (int lineNumber = 0; lineNumber < this.keyContents.Count; lineNumber++)
            {
                string line = this.keyContents[lineNumber];

                // string.IsNullOrWhiteSpace(value)) is not available in .NET 3.5
                if ((string.IsNullOrEmpty(line) || line.Trim().Length == 0) == false)
                {
                    Parse(line, lineNumber + 1);
                }
            }

            if (this.state != State.KeyIsValidated)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnrecognizedFormat);
            }

            return this.sshKey.Copy();
        }


        /// <summary>
        ///     Initializes a new instance of the PuttyKeyHandler to validate the given key.
        /// </summary>
        /// 
        /// <param name="keyContents">
        ///     The contents of the key to validate
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the \c keyContents null or empty.
        /// </exception>
        public PuttyKeyHandler(List<string> keyContents)
        {
            if (keyContents == null || keyContents.Count == 0)
            {
                throw new ArgumentNullException("keyContents", string.Format(CultureInfo.CurrentCulture, Resources.NullSshKey));
            }

            this.state          = State.ValidateKeyType;
            this.numLinesInBlob = 0;
            this.keyContents    = keyContents;
        }

        /// <summary>
        ///     Frees disposable resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sshKey != null)
                {
                    sshKey.Dispose();
                    sshKey = null;
                }
            }
        }

        private void Parse(string line, int lineNumber)
        {
            switch (this.state)
            {
                case State.ValidateKeyType:
                    ValidateKeyType(line);
                    break;

                case State.ValidateEncryption:
                    ValidateEncryption(line, lineNumber);
                    break;

                case State.ValidateComment:
                    ValidateComment(line, lineNumber);
                    break;

                case State.ValidatePublicLines:
                    ValidatePublicLines(line, lineNumber);
                    break;

                case State.ValidatePublicKeyBlob:
                    ValidatePublicKeyBlob(line, lineNumber);
                    break;

                case State.ValidatePrivateLines:
                    ValidatePrivateLines(line, lineNumber);
                    break;

                case State.ValidatePrivateKeyBlob:
                    ValidatePrivateKeyBlob(line, lineNumber);
                    break;

                case State.ValidateMAC:
                    ValidateMAC(line, lineNumber);
                    break;

                default:
                    throw new InvalidSshKeyException(lineNumber);
            }
        }

        private void ValidateKeyType(string line)
        {
            const string PUTTY_V2_KEY_TYPE_PATTERN = @"^PuTTY-User-Key-File-2: ssh-(dss|rsa)$";

            Match match = Regex.Match(line, PUTTY_V2_KEY_TYPE_PATTERN);
            if (!match.Success)
            {
                // call ThrowUnknownFormatException to throw the correct exception
                ThrowUnknownFormatException(line);
            }

            AppendToKey(match.ToString());

            this.state = State.ValidateEncryption;
        }

        private void ValidateEncryption(string line, int lineNumber)
        {
            const string ENCRYPTION_PATTERN = @"^Encryption: (none|aes256-cbc)$";

            Match match = Regex.Match(line, ENCRYPTION_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            AppendToKey(match.ToString());

            this.state = State.ValidateComment;
        }

        private void ValidateComment(string line, int lineNumber)
        {
            const string COMMENT_PATTERN = @"^Comment: .*$";

            Match match = Regex.Match(line, COMMENT_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            AppendToKey(match.ToString());

            this.state = State.ValidatePublicLines;
        }

        private void ValidatePublicLines(string line, int lineNumber)
        {
            const string PUBLIC_LINES_PATTERN = @"^Public-Lines: ([1-9][0-9]*)$";

            Match match = Regex.Match(line, PUBLIC_LINES_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            // save the number of public lines to parse
            try
            {
                const string PUBLIC_LINES_REPLACE_VALUE_PATTERN = "$1";

                this.numLinesInBlob = int.Parse(match.Result(PUBLIC_LINES_REPLACE_VALUE_PATTERN));
            }
            catch (Exception e)
            {
                throw new InvalidSshKeyException(lineNumber, e);
            }
            
            AppendToKey(match.ToString());

            this.state = State.ValidatePublicKeyBlob;
        }

        private void ValidatePublicKeyBlob(string line, int lineNumber)
        {
            ValidateKeyBlob(line, lineNumber);

            if (this.numLinesInBlob == 0)
            {
                this.state = State.ValidatePrivateLines;
            }
        }

        private void ValidatePrivateLines(string line, int lineNumber)
        {
            const string PRIVATE_LINES_PATTERN = @"^Private-Lines: ([1-9][0-9]*)$";

            Match match = Regex.Match(line, PRIVATE_LINES_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            // save the number of public lines to parse
            try
            {
                const string PRIVATE_LINES_REPLACE_VALUE_PATTERN = "$1";

                this.numLinesInBlob = int.Parse(match.Result(PRIVATE_LINES_REPLACE_VALUE_PATTERN));
            }
            catch (Exception e)
            {
                throw new InvalidSshKeyException(lineNumber, e);
            }

            AppendToKey(match.ToString());

            this.state = State.ValidatePrivateKeyBlob;
        }

        private void ValidatePrivateKeyBlob(string line, int lineNumber)
        {
            ValidateKeyBlob(line, lineNumber);

            if (this.numLinesInBlob == 0)
            {
                this.state = State.ValidateMAC;
            }
        }


        private void ValidateMAC(string line, int lineNumber)
        {
            const string MAC_PATTERN = @"^Private-MAC: [0-9a-fA-F]*$";

            Match match = Regex.Match(line, MAC_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            AppendToKey(match.ToString());

            this.state = State.KeyIsValidated;
        }


        private void ValidateKeyBlob(string line, int lineNumber)
        {
            if (this.numLinesInBlob == 0)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            const string KEY_BLOB_PATTERN = @"^[A-Za-z0-9+/=]*$";

            Match match = Regex.Match(line, KEY_BLOB_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            // Except for the last line, each Base64 encoded line MUST have 64
            // characters.  If the last line is less than 64 characters, the
            // number of characters must be evenly divisible by 4.
            if ((match.ToString().Length > 64) ||
                ((this.numLinesInBlob != 1) && (match.ToString().Length != 64)) ||
                ((this.numLinesInBlob == 1) && ((match.ToString().Length % 4) != 0)))
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            AppendToKey(match.ToString());
            --this.numLinesInBlob;
        }


        /// <summary>
        ///     Throws an InvalidSshKeyException based on the first line of the
        ///     key file.
        /// </summary>
        /// <remarks>
        ///     This method should only be called by \c ValidateKeyType if the
        ///     first line of the key file does not match the signature for a
        ///     PuTTY v.2 private key.
        /// </remarks>
        /// <param name="firstLine">The first line of the key file.</param>
        private static void ThrowUnknownFormatException(string firstLine)
        {
            const string PUTTY_V1_KEY_TYPE_PATTERN       = @"^PuTTY-User-Key-File-1: ";
            const string SSH_V1_RSA_KEY_TYPE_PATTERN     = @"^SSH PRIVATE KEY FILE FORMAT 1.1$";
            const string OPENSSH_KEY_TYPE_PATTERN        = @"^-----BEGIN ";
            const string COMMERCIAL_SSH_KEY_TYPE_PATTERN = @"^---- BEGIN SSH2 ENCRYPTED PRIVATE";

            if (Regex.Match(firstLine, PUTTY_V1_KEY_TYPE_PATTERN).Success)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnsupportedVersion);
            }
            
            if (Regex.Match(firstLine, SSH_V1_RSA_KEY_TYPE_PATTERN).Success)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnsupportedFormatSshV1Rsa);
            }
            
            if (Regex.Match(firstLine, OPENSSH_KEY_TYPE_PATTERN).Success)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnsupportedFormatOpenSsh);
            }
            
            if (Regex.Match(firstLine, COMMERCIAL_SSH_KEY_TYPE_PATTERN).Success)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnsupportedFormatCommercialSsh);
            }

            throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnrecognizedFormat);
        }


        private void AppendToKey(IEnumerable<char> line)
        {
            try
            {
                foreach (char ch in line)
                {
                    this.sshKey.AppendChar(ch);
                }

                this.sshKey.AppendChar('\n');
            }
            catch (ArgumentOutOfRangeException)
            {
                // SecureString can only carry 65535 chars
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.MaxKeyLengthExceeded);
            }
        }

        private enum State
        {
            ValidateKeyType,
            ValidateEncryption,
            ValidateComment,
            ValidatePublicLines,
            ValidatePublicKeyBlob,
            ValidatePrivateLines,
            ValidatePrivateKeyBlob,
            ValidateMAC,
            KeyIsValidated
        }

        private State state;                        // Current state of the validation process
        private int numLinesInBlob;                 // Number of public/private lines to parse
        private readonly List<string> keyContents;  // Original contents of the file, line by line
        private SecureString sshKey;                // Resulting key, in internal format
    }
}
