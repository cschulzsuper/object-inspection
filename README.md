# Object Inspection

* 19 Sep 2021
  * Tom: _"Who is Paula?"_
  * Peter: _"Paula is an experimental business application."_
  * Tom: _"Like a playground?"_
  * Peter: _"Exactly!"_

* 20 Feb 2022
  * Tom: _"Do you still play ín your playground?"_
  * Peter: _"Yes, but the `README.md` might be out of date?"_
  * Tom: _"Mh, but can I have some information about the progress?"_
  * Peter: _"Sure, just have a look in the [project board](https://trello.com/b/xUlXP4Rm) on trello?"_

 * 21 Sep 2022
  * Tom: _"Hey, what is up with Paula?"_
  * Peter: _"She is taking shape, but I changed the name to Object Inspection?"_
  * Tom: _"That sounds boring."_
  * Peter: _"Yes, but way more professional!"_

![Introduction](/docs/bucket/56f32e7d-0648-4b75-a035-e6c56da6662f.gif)

## Getting Started

A couple of things need to be setup, if you want to start the Object Inspection yourself.

* Install the Azure Cosmos DB emulator

* Create a `usersecrets.json` for the `data/Migrator` project.

```json
{
	"ObjectInspection": {
        "CosmosEndpoint": "<URI of the Azure Cosmos DB emulator>",
        "CosmosKey": "<Primary Key of the Azure Cosmos DB emulator>",
        "CosmosDatabase": "<Name of the Azure Cosmos DB Database>"
	}
}
```

* Run `data/Migrator` to initialize the database.

* Create a `usersecrets.json` for the `server/ServerApp` project.

```json
{
    "ObjectInspection": {
        "CosmosEndpoint": "<URI of the Azure Cosmos DB emulator>",
        "CosmosKey": "<Primary Key of the Azure Cosmos DB emulator>",
        "CosmosDatabase": "<Name of the Azure Cosmos DB Database>",
        "BlobStorageConnectionString": "<Connection String of the Azure Blob Storage>",
        "MaintainerIdentity": "<The identity that will be the admin>",
        "Server": "https://localhost:5001",
        "Client": "https://localhost:5002"
    }
}
```

* Set `server/ServerApp` and `blazor/BlazorServerSideHost` as multiple start projects.

* Run the application. 
  * `https://localhost:5002`
* Create the first identity. 
  * `https://localhost:5002/sign-up`
* Create the first organization. 
  * `https://localhost:5002/organizations/register`
* Create the chief inspector of the first organization. 
  * `https://localhost:5002/organizations/{organization}/register`

## Adventure Tours

Additionally to the `data/Migrator` you might want to look at `templates/AdventureTours` and `templates/AdventureToursAuditing`. 
* `templates/AdventureTours` initializes the database with an example.
* `templates/AdventureToursAuditing` executes an auditing on the example.

Both projects use [playwright](https://github.com/microsoft/playwright).

A more elaborate explanation will follow at a later date.

## Currenty Status

I'm tracking the progress of the application on the [project board](https://trello.com/b/xUlXP4Rm).
