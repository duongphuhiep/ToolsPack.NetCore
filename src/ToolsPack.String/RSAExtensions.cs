//
// Copyright 2021 Johannes Passing, https://jpassing.com/
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.   
// 

using System;
using System.Security.Cryptography;
using System.Text;

namespace ToolsPack.String
{
    public static class RSAExtensions
    {
        private const string RsaPublickeyPemHeader = "-----BEGIN RSA PUBLIC KEY-----";
        private const string RsaPublickeyPemFooter = "-----END RSA PUBLIC KEY-----";
        private const string SubjectPublicKeyInfoPemHeader = "-----BEGIN PUBLIC KEY-----";
        private const string SubjectPublicKeyInfoPemFooter = "-----END PUBLIC KEY-----";

        //---------------------------------------------------------------------
        // NetFx surrogate implementations for methods available in .NET Core/
        // .NET 5+.
        //---------------------------------------------------------------------


        //---------------------------------------------------------------------
        // Convenience methods for reading/writing PEM-encoded keys.
        //---------------------------------------------------------------------

        // public static void ImportFromPem(
        //     this RSA key,
        //     string source)
        //     => ImportFromPem(key, source, out _);
        //
        // public static void ImportFromPem(
        //     this RSA key,
        //     string source,
        //     out RsaPublicKeyFormat format)
        // {
        //     //
        //     // Inspect header to determine format.
        //     //
        //     if (source.StartsWith(SubjectPublicKeyInfoPemHeader) &&
        //         source.EndsWith(SubjectPublicKeyInfoPemFooter))
        //     {
        //         format = RsaPublicKeyFormat.SubjectPublicKeyInfo;
        //     }
        //     else if (source.StartsWith(RsaPublickeyPemHeader) &&
        //              source.EndsWith(RsaPublickeyPemFooter))
        //     {
        //         format = RsaPublicKeyFormat.RsaPublicKey;
        //     }
        //     else
        //     {
        //         throw new FormatException("Missing Public key header/footer");
        //     }
        //
        //     //
        //     // Decode body to get DER blob.
        //     //
        //     var der = Convert.FromBase64String(string.Concat(
        //         source
        //             .Split('\n')
        //             .Select(s => s.Trim())
        //             .Where(line => !line.StartsWith("-----"))));
        //     if (format == RsaPublicKeyFormat.RsaPublicKey)
        //     {
        //         key.ImportRSAPublicKey(der, out var _);
        //     }
        //     else
        //     {
        //         key.ImportSubjectPublicKeyInfo(der, out var _);
        //     }
        // }

#if NETSTANDARD2_1_OR_GREATER || NET

        /// <summary>
        /// Export public key to PEM format
        /// </summary>
        public static string ExportToPem(
            this RSA key,
            RsaPublicKeyFormat format)
        {
            var buffer = new StringBuilder();

            if (format == RsaPublicKeyFormat.RsaPublicKey)
            {
                buffer.AppendLine(RsaPublickeyPemHeader);
                buffer.AppendLine(Convert.ToBase64String(
                    key.ExportRSAPublicKey(),
                    Base64FormattingOptions.InsertLineBreaks));
                buffer.AppendLine(RsaPublickeyPemFooter);
            }
            else if (format == RsaPublicKeyFormat.SubjectPublicKeyInfo)
            {
                buffer.AppendLine(SubjectPublicKeyInfoPemHeader);
                buffer.AppendLine(Convert.ToBase64String(
                    key.ExportSubjectPublicKeyInfo(),
                    Base64FormattingOptions.InsertLineBreaks));
                buffer.AppendLine(SubjectPublicKeyInfoPemFooter);
            }
            else
            {
                throw new ArgumentException(nameof(format));
            }

            return buffer.ToString();
        }
#endif
    }

    public enum RsaPublicKeyFormat
    {
        /// <summary>
        /// -----BEGIN RSA PUBLIC KEY-----
        /// </summary>
        RsaPublicKey,

        /// <summary>
        /// -----BEGIN PUBLIC KEY-----
        /// </summary>
        SubjectPublicKeyInfo
    }

}