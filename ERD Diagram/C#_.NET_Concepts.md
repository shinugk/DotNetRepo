Explain difference between C#/.Net/.Net Core/.Net Web-API in table:
-----------------------------------------------------------
C# is a programming language, .NET is the platform that runs C# code, .NET Core is the modern cross-platform version of .NET, and ASP.NET Web API is a framework within .NET used to build RESTful APIs.

<br>

Lifecycle of C# Code Execution
------------------------------------
```
Your C# Code (.cs files)
        ↓
C# Compiler (Roslyn)
        ↓
Intermediate Language (IL) + Metadata
        ↓
Assembly (.dll / .exe)
        ↓
CLR (Common Language Runtime)
        ↓
JIT Compiler
        ↓
Native Machine Code
        ↓
Execution by CPU
```
1) You write code in C#: This is called Managed Code.
2) Compilation by C# Compiler (Roslyn) 
    - When you build the project <dotnet build> 
    - The compiler converts C# into ➜ Intermediate Language (IL / MSIL / CIL) outputfiles (program.dll/program.exe)
    - Assemblies contain:
        - IL Code
        - Metadata (types, methods, references)
        - Manifest (assembly info)
3) CLR Loads the Assembly
    - When you run: <dotnet run> . The CLR (Common Language Runtime) starts working.
4) JIT Compilation (Just-In-Time Compiler)
    - CLR does NOT directly execute IL.
    - Instead: <IL → JIT Compiler → Native Machine Code>
5) Execution by CPU
    - Now native machine code runs directly on the OS and CPU.

<br>

Difference between .Net6(C#10), .Net7(C#11), .Net8(C#12)
------------------------------------------------------------

<br>
<br>

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

<br>

How to create a custom Middleware (how to register it in Program.cs):
------------------------------------------------------
- Custom middleware is a user-defined component that runs inside the HTTP request pipeline to handle requests and responses.
- It allows you to:
   - Execute code before a request reaches the controller
   - Execute code after the controller returns a response
   - Modify request or response
   - Stop the request completely if needed
- In simple words: Middleware = Code that runs between Client → Server → Client.

Step 1: create middleware by inheriting IMiddleware
```
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class MyCustomMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Before controller
        Console.WriteLine("Request Path: " + context.Request.Path);   //Executes before controller

        await next(context); // Call next middleware

        // After controller
        Console.WriteLine("Response Status: " + context.Response.StatusCode);    //Executes after controller
    }
}
```
Step 2: Register Middleware in DI Container
```
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Register Middleware in DI
builder.Services.AddScoped<MyCustomMiddleware>();

var app = builder.Build();
```
Step 3: Add Middleware to Pipeline
```
app.UseMiddleware<MyCustomMiddleware>();
```
| Parameter            | Meaning                         |
| -------------------- | ------------------------------- |
| HttpContext          | Current HTTP request & response |
| RequestDelegate next | Next middleware in pipeline     |
- The line that controls flow: await next(context); . If you remove this → pipeline stops. This is called short-circuiting.

<br>
<br>

how to create inline middleware:
--------------------------------------
- Inline middleware is middleware written directly inside Program.cs using a lambda expression instead of creating a separate class file.
- We use the app.Use() method.
```
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Inline Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("Request Path: " + context.Request.Path);   // Code BEFORE next middleware

    await next(); // Pass request to next middleware

    Console.WriteLine("Response Status: " + context.Response.StatusCode);   // Code AFTER next middleware
});

app.MapGet("/", () => "Hello World");
app.Run();
```
Execution flow
```
Client Request
      ↓
Inline Middleware (Before)
      ↓
Endpoint / Controller
      ↓
Inline Middleware (After)
      ↓
Client Response
```
<br>
<br>

How to write extension method and how to use it:
--------------------------------------------------
- In C#, extension methods allow you to “add” new methods to an existing class without modifying the original class or creating a derived class.
- They are very commonly used in .NET (for example: string.IsNullOrEmpty(), LINQ methods like Where(), Select() etc.).
- Basic Rules to Create Extension Method
   - Must be inside a static class
   - Method must be static
   - First parameter must use the this keyword
   - The this parameter represents the type you are extending

Example 1 — Extension Method for int
```
public static class IntExtensions
{
    public static bool IsEven(this int number)
    {
        return number % 2 == 0;
    }
}
```
Step 2 — How to Use It
```
class Program
{
    static void Main()
    {
        int num = 10;
        if (num.IsEven())   //using extension method -> returns true
        {
            Console.WriteLine("Even number");
        }
    }
}
```
- you can create Extension Method with Multiple Parameters

<br>
<br>

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

<br>
<br>

Other specific Built-in Filters: which are interfaces which we can inherit & implement Methods directly
-------------------------------------------
- (Inheriting from interfaces IActionFilter, IResultFilter, etc.. which has OnActionExecuting, OnActionExecuted, OnResultExecuting, OnResultExecuted method template and we must implement both if we inherit one of them) https://share.google/aimode/c2z1cnM7sDEyYdqnn
- In ASP.NET Core, filters allow you to run code at specific stages of the request processing pipeline. They are typically used for "cross-cutting concerns" like security, logging, or caching to keep your controller actions clean. 

Primary Built-in Filter Types 
Each filter runs in a strictly defined order: 
1) IAuthorizationFilter:
- When it runs: First in the pipeline, immediately after routing.
- Purpose: Determines if the user is permitted to access the resource. It can short-circuit the request (stop it immediately) if the user is unauthorized.
2) IResourceFilter:
- When it runs: Right after authorization and before model binding.
- Purpose: Wraps most of the pipeline. Since it runs before model binding, it is ideal for response caching because it can return a cached result before the system does the expensive work of processing the request body.
3) IActionFilter:
- When it runs: Immediately before and after a controller action executes.
- Purpose: Allows you to inspect or modify the parameters being passed into an action or the result returned from it.
4) IExceptionFilter:
- When it runs: Only when an unhandled exception occurs during action execution.
- Purpose: Centralizes error handling logic. It is the last line of defense within the MVC pipeline.
5) IResultFilter:
- When it runs: Just before and after the execution of the final action result (e.g., while rendering a View or serializing JSON).
- Purpose: Used for logic that must surround the formatting of the response.

Built-in Filter Types and methods they provide:
| Filter Type              | Synchronous Interface & Methods                                            | Asynchronous Interface & Methods                          | When It Runs                              | Purpose                                              |
| ------------------------ | -------------------------------------------------------------------------- | --------------------------------------------------------- | ----------------------------------------- | ---------------------------------------------------- |
| **Authorization Filter** | `IAuthorizationFilter`<br>• `OnAuthorization()`                            | `IAsyncAuthorizationFilter`<br>• `OnAuthorizationAsync()` | **Before everything**                     | Checks whether user is authorized to access resource |
| **Resource Filter**      | `IResourceFilter`<br>• `OnResourceExecuting()`<br>• `OnResourceExecuted()` | `IAsyncResourceFilter`<br>• `OnResourceExecutionAsync()`  | After authorization, before model binding | Used for caching, performance monitoring, setup logic           |
| **Action Filter**        | `IActionFilter`<br>• `OnActionExecuting()`<br>• `OnActionExecuted()`       | `IAsyncActionFilter`<br>• `OnActionExecutionAsync()`      | Before and after controller action method | Validation, logging, modifying inputs/outputs        |
| **Exception Filter**     | `IExceptionFilter`<br>• `OnException()`                                    | `IAsyncExceptionFilter`<br>• `OnExceptionAsync()`         | When exception occurs                     | Handle errors globally (Logging errors to a database or returning standardized error responses like ProblemDetails)                              |
| **Result Filter**        | `IResultFilter`<br>• `OnResultExecuting()`<br>• `OnResultExecuted()`       | `IAsyncResultFilter`<br>• `OnResultExecutionAsync()`      | Before and after action result execution  | Modify response before sending to client (Adding custom HTTP headers to the final output.)            |

<br>
<br>

How do you call external endpoint from .net webapi using HttpClient or IHttpClientFactory:
-----------------------------------------------------------------------
There are 2 main ways:
- Direct HttpClient (not recommended for production) ❌
- IHttpClientFactory (recommended — avoids socket exhaustion, better DI support) ✅

Example 1. Calling External API using HttpClient (Basic Way)
```
public class ExternalApiService
{
    private readonly HttpClient _httpClient;

    public ExternalApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetDataAsync()
    {
        var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts/1");

        response.EnsureSuccessStatusCode(); // Throws exception if status code is not 200-299

        var result = await response.Content.ReadAsStringAsync();

        return result;
    }
}
```
Problem with this approach
- Creating HttpClient manually many times causes:
- Socket exhaustion
- Performance issues
- DNS caching problems

Example 2. Calling External API using IHttpClientFactory (Recommended)
- Register HttpClient in Program.cs:
```
builder.Services.AddHttpClient();
```
- Create Service
```
public class ExternalApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetPostAsync()
    {
        var response = await _httpClientFactory.GetAsync("https://jsonplaceholder.typicode.com/posts/1");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
```
- Register Service in DI
```
builder.Services.AddScoped<ExternalApiService>();
```

Do you use using with HttpClient?
- Answer:
  - When using IHttpClientFactory or Typed Client, we should NOT use 'using' because the factory manages the lifetime.
  - If we create HttpClient manually, using is possible but not recommended in ASP.NET Core.


<br>
<br>

What is 'using' in .Net:
---------------------------------
- using is used to automatically release unmanaged resources when you are done using an object.
- It ensures that Dispose() method is called.
- Why do we need using?: Some objects use external resources like: Files, Database connections, Network connections, Streams, HttpResponseMessage, Graphics objects
- These are NOT cleaned immediately by Garbage Collector (GC).
- So we manually release them using: IDisposable → Dispose() → using

Basic Example — File Handling without 'using'
```
FileStream file = new FileStream("test.txt", FileMode.Open);

// work with file
file.Dispose(); // Must remember to call
```
Problem ❌: If exception occurs → Dispose never runs → file lock remains.

With using (Safe ✅):
```
using (FileStream file = new FileStream("test.txt", FileMode.Open))
{
    // work with file
}
// Dispose automatically called here
```
Even if exception occurs → Dispose still runs ✅

- Modern C# Syntax (Best) -> var file = new FileStream("test.txt", FileMode.Open); -> Dispose happens automatically at end of scope.

What Happens Internally? using converts into:
```
var resource = new Resource();
try
{
    // work
}
finally
{
    resource.Dispose();   
}
```
-> So even if exception occurs → Dispose runs.

✅ When Should You Use using?
- Use when object implements: IDisposable
- Examples: FileStream, StreamReader, SqlConnection, HttpResponseMessage, MemoryStream, Graphics, CancellationTokenSource

✅ When NOT to Use using?
Do NOT use when:
- Object lifetime managed by Dependency Injection
- Singleton services
- IHttpClientFactory HttpClient
- Controller / Service classes
- Example ❌ -> using var service = new MyService(); // wrong if DI managed>

Why do we use using in .NET?
- We use using to ensure that unmanaged resources are released properly by automatically calling Dispose(). It prevents memory leaks and resource locking issues, especially for objects like file streams, database connections, and network resources.

<br>
<br>

what is difference between throw and throe ex:
-----------------------------------------------------
- The difference between throw and throw ex is about stack trace preservation.
- throw; → preserves the original stack trace ✅ (Best practice)
- throw ex; → resets the stack trace ❌ (Bad practice)

✅ Example — Using throw (Correct Way)
```
try
{
    Method1();
}
catch (Exception ex)
{
    Console.WriteLine("Error occurred");
    throw; // rethrow original exception   
}
```

Interview Trick Question: 👉 What happens if we use throw outside catch?
- throw; // compile error
- only valid inside catch block.

When Should You Use throw ex? Almost never.
- Only rare case: When you want to wrap exception into a new one
```
catch (Exception ex)
{
    throw new ApplicationException("Custom message", ex);
}
```
Here we are creating a new exception, not rethrowing.

<br>
<br>


What is difference between IConfiguration & IOptions in .NET Core ?
-----------------------------------------------------------

What is Shallow copy and deep copy in C#:
--------------------------------------------------

Difference between Dependency Injection and Dependency Inversion:
-----------------------------------------------------------------

How to implement logging (built-in request&response logging using ILogger interface) and how to use it:
----------------------------------------------------------------------------------------------------
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-8.0


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

<br>

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

<br>

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















