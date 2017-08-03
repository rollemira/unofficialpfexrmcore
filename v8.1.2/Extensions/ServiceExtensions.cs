/*================================================================================================================================

  This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.  

  THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, 
  INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  

  We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object 
  code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software 
  product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the 
  Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims 
  or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.

 =================================================================================================================================*/

namespace Microsoft.Pfe.Xrm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using System.Xml.Linq;

    public static class DiscoveryServiceExtensions
    {
        /// <summary>
        /// Helper method for assigning common proxy-specific settings such as channel timeout
        /// </summary>
        /// <param name="proxy">The service proxy</param>
        /// <param name="options">The options to configure on the service proxy</param>
        public static void SetProxyOptions(this DiscoveryServiceProxy proxy, DiscoveryServiceProxyOptions options)
        {
            if (!options.Timeout.Equals(TimeSpan.Zero)
                && options.Timeout > TimeSpan.Zero)
                proxy.Timeout = options.Timeout;
        }
    }

    public static class OrganizationServiceExtensions
    {
        private static ServiceManagerCache Cache => _cache ?? (_cache = new ServiceManagerCache(CacheStrategies.None));
        private static ServiceManagerCache _cache;

        /// <summary>
        /// Helper method for assigning common proxy-specific settings for impersonation, early-bound types, and channel timeout
        /// </summary>
        /// <param name="proxy">The service proxy</param>
        /// <param name="options">The options to configure on the service proxy</param>
        public static void SetProxyOptions(this OrganizationServiceProxy proxy, OrganizationServiceProxyOptions options)
        {
            SetProxyOptions(proxy, options, null);
        }

        /// <summary>
        ///     Helper method for assigning common proxy-specific settings for impersonation, early-bound types, and channel
        ///     timeout
        /// </summary>
        /// <param name="proxy">The service proxy</param>
        /// <param name="options">The options to configure on the service proxy</param>
        /// <param name="cache">The cache to be used (if any)</param>
        internal static void SetProxyOptions(this OrganizationServiceProxy proxy, OrganizationServiceProxyOptions options, ServiceManagerCache cache = null)
        {
            if (!options.CallerId.Equals(Guid.Empty)) proxy.CallerId = options.CallerId;

            if (options.ShouldEnableProxyTypes) proxy.EnableProxyTypes();

            if (options.ShouldEnableProxyTypes)
                proxy.EnableProxyTypes();
            
            if (!options.Timeout.Equals(TimeSpan.Zero)
                && options.Timeout > TimeSpan.Zero)
                proxy.Timeout = options.Timeout;

            _cache = cache;
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// If retrieving all pages, max result count is essentially unbound - Int64.MaxValue: 9,223,372,036,854,775,807
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, shouldRetrieveAllPages, Int64.MaxValue, null, null, null, pagedOperation);
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// If retrieving all pages, max result count is essentially unbound - Int64.MaxValue: 9,223,372,036,854,775,807
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, bool suppressCache, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, shouldRetrieveAllPages, Int64.MaxValue, null, null, suppressCache, pagedOperation);
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// If retrieving all pages, max result count is essentially unbound - Int64.MaxValue: 9,223,372,036,854,775,807
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, TimeSpan? slidingExpiration = null, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, shouldRetrieveAllPages, Int64.MaxValue, slidingExpiration, null, null, pagedOperation);
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// If retrieving all pages, max result count is essentially unbound - Int64.MaxValue: 9,223,372,036,854,775,807
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, DateTime? absoluteExpirationUtc = null, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, shouldRetrieveAllPages, Int64.MaxValue, null, absoluteExpirationUtc, null, pagedOperation);
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// If retrieving all pages, max result count is essentially unbound - Int64.MaxValue: 9,223,372,036,854,775,807
        /// </remarks>
        internal static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, TimeSpan? slidingExpiration = null, DateTime? absoluteExpirationUtc = null, bool? suppressCache = null, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, shouldRetrieveAllPages, Int64.MaxValue, slidingExpiration, absoluteExpirationUtc, suppressCache, pagedOperation);
        }

        /// <summary>
        /// Perform an iterative series of RetrieveMultiple requests in order to obtain all results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="maxResultCount">An upper limit on the maximum number of entity records that should be retrieved as the query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/></param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// Inherently retrieves all pages up to the max result count.  If max result count is less than initial page size, then page size is adjusted down to honor the max result count
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, int maxResultCount, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, true, maxResultCount, null, null, null, pagedOperation);
        }

        /// <summary>
        /// Perform an iterative series of RetrieveMultiple requests in order to obtain all results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="maxResultCount">An upper limit on the maximum number of entity records that should be retrieved as the query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/></param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// Inherently retrieves all pages up to the max result count.  If max result count is less than initial page size, then page size is adjusted down to honor the max result count
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, int maxResultCount, bool suppressCache, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, true, maxResultCount, null, null, suppressCache, pagedOperation);
        }

        /// <summary>
        /// Perform an iterative series of RetrieveMultiple requests in order to obtain all results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="maxResultCount">An upper limit on the maximum number of entity records that should be retrieved as the query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/></param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// Inherently retrieves all pages up to the max result count.  If max result count is less than initial page size, then page size is adjusted down to honor the max result count
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, int maxResultCount, TimeSpan? slidingExpiration = null, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, true, maxResultCount, slidingExpiration, null, null, pagedOperation);
        }

        /// <summary>
        /// Perform an iterative series of RetrieveMultiple requests in order to obtain all results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="maxResultCount">An upper limit on the maximum number of entity records that should be retrieved as the query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/></param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// 
        /// Inherently retrieves all pages up to the max result count.  If max result count is less than initial page size, then page size is adjusted down to honor the max result count
        /// </remarks>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, int maxResultCount, DateTime? absoluteExpirationUtc = null, Action<EntityCollection> pagedOperation = null)
        {
            return service.RetrieveMultiple(query, true, maxResultCount, null, absoluteExpirationUtc, null, pagedOperation);
        }

        /// <summary>
        /// Performs an iterative series of RetrieveMultiple requests in order to obtain all pages of results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="maxResultCount">An upper limit on the maximum number of entity records that should be retrieved as the query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/></param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g. MoreRecords, PagingCookie, etc.)</returns>
        /// <remarks>
        /// CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing subsequent 
        /// query requests so that all results can be retrieved.
        /// </remarks>
        private static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryBase query, bool shouldRetrieveAllPages, 
            long maxResultCount, TimeSpan? slidingExpiration = null, DateTime? absoluteExpirationUtc = null, bool? suppressCache = null,
            Action<EntityCollection> pagedOperation = null)
        {
            if (query == null)
                throw new ArgumentNullException("query", "Must supply a query for the RetrieveMultiple request");
            if (maxResultCount <= 0)
                throw new ArgumentException("maxResultCount", "Max entity result count must be a value greater than zero.");

            var qe = query as QueryExpression;

            if (qe != null)
            {
                return service.RetrieveMultiple(qe, shouldRetrieveAllPages, maxResultCount, slidingExpiration, absoluteExpirationUtc, suppressCache, pagedOperation);
            }
            else
            {
                var fe = query as FetchExpression;

                if (fe != null)
                {
                    return service.RetrieveMultiple(fe, shouldRetrieveAllPages, maxResultCount, slidingExpiration, absoluteExpirationUtc, suppressCache, pagedOperation);
                }
            }

            throw new ArgumentException("This method only handles FetchExpression and QueryExpression types.", "query");
        }

        /// <summary>
        ///     Performs an iterative series of RetrieveMultiple requests using QueryExpression in order to obtain all pages of
        ///     results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="query">The QueryExpression query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="maxResultCount">
        ///     An upper limit on the maximum number of entity records that should be retrieved as the
        ///     query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/>
        /// </param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>
        ///     An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g.
        ///     MoreRecords, PagingCookie, etc.)
        /// </returns>
        /// <remarks>
        ///     CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing
        ///     subsequent
        ///     query requests so that all results can be retrieved.
        /// </remarks>
        private static EntityCollection RetrieveMultiple(this IOrganizationService service, QueryExpression query,
            bool shouldRetrieveAllPages, long maxResultCount, TimeSpan? slidingExpiration = null, DateTime? absoluteExpirationUtc = null,
            bool? suppressCache = null, Action<EntityCollection> pagedOperation = null)
        {
            // Establish page info (only if TopCount not specified)
            if (query.TopCount == null)
            {
                if (query.PageInfo == null)
                {
                    // Default to first page
                    query.PageInfo = new PagingInfo
                    {
                        Count = QueryExtensions.DefaultPageSize,
                        PageNumber = 1,
                        PagingCookie = null,
                        ReturnTotalRecordCount = false
                    };
                }
                else if (query.PageInfo.PageNumber <= 1 || query.PageInfo.PagingCookie == null)
                {
                    // Reset to first page
                    query.PageInfo.PageNumber = 1;
                    query.PageInfo.PagingCookie = null;
                }

                // Limit initial page size to max result if less than current page size. No risk of conversion overflow.
                if (query.PageInfo.Count > maxResultCount) query.PageInfo.Count = Convert.ToInt32(maxResultCount);
            }

            // Track local long value to avoid expensive IEnumerable<T>.LongCount() method calls
            long totalResultCount = 0;
            var allResults = new EntityCollection();

            while (true)
            {
                EntityCollection page;
                //avoid thread collision
                lock (ThreadSafety.SyncRoot)
                {
                    var cacheDisabled = suppressCache ?? false;
                    //generate cache key
                    var cacheKey = CacheKeys.QueryExpressionFormat.Fmt(query.ToJsonString());

                    //can we get it from cache?
                    if (!cacheDisabled && Cache.Exists(cacheKey))
                    {
                        //yes
                        page = Cache.Get<EntityCollection>(cacheKey);
                    }
                    else
                    {
                        //no, retrieve and add it to cache
                        page = service.RetrieveMultiple(query);
                        if (!cacheDisabled)
                            Cache.Add(page, cacheKey, slidingExpiration, absoluteExpirationUtc);
                    }
                }

                // Capture the page
                if (totalResultCount == 0) allResults = page;
                else allResults.Entities.AddRange(page.Entities);

                // Invoke the paged operation if non-null
                pagedOperation?.Invoke(page);

                // Update the count of pages retrieved and processed
                totalResultCount = totalResultCount + page.Entities.Count;

                // Determine if we should retrieve the next page
                if (shouldRetrieveAllPages && totalResultCount < maxResultCount && page.MoreRecords)
                {
                    // Setup for next page
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = page.PagingCookie;

                    var remainder = maxResultCount - totalResultCount;

                    // If max result count is not divisible by page size, then final page may be less than the current page size and should be sized to remainder. 
                    // No risk of coversion overflow.
                    if (query.PageInfo.Count > remainder) query.PageInfo.Count = Convert.ToInt32(remainder);
                }
                else
                {
                    allResults.CopyFrom(page);
                    break;
                }
            }

            return allResults;
        }

        /// <summary>
        ///     Performs an iterative series of RetrieveMultiple requests using FetchExpression in order to obtain all pages of
        ///     results up to the provided maximum result count
        /// </summary>
        /// <param name="service">The current IOrganizationService instance</param>
        /// <param name="fetch">The FetchExpression query to be executed</param>
        /// <param name="shouldRetrieveAllPages">True = perform iterative paged query requests, otherwise return first page only</param>
        /// <param name="maxResultCount">
        ///     An upper limit on the maximum number of entity records that should be retrieved as the
        ///     query results - useful when the total size of result set is unknown and size may cause <see cref="OutOfMemoryException"/>
        /// </param>
        /// <param name="slidingExpiration">The cache sliding expiration (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="absoluteExpirationUtc">The cache absolute expiration in UTC (If an ICachesStragegy Chosen <see cref="CacheStrategies"/>)</param>
        /// <param name="suppressCache">Suppresses any cache lookup or insertion</param>
        /// <param name="pagedOperation">An operation to perform on each page of results as it's retrieved</param>
        /// <returns>
        ///     An EntityCollection containing the results of the query. Details reflect the last page retrieved (e.g.
        ///     MoreRecords, PagingCookie, etc.)
        /// </returns>
        /// <remarks>
        ///     CRM limits query response to paged result sets of 5,000. This method encapsulates the logic for performing
        ///     subsequent
        ///     query requests so that all results can be retrieved.
        /// </remarks>
        private static EntityCollection RetrieveMultiple(this IOrganizationService service, FetchExpression fetch,
            bool shouldRetrieveAllPages, long maxResultCount, TimeSpan? slidingExpiration = null, DateTime? absoluteExpirationUtc = null,
            bool? suppressCache = null, Action < EntityCollection> pagedOperation = null)
        {
            var fetchXml = fetch.ToXml();
            var pageNumber = fetchXml.GetFetchXmlPageNumber();
            var pageCookie = fetchXml.GetFetchXmlPageCookie();
            var pageSize = fetchXml.GetFetchXmlPageSize(QueryExtensions.DefaultPageSize);

            // Establish the first page based on lesser of initial/default page size or max result count (will be skipped if top count > 0)
            if (pageSize > maxResultCount) pageSize = Convert.ToInt32(maxResultCount);

            if (pageNumber <= 1 || string.IsNullOrWhiteSpace(pageCookie)) fetchXml.SetFetchXmlPage(null, 1, pageSize);
            else fetchXml.SetFetchXmlPage(pageCookie, pageNumber, pageSize);

            fetch.Query = fetchXml.ToString();

            // Track local long value to avoid expensive IEnumerable<T>.LongCount() method calls
            long totalResultCount = 0;
            var allResults = new EntityCollection();

            while (true)
            {
                EntityCollection page;
                //avoid thread collision
                lock (ThreadSafety.SyncRoot)
                {
                    var cacheDisabled = suppressCache ?? false;
                    //generate cache key
                    var cacheKey = CacheKeys.FetchXmlFormat.Fmt(fetchXml);

                    //can we get it from cache?
                    if (!cacheDisabled && Cache.Exists(cacheKey))
                    {
                        //yes
                        page = Cache.Get<EntityCollection>(cacheKey);
                    }
                    else
                    {
                        //no, retrieve and add it to cache
                        page = service.RetrieveMultiple(fetch);
                        if (!cacheDisabled)
                            Cache.Add(page, cacheKey, slidingExpiration, absoluteExpirationUtc);
                    }
                }

                // Capture the page
                if (totalResultCount == 0) allResults = page;
                else allResults.Entities.AddRange(page.Entities);

                // Invoke the paged operation if non-null
                pagedOperation?.Invoke(page);

                // Update the count of pages retrieved and processed
                totalResultCount = totalResultCount + page.Entities.Count;

                // Determine if we should retrieve the next page
                if (shouldRetrieveAllPages && totalResultCount < maxResultCount && page.MoreRecords)
                {
                    // Setup for next page
                    pageNumber++;

                    var remainder = maxResultCount - totalResultCount;

                    // If max result count is not divisible by page size, then final page may be less than the current page size and should be sized to remainder. 
                    // No risk of coversion overflow.
                    if (pageSize > remainder) pageSize = Convert.ToInt32(remainder);

                    fetch.SetPage(page.PagingCookie, pageNumber, pageSize);
                }
                else
                {
                    allResults.CopyFrom(page);
                    break;
                }
            }

            return allResults;
        }
    }
}
