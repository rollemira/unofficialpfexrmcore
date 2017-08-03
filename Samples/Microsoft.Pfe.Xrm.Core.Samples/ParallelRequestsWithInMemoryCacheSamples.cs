using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Microsoft.Pfe.Xrm.Samples
{
    class ParallelRequestsWithInMemoryCacheSamples
    {
        public ParallelRequestsWithInMemoryCacheSamples(Uri serverUri, string username, string password)
        {
            this.Manager = new OrganizationServiceManager(serverUri, username, password, CacheStrategies.Memory);
        }

        /// <summary>
        /// Reusable instance of OrganizationServiceManager
        /// </summary>
        OrganizationServiceManager Manager { get; set; }

        private readonly TimeSpan _slidingExpiration = TimeSpan.FromMinutes(2);
        private readonly DateTime _absoluteExpirationUtc = DateTime.UtcNow.AddMinutes(10);


        /// <summary>
        /// Demonstrates parallelized submission of multiple create requests as a list
        /// </summary>
        /// <param name="targets">The collection of target entities to create in parallel</param>
        /// <returns>The provided list of target entities, hydrated with the generated unique identifiers</returns>
        public List<Entity> ParallelCreateWithEntityListSlidingCacheExpiration(List<Entity> targets)
        {
            try
            {
                targets = this.Manager.ParallelProxy.Create(targets, _slidingExpiration).ToList();
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }

            targets.ForEach(t =>
            {
                Console.WriteLine("Created {0} with id={1}", t.LogicalName, t.Id);
            });

            return targets;
        }

        /// <summary>
        /// Demonstrates a parallelized submission of multiple create requests with service proxy options
        /// 1. CallerId = The unique identifier of a systemuser being impersonated for the parallelized requests
        /// 2. ShouldEnableProxyTypes = Adds ProxyTypesBehavior to each OrganizationServiceProxy binding. Used for early-bound programming approach
        /// 3. Timeout = Increase the default 2 minute timeout on the channel to 5 minutes.
        /// </summary>
        /// <param name="targets">The list of target entities to create in parallel</param>
        /// <param name="callerId">The systemuser who should be impersonated for the parallelized requests</param>
        /// <returns>The collection of targets created with the assigned unique identifier</returns>
        public List<Entity> ParallelCreateWithOptionsAbsoluteCacheExpiration(List<Entity> targets, Guid callerId)
        {
            var options = new OrganizationServiceProxyOptions()
            {
                CallerId = callerId,
                ShouldEnableProxyTypes = true,
                Timeout = new TimeSpan(0, 5, 0)
            };

            try
            {
                targets = this.Manager.ParallelProxy.Create(targets, options, _absoluteExpirationUtc).ToList();
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }

            targets.ForEach(t =>
            {
                Console.WriteLine("Created {0} with id={1}", t.LogicalName, t.Id);
            });

            return targets;
        }

        /// <summary>
        /// Demonstrates a parallelized submission of multiple update requests
        /// </summary>
        /// <param name="targets">The list of target entities to update in parallel</param>
        public void ParallelUpdateSlidingCacheExpiration(List<Entity> targets)
        {
            try
            {
                this.Manager.ParallelProxy.Update(targets, _slidingExpiration);
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }
        }

        /// <summary>
        /// Demonstrates a parallelized submission of multiple update requests with the optional exception handler delegate
        /// </summary>
        /// <param name="targets">The list of target entities to update in parallel</param>
        /// <remarks>
        /// The exception handler delegate is provided the request type and the fault exception encountered. This delegate function is executed on the
        /// calling thread after all parallel operations are complete
        /// </remarks>
        public void ParallelUpdateWithExceptionHandlerAbsoluteCacheExpiration(List<Entity> targets)
        {
            int errorCount = 0;

            try
            {
                this.Manager.ParallelProxy.Update(targets, _absoluteExpirationUtc,
                    (target, ex) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Error encountered during update of entity with Id={0}: {1}", target.Id, ex.Detail.Message);
                        errorCount++;
                    });
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }

            Console.WriteLine("{0} errors encountered during parallel update.", errorCount);
        }

        /// <summary>
        /// Demonstrates a parallelized submission of multiple retrieve requests
        /// </summary>
        /// <param name="requests">The list of retrieve requests to submit in parallel</param>
        /// <returns>The list of retrieved entities</returns>
        public List<Entity> ParallelRetrieveAbsoluteCacheExpiration(List<RetrieveRequest> requests)
        {
            List<Entity> responses = null;

            try
            {
                responses = this.Manager.ParallelProxy.Retrieve(requests, _absoluteExpirationUtc).ToList();
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }

            responses.ForEach(r =>
            {
                Console.WriteLine("Retrieved {0} with id = {1}", r.LogicalName, r.Id);
            });

            return responses;
        }

        /// <summary>
        /// Demonstrates a parallelized submission of multiple retrieve requests with optional exception handler delegate
        /// </summary>
        /// <param name="requests">The list of retrieve requests to submit in parallel</param>
        /// <returns>The list of retrieved entities</returns>
        /// <remarks>
        /// The exception handler delegate is provided the request type and the fault exception encountered. This delegate function is executed on the
        /// calling thread after all parallel operations are complete
        /// </remarks>
        public List<Entity> ParallelRetrieveWithExceptionHandlerSlidingCacheExpiration(List<RetrieveRequest> requests)
        {
            int errorCount = 0;
            List<Entity> responses = null;

            try
            {
                responses = this.Manager.ParallelProxy.Retrieve(requests, _slidingExpiration,
                    (request, ex) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Error encountered during retrieve of entity with Id={0}: {1}", request.Target.Id, ex.Detail.Message);
                        errorCount++;
                    }).ToList();
            }
            catch (AggregateException ae)
            {
                // Handle exceptions
            }

            Console.WriteLine("{0} errors encountered during parallel retrieves.", errorCount);

            return responses;
        }
    }
}