Explain difference between C#/.Net/.Net Core/.Net Web-API in table:
-----------------------------------------------------------

How to create a custom Middleware (how to register it in Program.cs):
------------------------------------------------------

how to create inline middleware:
--------------------------------------

How to write extension method and how to use it:
--------------------------------------------------

How to write custom action filter and use it on Action Methods(Controllers): -> Inheriting from ActionFilterAttributes (provides virtual methods OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted which we can override and use it like [CsrfFilter] attribute directly on Action method(Controller)) or (Inheriting from interfaces IActionFilter, IResultFilter, etc.. which has OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted method template and we must implement both if we inherit one of them) https://share.google/aimode/c2z1cnM7sDEyYdqnn 
-----------------------------------------------------------------------------------------------------------------------------------------

Other specific Built-in middlewares: which are interfaces which we can inherit & implement Methods directly
-------------------------------------------
https://share.google/aimode/LSNu0vvvBU1fNzKUf 

How do you call external endpoint from .net webapi using HttpClient or IHttpClientFactory: https://share.google/aimode/cZYh8JgyfbvE5UZgL
-----------------------------------------------------------------------


What is difference between IConfiguration & IOptions in .NET Core ?
-----------------------------------------------------------

What is Shallow copy and deep copy in C#:
--------------------------------------------------

Difference between Dependency Injection and Dependency Inversion:
-----------------------------------------------------------------

How to implement logging (built-in request&response logging using ILogger interface) and how to use it:
----------------------------------------------------------------------------------------------------
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-8.0


Explain SQL Query execution plan:
-------------------------------------------

How strings are immutable in C#:
----------------------------------


Questions on Caching, Multi-Threading, Events and Delegates, deadlock, race conditions,
--------------------------------------------------------------------

Scalability, Idempotancy, Concurrency
-----------------------------------------------

Async and await:
-------------------






