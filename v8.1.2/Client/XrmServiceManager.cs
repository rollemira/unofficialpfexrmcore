﻿/*================================================================================================================================

  This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.  

  THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

  We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object 
  code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software 
  product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the 
  Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims 
  or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.

 =================================================================================================================================*/

using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Crm.Services.Utility;
using Microsoft.Pfe.Xrm.Caching;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    ///     Wrapper class for managing the service configuration of the Dynamics CRM Discovery.svc endpoint
    /// </summary>
    public class DiscoveryServiceManager : XrmServiceManager<IDiscoveryService, ManagedTokenDiscoveryServiceProxy>
    {
        #region Constructor(s)

        /// <summary>
        ///     Establishes an <see cref="IDiscoveryService" /> configuration at Uri location using supplied
        ///     <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="credentials">The auth credentials</param>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm Uri by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        public DiscoveryServiceManager(Uri serviceUri, AuthenticationCredentials credentials) : base(serviceUri,
            credentials)
        {
        }

        /// <summary>
        ///     Establishes an <see cref="IDiscoveryService" /> configuration at Uri location using supplied identity details
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="username">The username of the identity to authenticate</param>
        /// <param name="password">The password of the identity to authenticate</param>
        /// <param name="domain">Optional parameter for specifying the domain (when known)</param>
        /// <param name="homeRealm">Optional parameter for specifying the federated home realm location (when known)</param>
        public DiscoveryServiceManager(Uri serviceUri, string username, string password, string domain = null,
            Uri homeRealm = null) : base(serviceUri, username, password, domain, homeRealm)
        {
        }

        /// <summary>
        ///     Manages an established <see cref="IDiscoveryService" /> configuration using supplied
        ///     <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <param name="credentials">The auth credentials</param>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm Uri by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        public DiscoveryServiceManager(IServiceManagement<IDiscoveryService> serviceManagement,
            AuthenticationCredentials credentials) : base(serviceManagement, credentials)
        {
        }

        /// <summary>
        ///     Manages an established <see cref="IDiscoveryService" /> configuration using DefaultNetworkCredentials
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <remarks>
        ///     This approach authenticates using DefaultNetworkCredentials (AD) since no credentials are supplied
        /// </remarks>
        public DiscoveryServiceManager(IServiceManagement<IDiscoveryService> serviceManagement) :
            base(serviceManagement)
        {
        }

        #endregion

        #region Properties

        private ParallelDiscoveryServiceProxy parallelProxy;

        public ParallelDiscoveryServiceProxy ParallelProxy
        {
            get
            {
                if (parallelProxy == null) parallelProxy = new ParallelDiscoveryServiceProxy(this);

                return parallelProxy;
            }
        }

        #endregion
    }

    /// <summary>
    ///     Wrapper class for managing the service configuration of the Dynamics CRM Organization.svc endpoint
    /// </summary>
    public class OrganizationServiceManager : XrmServiceManager<IOrganizationService,
        ManagedTokenOrganizationServiceProxy>
    {
        #region Fields

        private ParallelOrganizationServiceProxy parallelProxy;

        #endregion

        #region Properties

        public ParallelOrganizationServiceProxy ParallelProxy
        {
            get
            {
                if (parallelProxy == null) parallelProxy = new ParallelOrganizationServiceProxy(this);

                return parallelProxy;
            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Establishes an <see cref="IOrganizationService" /> configuration at Uri location using supplied
        ///     <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="credentials">The auth credentials</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm Uri by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        public OrganizationServiceManager(Uri serviceUri, AuthenticationCredentials credentials, ICacheStrategy cacheStrategy = null) : base(serviceUri,
            credentials, cacheStrategy)
        {
        }

        /// <summary>
        ///     Establishes an <see cref="IOrganizationService" /> configuration at Uri location using supplied identity details
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="username">The username of the identity to authenticate</param>
        /// <param name="password">The password of the identity to authenticate</param>
        /// <param name="domain">Optional parameter for specifying the domain (when known)</param>
        /// <param name="homeRealm">Optional parameter for specifying the federated home realm location (when known)</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        public OrganizationServiceManager(Uri serviceUri, string username, string password, 
            ICacheStrategy cacheStrategy = null) : base(serviceUri, username, password, null, null, cacheStrategy)
        {
        }

        /// <summary>
        ///     Establishes an <see cref="IOrganizationService" /> configuration at Uri location using supplied identity details
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="username">The username of the identity to authenticate</param>
        /// <param name="password">The password of the identity to authenticate</param>
        /// <param name="domain">Optional parameter for specifying the domain (when known)</param>
        /// <param name="homeRealm">Optional parameter for specifying the federated home realm location (when known)</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        public OrganizationServiceManager(Uri serviceUri, string username, string password, string domain = null,
            Uri homeRealm = null, ICacheStrategy cacheStrategy = null) : base(serviceUri, username, password, domain, homeRealm, cacheStrategy)
        {
        }

        /// <summary>
        ///     Manages an established <see cref="IOrganizationService" /> configuration using supplied
        ///     <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <param name="credentials">The auth credentials</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm Uri by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        public OrganizationServiceManager(IServiceManagement<IOrganizationService> serviceManagement,
            AuthenticationCredentials credentials, ICacheStrategy cacheStrategy = null) : base(serviceManagement, credentials, cacheStrategy)
        {
        }

        /// <summary>
        ///     Manages an established <see cref="IOrganizationService" /> configuration using DefaultNetworkCredentials
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     This approach authenticates using DefaultNetworkCredentials (AD) since no credentials are supplied
        /// </remarks>
        public OrganizationServiceManager(IServiceManagement<IOrganizationService> serviceManagement, ICacheStrategy cacheStrategy = null) :
            base(serviceManagement, cacheStrategy)
        {
        }

        /// <summary>
        ///     Manages an established <see cref="IOrganizationService" /> configuration using DefaultNetworkCredentials
        /// </summary>
        /// <param name="serviceUri">Uri to use for establishing <see cref="IServiceManagement" /></param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     This approach authenticates using DefaultNetworkCredentials (AD only) since no credentials are supplied
        /// </remarks>
        public OrganizationServiceManager(Uri serviceUri, ICacheStrategy cacheStrategy = null) : this(ServiceConfigurationFactory
            .CreateManagement<IOrganizationService>(serviceUri), cacheStrategy)
        {
        }

        #endregion
    }

    /// <summary>
    ///     Generic class for establishing and managing a service configuration for Dynamics CRM endpoints
    /// </summary>
    /// <typeparam name="TService">
    ///     Set <see cref="IDiscoveryService" /> or <see cref="IOrganizationService" /> type to request
    ///     respective service proxy instances.
    /// </typeparam>
    /// <typeparam name="TProxy">
    ///     Set a proxy return type to either <see cref="DiscoveryServiceProxy" /> or
    ///     <see cref="OrganizationServiceProxy" /> type based on TService type.
    /// </typeparam>
    /// <remarks>
    ///     Provides a means to reuse thread-safe service configurations and security tokens to open multiple client service
    ///     proxies (channels)
    /// </remarks>
    public abstract class XrmServiceManager<TService, TProxy> : XrmServiceManagerBase
        where TService : class where TProxy : ServiceProxy<TService>
    {
        #region Fields

        private AuthenticationCredentials defaultCredentials;

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Default constructor
        /// </summary>
        private XrmServiceManager()
            : base(new NoCacheStrategy())
        {
            throw new NotImplementedException("Default constructor not implemented");
        }

        #region Uri Constructor(s)

        /// <summary>
        ///     Establishes a service configuration at <see cref="Uri" /> location using supplied
        ///     <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="credentials">The auth credentials</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm Uri by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        protected XrmServiceManager(Uri serviceUri, AuthenticationCredentials credentials, ICacheStrategy cacheStrategy = null)
            : base(cacheStrategy)
        {
            ServiceUri = serviceUri;
            ServiceManagement = ServiceConfigurationFactory.CreateManagement<TService>(serviceUri);

            Authenticate(credentials);
        }

        /// <summary>
        ///     Establishes a service configuration of type TService at <see cref="Uri" /> location using supplied identity details
        /// </summary>
        /// <param name="serviceUri">The service endpoint location</param>
        /// <param name="username">The username of the identity to authenticate</param>
        /// <param name="password">The password of the identity to authenticate</param>
        /// <param name="domain">Optional parameter for specifying the domain (when known)</param>
        /// <param name="homeRealm">Optional parameter for specifying the federated home realm location (when known)</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        protected XrmServiceManager(Uri serviceUri, string username, string password, string domain = null,
            Uri homeRealm = null, ICacheStrategy cacheStrategy = null)
            : base(cacheStrategy)
        {
            ServiceUri = serviceUri;
            ServiceManagement = ServiceConfigurationFactory.CreateManagement<TService>(serviceUri);

            Authenticate(username, password, domain, homeRealm);
        }

        #endregion

        #region IServiceManagement<TService> Constructor(s)

        /// <summary>
        ///     Manages an established service configuration using supplied <see cref="AuthenticationCredentials" />
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <param name="credentials">The auth credentials</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     <see cref="AuthenticationCredentials" /> can represent AD, Claims, or Cross-realm Claims
        ///     <see cref="ClientCredentials" />
        ///     The authCredentials may already contain a <see cref="SecurityTokenResponse" />
        ///     For cross-realm (federated) scenarios it can contain a HomeRealm <see cref="Uri" /> by itself, or also include a
        ///     <see cref="SecurityTokenResponse" /> from the federated realm
        /// </remarks>
        protected XrmServiceManager(IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials credentials, ICacheStrategy cacheStrategy = null)
            : base(cacheStrategy)
        {
            ServiceUri = serviceManagement.CurrentServiceEndpoint.Address.Uri;
            ServiceManagement = serviceManagement;

            Credentials = credentials != null ? credentials : DefaultCredentials;

            RequestSecurityToken();
        }

        /// <summary>
        ///     Manages an established service configuration using DefaultNetworkCredentials
        /// </summary>
        /// <param name="serviceManagement">The established service configuration management object</param>
        /// <param name="cacheStrategy"/>
        /// <see cref="ICacheStrategy"/>
        /// <remarks>
        ///     This approach authenticates using default network credentials (AD) since no credentials are supplied
        /// </remarks>
        protected XrmServiceManager(IServiceManagement<TService> serviceManagement, ICacheStrategy cacheStrategy = null) : this(serviceManagement, null, cacheStrategy)
        {
        }

        #endregion

        #endregion

        #region Properties

        #region Private

        private IServiceManagement<TService> ServiceManagement { get; }
        private AuthenticationCredentials Credentials { get; set; }
        private AuthenticationCredentials AuthenticatedCredentials { get; set; }

        private AuthenticationCredentials DefaultCredentials
        {
            get
            {
                if (defaultCredentials == null)
                    defaultCredentials = new AuthenticationCredentials
                    {
                        ClientCredentials =
                            new ClientCredentials
                            {
                                Windows = {ClientCredential = CredentialCache.DefaultNetworkCredentials}
                            }
                    };

                return defaultCredentials;
            }
        }

        /// <summary>
        ///     Is TService an IOrganizationService type
        /// </summary>
        private bool IsOrganizationService => typeof(TService).Equals(typeof(IOrganizationService));

        /// <summary>
        ///     If AuthCredentials has a SecurityTokenResponse, then true. Otherwise, false;
        /// </summary>
        private bool HasToken
        {
            get
            {
                if (AuthenticatedCredentials != null &&
                    AuthenticatedCredentials.SecurityTokenResponse != null) return true;

                return false;
            }
        }

        /// <summary>
        ///     If security token is nearing expiration (15 minutes) or expired, then true. Otherwise, false
        /// </summary>
        private bool TokenExpired
        {
            get
            {
                if (HasToken && AuthenticatedCredentials.SecurityTokenResponse.Response.Lifetime.Expires <=
                    DateTime.UtcNow.AddMinutes(15)) return true;

                return false;
            }
        }

        #endregion

        /// <summary>
        ///     Current endpoint address
        /// </summary>
        public Uri ServiceUri { get; }

        /// <summary>
        ///     The <see cref="AuthenticationProviderType" /> of the targeted endpoint
        /// </summary>
        public AuthenticationProviderType AuthenticationType => ServiceManagement.AuthenticationType;

        /// <summary>
        ///     True if targeted endpoint's authentication provider type is LiveId or OnlineFederation, otherwise False
        /// </summary>
        public bool IsCrmOnline => AuthenticationType == AuthenticationProviderType.LiveId ||
                                   AuthenticationType == AuthenticationProviderType.OnlineFederation;

        #endregion

        #region Methods

        #region Private        

        /// <summary>
        ///     Handles identity authentication when the authentication provider is known
        /// </summary>
        /// <param name="credentials">The credentials representing the identity to authenticate</param>
        private void Authenticate(AuthenticationCredentials credentials)
        {
            Credentials = credentials;

            RequestSecurityToken();
        }

        /// <summary>
        ///     Handles identity authentication based on the service endpoint's authentication provider type
        /// </summary>
        /// <param name="username">The username of the identity to authenticate</param>
        /// <param name="password">The password of the identity to authenticate</param>
        /// <param name="domain">Optional parameter for specifying the domain (when known)</param>
        /// <param name="homeRealmUri">Optional parameter for specifying the federated home realm location (when known)</param>
        /// <remarks>
        ///     Invokes helper methods for each authentication type that in turn perform the request for a security token from the
        ///     auth provider
        /// </remarks>
        private void Authenticate(string username, string password, string domain = null, Uri homeRealmUri = null)
        {
            switch (AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    AuthenticateCredentials(new ClientCredentials
                    {
                        Windows = {ClientCredential = new NetworkCredential(username, password, domain)}
                    });
                    return;

                case AuthenticationProviderType.Federation:
                case AuthenticationProviderType.OnlineFederation:
                    AuthenticateFederatedRealmCredentials(
                        new ClientCredentials {UserName = {UserName = username, Password = password}}, homeRealmUri);
                    return;

                case AuthenticationProviderType.LiveId:
                    AuthenticateLiveIdCredentials(
                        new ClientCredentials {UserName = {UserName = username, Password = password}});
                    return;

                default:
                    throw new NotSupportedException(string.Format("{0} authentication type is not supported",
                        ServiceManagement.AuthenticationType));
            }
        }

        /// <summary>
        ///     Prepare and authenticate an OSDP/Office365 <see cref="AuthenticationCredentials" /> using UPN for SSO
        /// </summary>
        /// <param name="userPrincipalName">The user principal name (UPN)</param>
        private void AuthenticateSingleSignOnCredentials(string userPrincipalName)
        {
            Credentials = new AuthenticationCredentials {UserPrincipalName = userPrincipalName};

            RequestSecurityToken();
        }

        /// <summary>
        ///     Prepare and authenticate client credentials
        /// </summary>
        /// <param name="clientCredentials">The client credentials</param>
        private void AuthenticateCredentials(ClientCredentials clientCredentials)
        {
            Credentials = new AuthenticationCredentials {ClientCredentials = clientCredentials};

            RequestSecurityToken();
        }

        /// <summary>
        ///     Prepare and authenticate client credentials from a federated realm
        /// </summary>
        /// <param name="clientCredentials">The client credentials</param>
        /// <param name="HomeRealmUri">The federated home realm location</param>
        private void AuthenticateFederatedRealmCredentials(ClientCredentials clientCredentials, Uri HomeRealmUri)
        {
            Credentials =
                new AuthenticationCredentials {ClientCredentials = clientCredentials, HomeRealm = HomeRealmUri};

            RequestSecurityToken();
        }

        /// <summary>
        ///     Prepare and authenticate client credentials and supporting device credentials for LiveID scenario
        /// </summary>
        /// <param name="clientCredentials">The client credentials (Microsoft Account)</param>
        /// <remarks>Implicitly registers device credentials using deviceidmanager.cs helper</remarks>
        private void AuthenticateLiveIdCredentials(ClientCredentials clientCredentials)
        {
            //Attempt to call .LoadOrRegisterDevice using IssuerEndpoint to load existing and/or persist to file.
            var deviceCredentials = ServiceManagement.IssuerEndpoints.ContainsKey("Username")
                ? DeviceIdManager.LoadOrRegisterDevice(ServiceManagement.IssuerEndpoints["Username"].IssuerAddress.Uri)
                : DeviceIdManager.LoadOrRegisterDevice();

            AuthenticateLiveIdCredentials(clientCredentials, deviceCredentials);
        }

        /// <summary>
        ///     Prepare and authenticate client credentials and supporting device credentials for LiveID scenario
        /// </summary>
        /// <param name="clientCredentials">The client credentials</param>
        /// <param name="deviceCredentials">The supporting device credentials</param>
        private void AuthenticateLiveIdCredentials(ClientCredentials clientCredentials,
            ClientCredentials deviceCredentials)
        {
            Credentials = new AuthenticationCredentials
            {
                ClientCredentials = clientCredentials,
                SupportingCredentials = new AuthenticationCredentials {ClientCredentials = deviceCredentials}
            };

            RequestSecurityToken();
        }

        /// <summary>
        ///     Clears any existing (potentially expired) tokens and issues request for new security token
        /// </summary>
        private void RefreshSecurityToken()
        {
            // Clear potentially expired cross-realm token
            Credentials.SecurityTokenResponse = null;
            // Clear potentially expired previously issued token
            AuthenticatedCredentials = null;

            RequestSecurityToken();
        }

        /// <summary>
        ///     Request a security token from the identity provider using the supplied credentials
        /// </summary>
        /// <remarks>
        ///     Invokes <see cref="IServiceManagement{TService}" />.Authenticate() method to perform claims request
        ///     Updates the stored credentials with the authentication response that includes the security token
        ///     Only performs this action if endpoint indicates a non-AD authentication provider scenario
        ///     <see cref="IServiceManagement{TService}" />.Authenticate() handles multiple scenarios:
        ///     1: Gets security token in current realm for AuthenticationCredentials.ClientCredentials.UserName
        ///     2: Gets security token in current realm by authenticating cross-realm for
        ///     AuthenticationCredentials.ClientCredentials.UserName using AuthenticationCredentials.HomeRealm
        ///     3: Gets securtiy token in current realm using the cross-realm AuthenticationCredentials.SecurityTokenResponse from
        ///     AuthenticationCredentials.HomeRealm
        /// </remarks>
        private void RequestSecurityToken()
        {
            //Obtain a security token only for non-AD scenarios
            if (AuthenticationType != AuthenticationProviderType.ActiveDirectory && Credentials != null)
                AuthenticatedCredentials = ServiceManagement.Authenticate(Credentials);
        }

        #endregion

        protected T GetProxy<T>()
        {
            // Obtain discovery/organization service proxy based on Authentication Type
            switch (ServiceManagement.AuthenticationType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    // Invokes ManagedTokenDiscoveryServiceProxy or ManagedTokenOrganizationServiceProxy 
                    // (IServiceManagement<TService>, ClientCredentials) constructor.
                    return (T) typeof(T)
                        .GetConstructor(new[] {typeof(IServiceManagement<TService>), typeof(ClientCredentials)})
                        .Invoke(new object[] {ServiceManagement, Credentials.ClientCredentials});

                case AuthenticationProviderType.Federation:
                case AuthenticationProviderType.OnlineFederation:
                case AuthenticationProviderType.LiveId:
                    //If we don't already have a token or token is expired, refresh the token
                    if (!HasToken || TokenExpired) RefreshSecurityToken();

                    // Invokes ManagedTokenOrganizationServiceProxy or ManagedTokenDiscoveryServiceProxy 
                    // (IServiceManagement<TService>, SecurityTokenResponse) constructor.
                    return (T) typeof(T)
                        .GetConstructor(new[] {typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse)})
                        .Invoke(new object[] {ServiceManagement, AuthenticatedCredentials.SecurityTokenResponse});

                default:
                    throw new NotSupportedException(string.Format("{0} authentication type is not supported",
                        ServiceManagement.AuthenticationType));
            }
        }

        /// <summary>
        ///     Sets up a new proxy connection of type TProxy using <see cref="IServiceManagement{TService}" />
        /// </summary>
        /// <returns>
        ///     An instance of a managed token TProxy
        ///     i.e. <see cref="DiscoveryServiceProxy" /> or <see cref="OrganizationServiceProxy" />
        /// </returns>
        /// <remarks>
        ///     The proxy represents a client service channel to a service endpoint.
        ///     Proxy connections should be disposed of properly before they fall out of scope to free up the allocated service
        ///     channel.
        /// </remarks>
        public TProxy GetProxy()
        {
            return GetProxy<TProxy>();
        }

        #endregion
    }

    /// <summary>
    ///     Base class for XrmServiceManager
    /// </summary>
    public abstract class XrmServiceManagerBase
    {

        #region Fields

        internal ServiceManagerCache Cache { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheStrategy">The cache strategy to be used, default is CacheStrategies.None</param>
        /// <see cref="CacheStrategies"/>
        public XrmServiceManagerBase(ICacheStrategy cacheStrategy)
        {
            //if cache strategy is null, assume no caching
            Cache = new ServiceManagerCache(cacheStrategy ?? CacheStrategies.None);
        } 

        #endregion
    }
}