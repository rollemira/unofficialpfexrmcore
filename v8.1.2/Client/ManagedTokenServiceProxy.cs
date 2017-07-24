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

using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    ///     Wrapper class for <see cref="DiscoveryServiceProxy" /> to support auto refresh security token
    /// </summary>
    public sealed class ManagedTokenDiscoveryServiceProxy : DiscoveryServiceProxy
    {
        private readonly AutoRefreshSecurityToken<DiscoveryServiceProxy, IDiscoveryService> _proxyManager;

        public ManagedTokenDiscoveryServiceProxy(IServiceManagement<IDiscoveryService> serviceManagement,
            SecurityTokenResponse securityTokenResponse) : base(serviceManagement, securityTokenResponse)
        {
            _proxyManager = new AutoRefreshSecurityToken<DiscoveryServiceProxy, IDiscoveryService>(this);
        }

        public ManagedTokenDiscoveryServiceProxy(IServiceManagement<IDiscoveryService> serviceManagement,
            ClientCredentials credentials) : base(serviceManagement, credentials)
        {
            _proxyManager = new AutoRefreshSecurityToken<DiscoveryServiceProxy, IDiscoveryService>(this);
        }

        protected override void AuthenticateCore()
        {
            _proxyManager.PrepareCredentials();
            base.AuthenticateCore();
        }

        protected override void ValidateAuthentication()
        {
            _proxyManager.RenewTokenIfRequired();
            base.ValidateAuthentication();
        }
    }

    /// <summary>
    ///     Wrapper class for <see cref="OrganizationServiceProxy" /> to support auto refresh security token
    /// </summary>
    public sealed class ManagedTokenOrganizationServiceProxy : OrganizationServiceProxy
    {
        private readonly AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService> _proxyManager;

        public ManagedTokenOrganizationServiceProxy(IServiceManagement<IOrganizationService> serviceManagement,
            SecurityTokenResponse securityTokenResponse) : base(serviceManagement, securityTokenResponse)
        {
            _proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this);
        }

        public ManagedTokenOrganizationServiceProxy(IServiceManagement<IOrganizationService> serviceManagement,
            ClientCredentials credentials) : base(serviceManagement, credentials)
        {
            _proxyManager = new AutoRefreshSecurityToken<OrganizationServiceProxy, IOrganizationService>(this);
        }

        protected override void AuthenticateCore()
        {
            _proxyManager.PrepareCredentials();
            base.AuthenticateCore();
        }

        protected override void ValidateAuthentication()
        {
            _proxyManager.RenewTokenIfRequired();
            base.ValidateAuthentication();
        }
    }
}