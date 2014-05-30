EntityFramework.Filters
=======================

Filters implementation for Entity Framework, on NuGet as the [EntityFramework.Filters](https://www.nuget.org/packages/EntityFramework.Filters/) package.

Filters allow you to define a parameterized filter at configuration time. At runtime, you turn on the filter and apply parameters, and every query for that entity will include the filter.

Configuration
-----------------------
The FilterInterceptor must be registered with Entity Framework, either through a DbConfiguration class:
```chsarp
public class ExampleConfiguration : DbConfiguration
{
    public ExampleConfiguration()
    {
        AddInterceptor(new FilterInterceptor());
    }
}
```
Or through the OnModelCreating method:
```csharp
protected override void OnModelCreating(DbModelBuilder modelBuilder)
{
    DbInterception.Add(new FilterInterceptor());
}
```

Examples
-----------------------

Filters are first defined, then configured. You define the filter against a single entity:

```
modelBuilder.Entity<Listing>()
    .Filter("ActiveListings", c => c.Condition<ListingStatus>(
        listing => listing.Status == ListingStatus.Active));
```

Or against a set of entities that match a type (interface or base class):
        
```
modelBuilder.Conventions.Add(
    FilterConvention.Create<IAgencyEntity, int>("Agency", (e, agencyId) => e.AgencyId == agencyId);
```

Filters are then enabled and parameter values filled in on a DbContext basis:

```
dbContext.EnableFilter("ActiveListings");
dbContext.EnableFilter("Agency")
    .SetParameter("agencyId", _userContext.CurrentUser.AgencyId);
```

Filters are disabled by default, and you can disable them selectively after enabling:

```
dbContext.DisableFilter("ActiveListings");
```

The filter names must be unique, and filter parameter names are matched by the parameter name you supply to the filter definition's expression.

Common Usages
----------------------------

Filters are used to define a predicate that will be applied to every entity in a DbContext, without a developer needing to remember to include it for every query. Common applications include:

* Security
* Multi-tenancy
* Logical data partitioning
* Soft deletes
* Active/inactive records

There are some limitations, however:

* No access to context for complex joins
* Collection properties not available
