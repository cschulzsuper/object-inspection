# Paula

* Tom: _"Who is Paula?"_
* Peter: _"Paula is an experimental business application."_
* Tom: _"Like a playground?"_
* Peter: _"Exactly!"_

![Introduction](/docs/bucket/42e8bf7e-3b56-475b-a9d6-d6773c822326.gif)

## Getting Started

A couple of things need to be setup, if you want to start Paula yourself.

* Install the Azure Cosmos DB emulator

* Create a `usersecrets.json` for the `data/Migrator` project.

```json
{
  "CosmosEndpoint": "<URI of the Azure Cosmos DB emulator>",
  "CosmosKey": "<Primary Key of the Azure Cosmos DB emulator>",
  "CosmosDatabase": "<Name of the Azure Cosmos DB Database>",
}
```

* Run `data/Migrator` to initialize the database.

* Create a `usersecrets.json` for the `server/ServerApp` project.

```json
{
  "CosmosEndpoint": "<URI of the Azure Cosmos DB emulator>",
  "CosmosKey": "<Primary Key of the Azure Cosmos DB emulator>",
  "CosmosDatabase": "<Name of the Azure Cosmos DB Database>",
  "Maintainer": "<The user that will be the admin>",
  "MaintainerOrganization": "<The organization of the admin user>"
}
```
* Set `server/ServerApp` and `blazor/BlazorServerSideHost` as multiple start projects.
* Run the application (https://localhost:5002) and register a new organization for your maintainer.

## Currenty Status

I'm tracking the progress of the application on the project board.

* [Project Board @ Trello](https://trello.com/b/xUlXP4Rm/paula)
