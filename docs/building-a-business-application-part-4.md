# Building a Business Application, Part 4

![Title](bucket/4b18fa16-2ba3-4572-90ee-a376efdaff08.png)

Today, I want to talk about validating front end inputs and back end data.

This article will paint a rough picture on the error handling and validation in my experimental prototype. It does not aspire to be perfect nor does it claim to be complete or the best approach. If you can agree with those terms, you may continue.

Until now I did not really mention validation and error handling in detail, as I started with my experimental business application as a prototype without any of that stuff. You could use any format and any data an the values would end up in the database. If data constrain would be violated, an exception would be thrown somewhere in the request processing stack and might even break the Blazor application.

![Application](bucket/081b4094-1167-4043-a8c1-8bb309966644.gif)

But I had in mind that I would later add validation at multiple stages. This was a little test an my implementation skills. *Am I able to add validation in a subsequent development stage, without much refactoring?*

I like to think of it as a single responsibility principle for myself. A couple of years ago I would respect all eventualities in the first step of my implementation, as I wanted to cover all failure cases from the beginning. This would usually postpone my prototype and would even make my code ugly. Fast and overthinking usually means not good in my case.

If I focus on the essentials, the essentials become better. When the essentials are complete, I start to think about other aspects. Two of these other aspects are validation and error handling.

The following aspects needed to be respected:

* Validate input data on the client side
* Validate input data on the server side
* Handle errors
* Display errors and validation messages

The following implementation can only be seen as its own first prototype of validation. It can still be improved, but that comes with refactoring in the future.

# Clint side validation

Blazor supports data annotations on properties.

![DataAnnotations](bucket/f331062c-7af0-4238-ab4e-56109ee03bf4.png)

If properties of DTOs are annotated with validation attributes and the *DataAnnotationsValidator* is added to the *EditForm*, a simple validation response for the user is quickly set up. This is a client side validation only, as the *DataAnnotationsValidator* will not send any request to the server. 

![DataAnnotationsValidator](bucket/d5e63763-9466-4eb1-9a0a-d471e70006c0.png)

You can even add your own validation attributes, to force a particular pattern on some properties.

![KebabCase](bucket/93042c1c-a5d8-4b40-98ed-27790833eedc.png)

# Endpoint data validation

If you are familiar with ASP.NET MVC, you know that it will support data annotation validation of the incoming model as well.

In ASP.NET MinimalAPI this is not yet the case, but I have feeling that MinimalAPI will evolve in the next versions. 

I'll omit validation of incoming data on the endpoint level for now.

# Business logic validation
 
I always like an additional validation step, which I am in charge of, right before I give the data to the data layer. This is the place where I experimented.

In my professional work environment we have the credo to always develop against an interface and only inject interfaces. Another word for it is dependency inversion. We also do this for validators, as it eliminates hard dependencies and also makes it easy to unit test as we can just mock away the validator. The downside of this approach *can* lead to complex validations that might even execute queries against the database. This will make an application slower and, in case of an application that relies an Azure RU/s, it also becomes more expensive.

That’s why I decided to use a static validation approach. It makes it hard to access dependencies and it keeps the validation fast.

![StaticValidation](bucket/650ed2db-66b4-442d-8dff-e48eac147c2c.png)

Additionally, any exceptions that might occur while accessing the data layer are caught and rethrown with a prettier error message. 

![ManagementException](bucket/bea956e8-e5f3-48bb-ba05-fca5d2f9ca19.png)

# Error handling

Exceptions which are triggered, are caught by the ASP.NET Middleware where they are converted into *ProblemDetails*. 

![CatchError](bucket/95c6f491-4225-40ed-b4de-0c749e2c196f.png)

The problem details are send to the client application, where they are received and converted into a client side exception.

![ReceiveError](bucket/d36311e1-8524-4e2c-b3a6-dfaa5cbec250.png)

The exception is handled with a global *ErrorBoundary*. Which displays the error in a very simple layout.

![ErrorBoundary](bucket/5e07db8b-fc12-480e-9a16-fbf7461168d8.png)

There is still room for improvement, but that is a story for another day. The most important thing for me is, that the error chain is working.

Bye.
