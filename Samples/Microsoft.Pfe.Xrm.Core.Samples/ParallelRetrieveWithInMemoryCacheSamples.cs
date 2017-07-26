using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Microsoft.Pfe.Xrm.Samples
{
    class ParallelRetrieveWithInMemoryCacheSamples
    {
        public ParallelRetrieveWithInMemoryCacheSamples(Uri serverUri, string username, string password)
        {
            /**
             In a web scenario, it'd be best to use CacheStrategies.Web
             */
            this.Manager = new OrganizationServiceManager(serverUri, username, password, CacheStrategies.Memory);
        }

        /// <summary>
        /// Reusable instance of OrganizationServiceManager
        /// </summary>
        OrganizationServiceManager Manager { get; set; }

        private readonly TimeSpan _slidingExpiration = TimeSpan.FromMinutes(2);
        private readonly DateTime _absoluteExpirationUtc = DateTime.UtcNow.AddMinutes(10);

        /// <summary>
        /// Demonstrates a parallelized submission of multiple retrieve requests
        /// </summary>
        /// <param name="requests">The list of retrieve requests to submit in parallel</param>
        /// <returns>The list of retrieved entities</returns>
        public List<Entity> ParallelRetrieveWithSlidingCache(List<RetrieveRequest> requests)
        {
            List<Entity> responses = null;

            try
            {
                responses = this.Manager.ParallelProxy.Retrieve(requests, _slidingExpiration).ToList();
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
        public List<Entity> ParallelRetrieveSlidingCacheAndExceptionHandler(List<RetrieveRequest> requests)
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

        /// <summary>
        /// Demonstrates a parallelized submission of multiple retrieve requests
        /// </summary>
        /// <param name="requests">The list of retrieve requests to submit in parallel</param>
        /// <returns>The list of retrieved entities</returns>
        public List<Entity> ParallelRetrieveWithAbsoluteCache(List<RetrieveRequest> requests)
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
        public List<Entity> ParallelRetrieveAbsoluteCacheAndExceptionHandler(List<RetrieveRequest> requests)
        {
            int errorCount = 0;
            List<Entity> responses = null;

            try
            {
                responses = this.Manager.ParallelProxy.Retrieve(requests, _absoluteExpirationUtc,
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