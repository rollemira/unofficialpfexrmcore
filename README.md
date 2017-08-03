# Warning!
**This is not the official repository of the xrmpfecore library.**

**The OFFICIAL soruce can be found here:
[https://pfexrmcore.codeplex.com/](https://pfexrmcore.codeplex.com/)**

**And the OFFICIAL NuGet packages here:
[https://www.nuget.org/packages/Microsoft.Pfe.Xrm.CoreV8/](https://www.nuget.org/packages/Microsoft.Pfe.Xrm.CoreV8/)**

## PFE Xrm Core Library... Now With Caching! (8.1.2)

This flavor of the library implements a layer of caching onto the parallelism that this library provides. All of the prior overloads of the methods still exist so there shouldn't be any breaking changes in here. All caching is opt-in and nothing is cached by default. The default cache strategy is **None**. 

**Be careful!** One of the potential risks of this caching layer is that you may get **some data** from cache and the rest from the server. This approach is for a use case in which potentially stale data is ok. Although there are overloads that let you suppress reads from the cache entirely.

### Examples

For all of these examples, I'm going to use a **TimeSpan** for sliding cache expiration. There are also overloads that accept a **DateTime** for absolute cache expiration.

Tell the OrganizationServiceManager which cache strategy to use:
```CSharp
// Web cache strategy
var serviceManager = new OrganizationServiceManager(serverUri, username, password, CacheStrategies.Web);
// Memory cache strategy
var serviceManager = new OrganizationServiceManager(serverUri, username, password, CacheStrategies.Memory);
// No cache strategy - disable cache (implicitly)
var serviceManager = new OrganizationServiceManager(serverUri, username, password);
// No cache strategy - disable cache (explicitly)
var serviceManager = new OrganizationServiceManager(serverUri, username, password, CacheStrategies.None);
```

Create items and store them in cache:
```CSharp
List<Entity> itemsToCreate = ...
var entities = serviceManager.ParallelProxy.Create(itemsToCreate, TimeSpan.FromMinutes(5));
```

Retrieve (out of cache if possible):
```CSharp
List<RetrieveRequest> requests = ...
var entities = serviceManager.ParallelProxy.Retrieve(requests, TimeSpan.FromMinutes(5));
entities.ForEach(r =>
{
    Console.WriteLine("Retrieved {0} with id = {1}", r.LogicalName, r.Id);
});
```

Optionally suppress cache lookup and insertion during Retrieve:
```CSharp
var results = serviceManager.ParallelProxy.Retrieve(requests, true);
```

Update items (also updates cached version):
```CSharp
serviceManager.ParallelProxy.Update(entities, TimeSpan.FromMinutes(5));
```

RetrieveMultiple (out of cache if possible):
```CSharp
IDictionary<string, QueryBase> queries = ...
var results = serviceManager.ParallelProxy.RetrieveMultiple(queries, true, TimeSpan.FromMinutes(5));
foreach (var result in results)
{
    Console.WriteLine("Query with key={0} for {1} returned {2} records.",
        result.Key,
        result.Value.EntityName,
        result.Value.Entities.Count);
}
```

Optionally suppress cache lookup and insertion during RetrieveMultiple:
```CSharp
var results = serviceManager.ParallelProxy.RetrieveMultiple(queries, true, true);
```

### More Examples
You can find samples in [this project of this repository](https://github.com/rollemira/unofficialpfexrmcore/tree/master/Samples/Microsoft.Pfe.Xrm.Core.Samples). Look for the C# files ending "WithInMemoryCacheSamples".