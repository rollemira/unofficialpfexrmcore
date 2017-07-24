// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================

using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Client;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    ///     Class that handles renewing the <see cref="SecurityTokenResponse" /> if expired
    /// </summary>
    public sealed class AutoRefreshSecurityToken<TProxy, TService>
        where TProxy : ServiceProxy<TService> where TService : class
    {
        private readonly TProxy _proxy;

        /// <summary>
        ///     Instantiates an instance of the proxy class
        /// </summary>
        /// <param name="proxy">Proxy that will be used to authenticate the user</param>
        public AutoRefreshSecurityToken(TProxy proxy)
        {
            if (proxy == null) throw new ArgumentNullException("proxy");

            _proxy = proxy;
        }

        /// <summary>
        ///     Prepares authentication before authenticated
        /// </summary>
        public void PrepareCredentials()
        {
            if (_proxy.ClientCredentials == null) return;

            switch (_proxy.ServiceConfiguration.AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    _proxy.ClientCredentials.UserName.UserName = null;
                    _proxy.ClientCredentials.UserName.Password = null;
                    break;
                case AuthenticationProviderType.Federation:
                case AuthenticationProviderType.OnlineFederation:
                case AuthenticationProviderType.LiveId:
                    _proxy.ClientCredentials.Windows.ClientCredential = null;
                    break;
                default: return;
            }
        }

        /// <summary>
        ///     Renews the token for non-AD scenarios (if it is near expiration or has expired)
        /// </summary>
        public void RenewTokenIfRequired()
        {
            if (_proxy.ServiceConfiguration.AuthenticationType != AuthenticationProviderType.ActiveDirectory &&
                _proxy.SecurityTokenResponse != null && DateTime.UtcNow.AddMinutes(15) >=
                _proxy.SecurityTokenResponse.Response.Lifetime.Expires)
                try
                {
                    _proxy.Authenticate();
                }
                catch (CommunicationException)
                {
                    if (_proxy.SecurityTokenResponse == null ||
                        DateTime.UtcNow >= _proxy.SecurityTokenResponse.Response.Lifetime.Expires) throw;

                    // Ignore the exception 
                }
        }
    }
}