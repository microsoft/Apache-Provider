// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PuttyKeyHandler.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scx.Test.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Validates that a given file is a supported PuTTY private key for SSH user authentication.
    /// </summary>
    public class PuttyKeyHandler
    {
        /// <summary>
        /// Original contents of the file, line by line
        /// </summary>
        private readonly List<string> keyContents;

        /// <summary>
        /// Current state of the validation process
        /// </summary>
        private State state;

        /// <summary>
        /// Number of public/private lines to parse
        /// </summary>
        private int numLinesInBlob;

        /// <summary>
        /// Resulting key, in internal format
        /// </summary>
        private SecureString sshKey;
        
        /// <summary>
        /// Initializes a new instance of the PuttyKeyHandler class to validate the given key.
        /// </summary>
        /// <param name="keyContents">
        ///     The contents of the key to validate
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the \c keyContents null or empty.
        /// </exception>
        public PuttyKeyHandler(List<string> keyContents)
        {
            if (keyContents == null || keyContents.Count == 0)
            {
                throw new ArgumentNullException("keyContents", String.Format(CultureInfo.CurrentCulture, "The SSH key could not be read because the credentials do not specify a key file."));
            }

            this.state = State.ValidateKeyType;
            this.numLinesInBlob = 0;
            this.keyContents = keyContents;
        }

        /// <summary>
        /// Validation state types
        /// </summary>
        private enum State
        {
            /// <summary>
            /// The enum ValidateKeyType of the State.
            /// </summary>
            ValidateKeyType,

            /// <summary>
            /// The enum ValidateEncryption of the State.
            /// </summary>
            ValidateEncryption,

            /// <summary>
            /// The enum ValidateComment of the State.
            /// </summary>
            ValidateComment,

            /// <summary>
            /// The enum ValidatePublicLines of the State.
            /// </summary>
            ValidatePublicLines,

            /// <summary>
            /// The enum ValidatePublicKeyBlob of the State.
            /// </summary>
            ValidatePublicKeyBlob,

            /// <summary>
            /// The enum ValidatePrivateLines of the State.
            /// </summary>
            ValidatePrivateLines,

            /// <summary>
            /// The enum ValidatePrivateKeyBlob of the State.
            /// </summary>
            ValidatePrivateKeyBlob,

            /// <summary>
            /// The enum ValidateMAC of the State.
            /// </summary>
            ValidateMAC,

            /// <summary>
            /// The enum KeyIsValidated of the State.
            /// </summary>
            KeyIsValidated
        }

        /// <summary>
        /// Validate that the file associated with \c this object is a supported
        /// PuTTY private key for SSH user authentication.
        /// </summary>
        /// <exception cref="Microsoft.SystemCenter.CrossPlatform.ClientLibrary.ClientTasks.InvalidSshKeyException">
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

                return this.sshKey;
            }

            this.sshKey = new SecureString();

            for (int lineNumber = 0; lineNumber < this.keyContents.Count; lineNumber++)
            {
                string line = this.keyContents[lineNumber];
                if (String.IsNullOrWhiteSpace(line) == false)
                {
                    this.Parse(line, lineNumber + 1);
                }
            }

            if (this.state != State.KeyIsValidated)
            {
                throw new InvalidSshKeyException(InvalidSshKeyException.ValidationFault.UnrecognizedFormat);
            }

            return this.sshKey;
        }

        /// <summary>
        /// Pase the specified line text
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void Parse(string line, int lineNumber)
        {
            switch (this.state)
            {
                case State.ValidateKeyType:
                    this.ValidateKeyType(line);
                    break;

                case State.ValidateEncryption:
                    this.ValidateEncryption(line, lineNumber);
                    break;

                case State.ValidateComment:
                    this.ValidateComment(line, lineNumber);
                    break;

                case State.ValidatePublicLines:
                    this.ValidatePublicLines(line, lineNumber);
                    break;

                case State.ValidatePublicKeyBlob:
                    this.ValidatePublicKeyBlob(line, lineNumber);
                    break;

                case State.ValidatePrivateLines:
                    this.ValidatePrivateLines(line, lineNumber);
                    break;

                case State.ValidatePrivateKeyBlob:
                    this.ValidatePrivateKeyBlob(line, lineNumber);
                    break;

                case State.ValidateMAC:
                    this.ValidateMAC(line, lineNumber);
                    break;

                default:
                    throw new InvalidSshKeyException(lineNumber);
            }
        }

        /// <summary>
        /// Validate the specifid line's key type
        /// </summary>
        /// <param name="line">One line text</param>
        private void ValidateKeyType(string line)
        {
            const string PUTTY_V2_KEY_TYPE_PATTERN = @"^PuTTY-User-Key-File-2: ssh-(dss|rsa)$";

            Match match = Regex.Match(line, PUTTY_V2_KEY_TYPE_PATTERN);
            if (!match.Success)
            {
                // call ThrowUnknownFormatException to throw the correct exception
                this.ThrowUnknownFormatException(line);
            }

            this.AppendToKey(match.ToString());

            this.state = State.ValidateEncryption;
        }

        /// <summary>
        /// Validate the specifid line's Encryption
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void ValidateEncryption(string line, int lineNumber)
        {
            const string ENCRYPTION_PATTERN = @"^Encryption: (none|aes256-cbc)$";

            Match match = Regex.Match(line, ENCRYPTION_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            this.AppendToKey(match.ToString());

            this.state = State.ValidateComment;
        }

        /// <summary>
        /// Validate the specifid line's Comment
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void ValidateComment(string line, int lineNumber)
        {
            const string COMMENT_PATTERN = @"^Comment: .*$";

            Match match = Regex.Match(line, COMMENT_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            this.AppendToKey(match.ToString());

            this.state = State.ValidatePublicLines;
        }

        /// <summary>
        /// Validate the specifid public lines
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
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

            this.AppendToKey(match.ToString());

            this.state = State.ValidatePublicKeyBlob;
        }

        /// <summary>
        /// Validate the specifid line's public key Blob
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void ValidatePublicKeyBlob(string line, int lineNumber)
        {
            this.ValidateKeyBlob(line, lineNumber);

            if (this.numLinesInBlob == 0)
            {
                this.state = State.ValidatePrivateLines;
            }
        }

        /// <summary>
        /// Validate the specifid privatelines
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
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

            this.AppendToKey(match.ToString());

            this.state = State.ValidatePrivateKeyBlob;
        }

        /// <summary>
        /// Validate the specifid line's private key Blob
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void ValidatePrivateKeyBlob(string line, int lineNumber)
        {
            this.ValidateKeyBlob(line, lineNumber);

            if (this.numLinesInBlob == 0)
            {
                this.state = State.ValidateMAC;
            }
        }

        /// <summary>
        /// Validate the specifid line's Mac
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
        private void ValidateMAC(string line, int lineNumber)
        {
            const string MAC_PATTERN = @"^Private-MAC: [0-9a-fA-F]*$";

            Match match = Regex.Match(line, MAC_PATTERN);
            if (!match.Success)
            {
                throw new InvalidSshKeyException(lineNumber);
            }

            this.AppendToKey(match.ToString());

            this.state = State.KeyIsValidated;
        }

        /// <summary>
        /// Validate the specifid line'skey blob
        /// </summary>
        /// <param name="line">One line text</param>
        /// <param name="lineNumber">Line number</param>
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

            this.AppendToKey(match.ToString());
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
        private void ThrowUnknownFormatException(string firstLine)
        {
            const string PUTTY_V1_KEY_TYPE_PATTERN = @"^PuTTY-User-Key-File-1: ";
            const string SSH_V1_RSA_KEY_TYPE_PATTERN = @"^SSH PRIVATE KEY FILE FORMAT 1.1$";
            const string OPENSSH_KEY_TYPE_PATTERN = @"^-----BEGIN ";
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

        /// <summary>
        /// Append the specified line to SSH key
        /// </summary>
        /// <param name="line">One line text</param>
        private void AppendToKey(IEnumerable<char> line)
        {
            foreach (char ch in line)
            {
                this.sshKey.AppendChar(ch);
            }

            this.sshKey.AppendChar('\n');
        }
    }
}
