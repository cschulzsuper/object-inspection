# Building a Business Application, Part 2

![Title](bucket/afd82686-fd32-4d1b-9ba5-237090fa702e.png)

A couple of weeks ago my PO asked me if I had any experience with SignalR. I had to disappoint him, as none of my private projects nor any of our company products included real time communication.

Since I’m still experimenting with my experimental business application, I thought this would be a cool learning experience and entry point for a new feature.

The simplest use case I could think of was a notification for an inspector when he is assigned to a business object. The endgame should be extensibility to all kinds of notifications.

I will not go into the specifics on how to get started with SignalR. The only thing that matters right now is that SignalR uses a WebSocket to communicate between client and server. The most common introductory example is normally a chat or messaging app. This is far more than what I wanted. For my use case a push from the server to the client is sufficient.

The rest of this article will become very technical. It contains some code snippets, but does also omit a lot, as this is only learning experience and not a tutorial.

## Into the Code (back end).

After understanding how SignalR works and after the decision how I would integrate it into my backend, the implementation was kind of easy.

I wrote a new notification handler with two delegate registration method. 

* The handler of `OnCreatedAsync` is triggered when a notification is created. 
* The handler of `OnDeletedAsync` is triggered when a notification is deleted.

![Contract](bucket/a63dbc3b-6b31-46fa-8aca-798d92083493.png)

These are called by a factory which is used by the DI framework during the service creation.

![Contract](bucket/5e93f27c-ca20-46f9-978a-18e10c02bc1c.png)

The actions given into `OnCreatedAsync` and `OnDeletedAsync` will just call the SignalR `IHubContext` and send a message to the client that needs to be informed. It is encapsulated in what I call a messenger class.

![Contract](bucket/260c1362-dbf7-4062-af85-dc085e4d6c0c.png)

The nice thing is that SignalR handles user tracking automatically. You only need to make sure that the `NameIdentifier` `Claim` is set during the authentication. I already did that in my pseudo toke based authentication.

The SignalR `Hub` itself contains nothing as I don‘t receive messages from the client.

## Into the Code (client side).

The client side uses the same interface for the client handler, which means I have `OnCreatedAsync` and `OnDeletedAsync` on the client side as well. This is perfect, as I can uses them to update the GUI. The implementation of `OnCreatedAsync` just registers the action in the SignalR HubConnection.

![Contract](bucket/edcdb0f1-708e-4bba-a0d2-428b5d996eb3.png)
![Contract](bucket/7623ad5a-262d-44dc-b0d9-bd99d9d51e11.png)

And that’s it. I added some more infrastructure code and did some caching, but that is just noise around the basic implementation.

Once everything was set up, and I got my first notification I thought that the whole thing is to easy. Sure all of this is experimental, but in my opinion I‘m still free of hacks and spaghetti code.

Thanks for reading.

Enjoy. 
