# Paula

* 19 Sep 2021
  * Tom: _"Who is Paula?"_
  * Peter: _"Paula is an experimental business application."_
  * Tom: _"Like a playground?"_
  * Peter: _"Exactly!"_

* 20 Feb 2022
  * Tom: _"Do you still play ín your playground?"_
  * Peter: _"Yes, but the `README.md` might be out of date?"_
  * Tom: _"Mh, but can I have some information about the progress?"_
  * Peter: _"Sure, just have a look in the [project board](https://trello.com/b/xUlXP4Rm/paula) on trello?"_

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
  "MaintainerIdentity": "<The identity that will be the admin>",
  "Server": "https://localhost:5001",
  "Client": "https://localhost:5002",
}
```

* Set `server/ServerApp` and `blazor/BlazorServerSideHost` as multiple start projects.

* Run the application. 
  * `https://localhost:5002`
* Create the first identity. 
  * `https://localhost:5002/sign-up`
* Create the first organization. 
  * `https://localhost:5002/register-organiztation`
* Create the chief inspector of the first organization. 
  * `https://localhost:5002/register-chief-inspector/{organization}`

## Adventure Tours

Instead of the `data/Migrator` you might want to look at the `templates/AdventureTours` project. 
It resets and initializes the database with a set of example data.

This initialization uses [playwright](https://github.com/microsoft/playwright) to create the example data.

A more elaborate explanation will follow soon. 


## Currenty Status

I'm tracking the progress of the application on the [project board](https://trello.com/b/xUlXP4Rm/paula).
