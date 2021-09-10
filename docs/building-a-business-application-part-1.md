# Building a Business Application, Part 1
## Hey.

In the last couple of weeks I have been creating a small experimental business application based on [.NET 6](https://dotnet.microsoft.com/).
It uses [Azure Cosmos DB](https://azure.microsoft.com/de-de/services/cosmos-db/), [EF Core](https://github.com/dotnet/efcore), 
[ASP.NET Core](https://github.com/dotnet/aspnetcore) and [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor). 

The idea is that inspections can be attached to business objects. The set of business object inspections can be audited. The auditing results are stored in a history. I’m not sure if I use the right wording here. I'm a developer not a PO.

Just imagine that you have an employee who needs to check certain matters on a regular basis. As I seldom have original business ideas myself, this example is inspired by the stuff I do at work. I'm just like a carpenter who is building his own cupboard.

You can have a look at the [repository](https://github.com/cschulzsuper/paula) if you are interested in the source code.

Be warned, the rest of this post gets technical.

## What's in the Box.

At the beginning I used [.NET 6 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-6/) but I regularly upgrade to newer versions as they become available.

In the front end I use Blazor which once and for all allows me to ignore all of my JavaScript skills. 
The basic layout comes from the default Blazor template. It includes [Bootstrap](https://getbootstrap.com/) and  [Open Iconic](https://useiconic.com/open). 
The application supports authentication, registration, basic administration and the actual business features.

The backend is an ASP.NET Core API with a repository pattern implementation and EF Core in the data layer. The actual data source itself is an Azure Cosmos DB emulator.

## Into the Code.

I’m not yet a fan of the new top level statements, which are coming as part of .NET 6, so I ignored them completely.
I also kept using the layout of the old startup class pattern. 

But I use some capabilities of new Minimal API to avoid ASP.NET Core API Controllers. 
Instead I have written something I call handler classes. The methods of those handlers are mapped to endpoints.
This approach gives me freedom and much less dependencies to ASP.NET Core itself. The functionality behind Minimal API was something I was looking for in previous version of ASP.NET Core, but only with .NET 6 it is comfortable thing to use.

``` csharp
public static IEndpointRouteBuilder MapBusinessObject(
    this IEndpointRouteBuilder endpoints)
{
    // MapCollection is an extension method I wrote,
    // it wraps some Minimal API features for REST like APIs.

    endpoints.MapCollection(
       "/business-objects",
       "/business-objects/{businessObject}",
       Get,
       GetAll,
       Create,
       Replace,
       Delete);
}
```

All back end handler classes have a client side counterpart. This allows me to use the same request and response DTOs. The client handler sends a request object via HTTP. The back end handler receives and processes it. Easy peasy.

The request classes have some data annotations that are evaluated with Blazor's `DataAnnotationsValidator`.

In the back end handler the request is mapped to an entity class that goes into a manager. The manager validates the incoming object once again before a repository will send the data via the DbContext into the cosmos.

``` csharp
// I'm still evaluating wheter this validation pattern is good or bad

private void EnsureInsertable(Organization organization)
    => Validator.Ensure(
        OrganizationValidator.UniqueNameHasValue(organization),
        OrganizationValidator.UniqueNameHasKebabCase(organization),
        OrganizationValidator.UniqueNameIsUnqiue(organization,GetQueryable()),
        OrganizationValidator.ChiefInspectorIsNotNull(organization),
        OrganizationValidator.ChiefInspectorHasKebabCase(organization));
```

This was the first time I used Azure Cosmos DB. After some misconception on how a document database works, I think I got the gist of it and was able to implement nearly everything I wanted with the EF Core Cosmos DB provider. The EF Core team really knows what they are doing.

Authentication and authorization is also implemented. It is only a custom pseudo token based authentication, because I did not want to add a complete identity framework. Although minimalistic the authentication supports impersonation.

Overall, this was really fun. After understanding all the basic concepts most of the stuff just worked seamlessly together. 

I'm not a writer, but I wanted to share this.

See you.
