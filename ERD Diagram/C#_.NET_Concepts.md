Explain difference between C#/.Net/.Net Core/.Net Web-API in table:
-----------------------------------------------------------
C# is a programming language, .NET is the platform that runs C# code, .NET Core is the modern cross-platform version of .NET, and ASP.NET Web API is a framework within .NET used to build RESTful APIs.

What is difference between Method Overriding and Method Hiding:
----------------------------------------------------------------
While both override and new allow a child class to have a method with the same name as its parent, they behave very differently when you treat a child object as if it were a parent. 
1. Method Overriding (override):
- What it does: It replaces the parent's logic with the child's logic.
- Polymorphism: This is "true" polymorphism. No matter how you reference the object (as a parent or a child), the child's version always runs.
- Requirement: The parent method must be marked as virtual or abstract.
  <br>
2. Method Hiding (new)
- What it does: It hides the parent's method from the child class but keeps the original parent method intact.
- Behavior: The method that runs depends on the variable type, not the actual object.
- If the variable is the Parent type, the Parent's method runs.
- If the variable is the Child type, the Child's method runs.
```
public class Base {
    public virtual void Show() => Console.WriteLine("Base Show");
}

public class Overrider : Base {
    public override void Show() => Console.WriteLine("Overrider Show");
}

public class Hider : Base {
    public new void Show() => Console.WriteLine("Hider Show");
}

---------Method Overriding--------------(Run-time / late bidning)
//If the instance is child class then it will override whatever variable type is
//Calls the instance/object version
Base obj = new Overrider();
         or
Overrider obj = new Overrider();
obj.Show(); // Output: "Overrider Show" (Polymorphism) 

//If the instance is base class then it is straight forward
Base obj2 = new Base();
obj2.Show(); // output: "Base Show" 


--------------Method Hiding-----------------(compile-time / early binding)
//It does not matter instance. it will always executes the variable version method
//calls the variable version
Base obj4 = new Hider();
obj4.Show(); // Output: "Base Show" (The child's method is hidden)
```
<img width="750" height="350" alt="image" src="https://github.com/user-attachments/assets/8f5dcc19-2714-4883-9136-e407ac953b3f" />



How to create a custom Middleware (how to register it in Program.cs):
------------------------------------------------------

how to create inline middleware:
--------------------------------------

How to write extension method and how to use it:
--------------------------------------------------

How to write custom action filter and use it on Action Methods(Controllers):
-----------------------------------------------------------------------------------------------------------------------------------------
- Inheriting from ActionFilterAttributes (provides virtual methods OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted which we can override and use it like attribute directly on Action method(Controller)) or
```
 public class ValidationActionFilter : ActionFilterAttribute
 {
     public override void OnActionExecuting(ActionExecutingContext context)
     {
         if (!context.ModelState.IsValid)
         {
             var path = context?.HttpContext?.Request?.Path.Value ?? "";
             bool isPublic = path.StartsWith("/public/api/", StringComparison.OrdinalIgnoreCase);
             
             if(isPublic)
             {
                 var errorDetails = context.ModelState.PublicAllErrors();
                 var publicResp = new
                 {
                     type = "BIO400",
                     title = "Bad Request",
                     status = "400",
                     detail = "One or more fields did not pass validation.",
                     errors = errorDetails
                 };
                 context.Result = new BadRequestObjectResult(publicResp);
             }             
         }
     }
 }
```
How To Use This Filter
Option 1 — Apply on Controller -> Filter runs for all actions in controller.
```
[ValidationActionFilter]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(UserDto dto)
    {
        return Ok();
    }
}
```
Option 2 — Apply on Specific Action -> Only this action uses filter.
```
[HttpPost]
[ValidationActionFilter]
public IActionResult Create(UserDto dto)
{
    return Ok();
}
```
Option 3 — Register Globally (Best Practice) in Program.cs controller section-> Now filter runs for every controller automatically
```
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ValidationActionFilter()); //Validate incoming requests, return a BadRequest status if the model state does not pass
    })
```

Other specific Built-in Filters: which are interfaces which we can inherit & implement Methods directly
-------------------------------------------
- (Inheriting from interfaces IActionFilter, IResultFilter, etc.. which has OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted method template and we must implement both if we inherit one of them) https://share.google/aimode/c2z1cnM7sDEyYdqnn
- In ASP.NET Core, filters allow you to run code at specific stages of the request processing pipeline. They are typically used for "cross-cutting concerns" like security, logging, or caching to keep your controller actions clean. 

Primary Built-in Filter Types 
Each filter runs in a strictly defined order: 
1) IAuthorizationFilter:
- When it runs: First in the pipeline, immediately after routing.
- Purpose: Determines if the user is permitted to access the resource. It can short-circuit the request (stop it immediately) if the user is unauthorized.
- Common Use: Implementing custom access rules or integrating with external identity providers.
2) IResourceFilter:
- When it runs: Right after authorization and before model binding.
- Purpose: Wraps most of the pipeline. Since it runs before model binding, it is ideal for response caching because it can return a cached result before the system does the expensive work of processing the request body.
- Common Use: Performance monitoring or bypassing the action entirely for cached data.
3) IActionFilter:
- When it runs: Immediately before and after a controller action executes.
- Purpose: Allows you to inspect or modify the parameters being passed into an action or the result returned from it.
- Common Use: Validating ModelState automatically or logging incoming request payloads.
4) IExceptionFilter:
- When it runs: Only when an unhandled exception occurs during action execution.
- Purpose: Centralizes error handling logic. It is the last line of defense within the MVC pipeline.
- Common Use: Logging errors to a database or returning standardized error responses like ProblemDetails.
5) IResultFilter:
- When it runs: Just before and after the execution of the final action result (e.g., while rendering a View or serializing JSON).
- Purpose: Used for logic that must surround the formatting of the response.
- Common Use: Adding custom HTTP headers to the final output.

Built-in Filter Types and methods they provide:
| Filter Type              | Synchronous Interface & Methods                                            | Asynchronous Interface & Methods                          | When It Runs                              | Purpose                                              |
| ------------------------ | -------------------------------------------------------------------------- | --------------------------------------------------------- | ----------------------------------------- | ---------------------------------------------------- |
| **Authorization Filter** | `IAuthorizationFilter`<br>• `OnAuthorization()`                            | `IAsyncAuthorizationFilter`<br>• `OnAuthorizationAsync()` | **Before everything**                     | Checks whether user is authorized to access resource |
| **Resource Filter**      | `IResourceFilter`<br>• `OnResourceExecuting()`<br>• `OnResourceExecuted()` | `IAsyncResourceFilter`<br>• `OnResourceExecutionAsync()`  | After authorization, before model binding | Used for caching, performance, setup logic           |
| **Action Filter**        | `IActionFilter`<br>• `OnActionExecuting()`<br>• `OnActionExecuted()`       | `IAsyncActionFilter`<br>• `OnActionExecutionAsync()`      | Before and after controller action method | Validation, logging, modifying inputs/outputs        |
| **Exception Filter**     | `IExceptionFilter`<br>• `OnException()`                                    | `IAsyncExceptionFilter`<br>• `OnExceptionAsync()`         | When exception occurs                     | Handle errors globally                               |
| **Result Filter**        | `IResultFilter`<br>• `OnResultExecuting()`<br>• `OnResultExecuted()`       | `IAsyncResultFilter`<br>• `OnResultExecutionAsync()`      | Before and after action result execution  | Modify response before sending to client             |


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


Explain Caching strategies in .NET
--------------------------------------------------------------------

Questions on Caching, Multi-Threading, Events and Delegates, deadlock, race conditions,
--------------------------------------------------------------------

Scalability, Idempotancy, Concurrency
-----------------------------------------------

Async and await:
-------------------

Task and Task<T>:
--------------------
- In C#, Task and Task<T> are the cornerstones of Asynchronous Programming. They represent an operation that will finish in the future, allowing your application to stay responsive (like keeping a UI from freezing or a server from idling) while waiting for long-running work to complete.
  
1). Task (The "Void" Version)
A Task represents a single operation that does not return a value. It is the asynchronous equivalent of a void method. You use it when you care that the work finished, but you don't need data back. 
Common uses:
  - Saving a file to disk.
  - Sending an email.
  - Updating a database record.
```
public async Task SaveDataAsync()
{
    Console.WriteLine("Starting save...");
    
    // Simulate a 2-second delay (like a database write)
    await Task.Delay(2000); 
    
    Console.WriteLine("Data saved successfully!");
}
```
.Net WebAPI Example:
```
[HttpPost]
public async Task<IActionResult> SaveUser(User user)
{
    await _service.SaveAsync(user);
    return Ok();                       //No value returned from service → Task
}
```
2). Task (The "Return" Version)
A Task<T> represents an asynchronous operation that returns a value of type T. It is the asynchronous equivalent of a method that returns a specific type (like int, string, or a custom object). 
Common uses:
  - Fetching data from an API.
  - Reading text from a file.
  - Calculating a complex formula
```
public async Task<string> FetchWeatherAsync()
{
    Console.WriteLine("Fetching weather...");
    
    await Task.Delay(3000); // Simulate network latency
    
    return "Sunny, 25°C"; // The 'string' inside Task<string>
}
```
.Net WebAPI Example:
```
[HttpGet]
public async Task<User> GetUser(int id)
{
    return await _service.GetUserAsync(id);   //Returns User → Task<User>
}
```
| Feature                  | **Task**                                                | **Task<T>**                                                        |
| ------------------------ | ------------------------------------------------------- | ------------------------------------------------------------------ |
| **Return Value**         | No return value (like `void`)                           | Returns a value of type `T`                                      |
| **Usage**                | `await DoWork();`                                       | `string result = await GetData();`                                 |
| **When to Use**          | When you only need to perform an operation              | When you need a result from the operation                          |
| **Example Return Type**  | `Task`                                                  | `Task<int>`, `Task<string>`, `Task<User>`                          |
| **ASP.NET Core Example** | `Task<IActionResult>` when no data returned             | `Task<User>` when returning data                                   |

🔥 Task vs Thread (Interview Question)
| Task             | Thread               |
| ---------------- | -------------------- |
| Lightweight      | Heavy                |
| Managed by .NET  | Managed by OS        |
| Uses thread pool | Creates new thread   |
| Recommended      | Rarely used directly |

Why We Use Task?
- Because async operations like: Database calls, API calls, File operations, Network requests take time.
- Without Task → thread gets blocked ❌
- With Task → thread is free while waiting ✅

How Garbage Collection works in the.NET CLR: and How Finalize and Dispose works:
---------------------------------------------------------------

Difference between ref, out, and in parameters in C#.
-------------------------------------------------------

Explain Value types vs Reference types in C#:
------------------------------------------


What is the difference between String and StringBuilder in C#;
------------------------------------------------------------


what is difference between static, const, readonly variables:
---------------------------------------------------
Constants are static by default

Difference between the Equality Operator (==) and Equals() Method in C#:
-----------------------------------------------------------------

what is boxing and Unboxing in C#:
------------------------------------















