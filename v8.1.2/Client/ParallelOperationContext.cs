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

using System.Collections.Generic;

namespace Microsoft.Pfe.Xrm
{
    /// <summary>
    ///     A parallel operation context object that maintains a reference to a Discovery.svc channel
    /// </summary>
    /// <typeparam name="TResponse">The expected response type to collect</typeparam>
    /// <remarks>
    ///     ASSUMPTION: The local reference temporarily points to a threadlocal instance shared across partitions, thus we do
    ///     not dispose from the context directly
    /// </remarks>
    internal sealed class ParallelDiscoveryOperationContext<TRequest, TResponse> : ParallelOperationContext<
        ManagedTokenDiscoveryServiceProxy, TResponse, ParallelDiscoveryOperationFailure<TRequest>>
    {
        public ParallelDiscoveryOperationContext(ManagedTokenDiscoveryServiceProxy proxy) : base(proxy)
        {
        }
    }

    /// <summary>
    ///     A parallel operation context object that maintains a reference to a Organization.svc channel
    /// </summary>
    /// <typeparam name="TResponse">The expected response type to collect</typeparam>
    /// <remarks>
    ///     ASSUMPTION: The local reference temporarily points to a threadlocal instance shared across partitions, thus we do
    ///     not dispose from the context directly
    /// </remarks>
    internal sealed class ParallelOrganizationOperationContext<TRequest, TResponse> : ParallelOperationContext<
        ManagedTokenOrganizationServiceProxy, TResponse, ParallelOrganizationOperationFailure<TRequest>>
    {
        public ParallelOrganizationOperationContext()
        {
        }

        public ParallelOrganizationOperationContext(ManagedTokenOrganizationServiceProxy proxy) : base(proxy)
        {
        }
    }

    /// <summary>
    ///     A context object can be passed between iterations of a parallelized process partition
    ///     Maintains a reference to a ThreadLocal
    ///     <OrganizationServiceProxy>
    ///         .Value and
    ///         implements ILocalResults<TResponse> for collecting partitioned results in parallel operations
    /// </summary>
    /// <typeparam name="TResponse">The expected response type to collect</typeparam>
    internal class ParallelOperationContext<TLocal, TResponse, TFailure> : ILocalResults<TResponse, TFailure>
        where TFailure : IParallelOperationFailure
    {
        protected ParallelOperationContext()
        {
        }

        public ParallelOperationContext(TLocal local)
        {
            Local = local;
        }

        public TLocal Local { get; set; }

        #region ILocalResults<TResponse> Members

        private IList<TResponse> results;

        public IList<TResponse> Results
        {
            get
            {
                if (results == null) results = new List<TResponse>();

                return results;
            }
        }

        private IList<TFailure> failures;

        public IList<TFailure> Failures
        {
            get
            {
                if (failures == null) failures = new List<TFailure>();

                return failures;
            }
        }

        #endregion
    }
}