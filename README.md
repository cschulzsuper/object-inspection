# Paula

## Getting Started

A couple of things need to be setup, if you want to start Paula yourself.

* Install the Azure Cosmos DB emulator
* Create a `usersecrets.json` for the `Super.Paula.Web` project.

```json
{
  "CosmosEndpoint": "<URI of the Azure Cosmos DB emulator>",
  "CosmosKey": "<Primary Key of the Azure Cosmos DB emulator>",
  "Maintainer": "<The user that will be the admin>",
  "MaintainerOrganization": "<The organization of the admin user>"
}
```
* Use `Super.Paula.Web` and `Super.Paula.BlazorServerSideHost` as start project
* Run the application and register a new organization for your maintainer.
