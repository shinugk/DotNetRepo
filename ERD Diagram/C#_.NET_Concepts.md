Explain difference between C#/.Net/.Net Core/.Net Web-API in table:
-----------------------------------------------------------
- C# is a programming language,
- .NET is the platform that runs C# code & It is a software development platform created by Microsoft.
   - It includes: Runtime (CLR), Libraries, Languages (C#, F#, VB), Frameworks (ASP.NET, WinForms, WPF)
   - It is used to build: Web apps, APIs, Desktop apps, Mobile apps
- .NET Core is the modern cross-platform version of .NET.
   - After 2020: .NET Core → merged into .NET 5+
   - Now we just say .NET 6, .NET 7, .NET 8
- ASP.NET Web API is a framework within .NET used to build RESTful APIs.

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
<br>

Data Types in C# .Net:
--------------------------------------------------------
| Category        | Types Included                                    | Description                      | Examples                   |
| --------------- | ------------------------------------------------- | -------------------------------- | -------------------------- |
| Value Types     | int, float, bool, char, struct, enum              | Store actual data directly       | `int x = 10;`              |
| Reference Types | string, class, object, array, interface, delegate | Store memory reference (address) | `Person p = new Person();` |
| Pointer Types   | pointer (`*`)                                     | Used in unsafe code              | `int* ptr`                 |

**1) Built-in Value Types Table:** <br>
**a) Integer Types:**
   | Type   | Size    | Range          | Example           | .NET Type     |
   | ------ | ------- | -------------- | ----------------- | ------------- |
   | byte   | 1 byte  | 0 to 255       | `byte b = 10;`    | System.Byte   |
   | sbyte  | 1 byte  | -128 to 127    | `sbyte s = -5;`   | System.SByte  |
   | short  | 2 bytes | ±32K           | `short s = 100;`  | System.Int16  |
   | ushort | 2 bytes | 0 to 65K       | `ushort u = 200;` | System.UInt16 |
   | int    | 4 bytes | ±2B            | `int x = 10;`     | System.Int32  |
   | uint   | 4 bytes | positive       | `uint u = 10;`    | System.UInt32 |
   | long   | 8 bytes | large          | `long l = 10L;`   | System.Int64  |
   | ulong  | 8 bytes | large positive | `ulong u = 10UL;` | System.UInt64 |

**b) Floating Point Types:**
   | Type    | Size     | Precision    | Usage           | Example              |
   | ------- | -------- | ------------ | --------------- | -------------------- |
   | float   | 4 bytes  | 7 digits     | Scientific      | `float f = 10.5f;`   |
   | double  | 8 bytes  | 15–16 digits | Default decimal | `double d = 10.5;`   |
   | decimal | 16 bytes | 28–29 digits | Financial       | `decimal m = 10.5m;` |

**c) Other Primitive Value Types:**
   | Type | Description              | Example                 |
   | ---- | ------------------------ | ----------------------- |
   | bool | true/false               | `bool isActive = true;` |
   | char | Single Unicode character | `char c = 'A';`         |

**d) User Defined Value Types:**
   | Type   | Description       | Example                     |
   | ------ | ----------------- | --------------------------- |
   | struct | Custom value type | `struct Person { int Id; }` |
   | enum   | Named constants   | `enum Days { Mon, Tue }`    |

<br>

**2) Reference Types Table:**
   | Type      | Description            | Example                  |
   | --------- | ---------------------- | ------------------------ |
   | class     | Blueprint for objects  | `class Person {}`        |
   | object    | Base type of all types | `object obj = 10;`       |
   | string    | Text data (immutable)  | `string name = "Jaith";` |
   | array     | Collection of items    | `int[] arr = {1,2};`     |
   | interface | Contract definition    | `interface IAnimal {}`   |
   | delegate  | Function pointer       | `delegate void MyDel();` |
   | dynamic   | Runtime type           | `dynamic d = 10;`        |


**Value Type vs Reference Type — Complete Comparison:**
| Feature            | Value Type                        | Reference Type       |
| ------------------ | --------------------------------- | -------------------- |
| Storage Location   | Stack (usually)                   | Heap                 |
| Stores             | Actual Data                       | Memory Address       |
| Copy Behavior      | New copy created                  | Same reference       |
| Null Allowed       | No (except nullable)              | Yes                  |
| Performance        | Faster                            | Slightly slower      |
| Memory Allocation  | Inline                            | Separate object      |
| Garbage Collection | Not required                      | Required             |
| Inheritance        | No (except from System.ValueType) | Yes                  |
| Examples           | int, bool, struct                 | class, string, array |


**Stack vs Heap Table:**
| Feature       | Stack                   | Heap              |
| ------------- | ----------------------- | ----------------- |
| Speed         | Very fast               | Slower            |
| Size          | Small                   | Large             |
| Stores        | Value types, references | Objects           |
| Cleanup       | Automatic               | Garbage Collector |
| Thread Safety | Yes                     | No                |


**Boxing and Unboxing:**
| Concept     | Description       | Example             |
| ----------- | ----------------- | ------------------- |
| Boxing      | Value → Reference | `object obj = 10;`  |
| Unboxing    | Reference → Value | `int x = (int)obj;` |
| Performance | Slower            | Because conversion  |




<br>
<br>

Difference between .Net6(C#10), .Net7(C#11), .Net8(C#12)
------------------------------------------------------------
| Category          | .NET 6 (C# 10)     | .NET 8 (C# 12)                      | .NET 10 (C# 14)                   |
| ----------------- | ------------------ | ----------------------------------- | --------------------------------- |
| Performance       | Good               | Much faster (JIT + GC improvements) | Even faster runtime optimizations |
| Minimal APIs      | Introduced         | Mature + Filters + Better routing   | More enhancements                 |
| AI Integration    | None               | AI libraries introduced             | Deep AI ecosystem                 |
| Memory Usage      | Moderate           | Lower memory footprint              | Further optimized                 |
| Startup Time      | Good               | Very fast                           | Faster                            |
| Observability     | Basic logging      | OpenTelemetry support               | Better monitoring                 |
| ASP.NET Core      | Stable             | More scalable & faster              | More optimizations                |
| Language Features | C# 10 improvements | Modern C# features                  | Advanced language features        |

Important C# 12 Features: (.Net 8) (LTS: long-term support till 2026+) ✅
1) Primary Constructors:
- Before:
```
public class Person
{
    private readonly string _name;
    public Person(string name)
    {
        _name = name;
    }
}
```
- C# 12:
```
public class Person(string name)                      //Very useful in: Dependency Injection, Services, Controllers
{
     public void Print() => Console.WriteLine(name);
}
```

2) Collection Expressions:
- Before: `int[] numbers = new int[] { 1, 2, 3 };`
- After: `int[] numbers = [1, 2, 3];`    //Works with: Arrays, Lists, Spans

3) Default Parameters in Lambdas:
```
var add = (int a, int b = 10) => a + b;
Console.WriteLine(add(5)); // 15
```

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

Example 1: Creating Custom Middleware (with IMiddleware)
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
- The line that controls flow: `await next(context);`. If you remove this → pipeline stops. This is called short-circuiting.

<br>

Example 2: Creating global Exception middleware (without IMiddleware)
```
public class GlobalExceptionMiddleware {
  private readonly RequestDelegate _next;

  public GlobalExceptionMiddleware(RequestDelegate next) {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context) {
    try {
      await _next(context);     //Go to next middleware, anywhere exception happens in middleware flow, it catches in below and modifiies custom response
    } catch (Exception ex) {
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(
          new { Message = "An error occurred", Error = ex.Message });
    }
  }
}
```
- Register: `app.UseMiddleware<GlobalExceptionMiddleware>();`

EXPLANATION:
- RequestDelegate _next: This delegate represents the next middleware component in the pipeline. Calling _next(context) passes control to the subsequent middleware.
- HttpContext context: This object provides access to the current HTTP request and response, allowing you to inspect and modify them within your middleware.
- InvokeAsync: This is the entry point for your middleware's logic. You can perform actions before calling _next(context) (e.g., request validation, logging) and after (e.g., response modification, additional logging).
- app.UseMiddleware<YourMiddlewareClassName>(): This method adds your custom middleware to the ASP.NET Core request pipeline. When a request comes in, it will flow through the middleware components in the order they are registered. 

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

How to implement logging and how to use it:
----------------------------------------------------------------------------------------------------
- .NET already provides a built-in logging framework using the ILogger interface.
- ILogger is an interface provided by .NET for logging messages from your application.
- It supports log levels:
   | Log Level   | Purpose                |
   | ----------- | ---------------------- |
   | Trace       | Very detailed          |
   | Debug       | Debugging              |
   | Information | Normal app flow        |
   | Warning     | Unexpected but handled |
   | Error       | Errors                 |
   | Critical    | System failure         |

Step 1: Built-in Logging Already Enabled (No extra setup needed in Program.cs)

Step 2: Inject ILogger into Controller
```
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;           //should be of type ILogger<T>

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Fetching all products");

        return Ok(new[] { "Laptop", "Mobile" });
    }
}
```
Console:
```
info: ProductsController[0]
      Fetching all products
```

Step 3: Configure Log Levels (appsettings.json)
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```
- You can change to: Debug, Trace, Error

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

How Garbage Collection works in the.NET CLR: and How Finalize and Dispose works:
--------------------------------------------------------------------------------
- Garbage Collection is an automatic memory management system in .NET that frees memory occupied by objects that are no longer in use.
- In simple words:
    - GC automatically deletes unused objects from memory. 
    - So, developers do not need to manually free memory like in C/C++.
 
**1) Why Garbage Collection is Needed:**
- When we create objects: `var person = new Person();`
- Memory is allocated in Heap.
- If objects are never removed → memory becomes full → application crashes.
- GC solves this by:
    - ✅ Finding unused objects
    - ✅ Removing them
    - ✅ Reclaiming memory
 
**2) Memory Types in .NET, There are two main memory areas:**

   | Memory | Stores                    |
   | ------ | ------------------------- |
   | Stack  | Value types, method calls |
   | Heap   | Reference types (objects) |

- NOTE: Garbage Collector works only on Heap memory.

**3) Example:**
```
public void CreateObjects()
{
    var obj = new Person();
}
```
After method ends:
- obj has no reference
- GC will remove it later
- We do NOT delete manually.

**4) When Does GC Run?**
- Garbage collector runs automatically when:
   - Memory is low
   - Many objects created
   - System idle
   - Gen threshold reached
   - You can also force: `GC.Collect();` But Not recommended in production ❌

**5) How Garbage Collection Works (Important)**
- GC uses a concept called Generations.
- Objects are grouped based on age.

   | Generation | Description               |
   | ---------- | ------------------------- |
   | Gen 0      | New objects (short-lived) |
   | Gen 1      | Medium lifetime           |
   | Gen 2      | Long-lived objects        |

- Most objects die young → Gen 0 is collected frequently.
<br>
<br>


what is Finalize and Dispose:
--------------------------------------
- In .NET, Finalize and Dispose are both related to memory/resource cleanup, but they serve different purposes and are used in different situations.
- Think of it like this:
    - Dispose → Manual cleanup (you do it)
    - Finalize → Automatic cleanup (GC does it if you forget)

✅ 1. Dispose in .NET
- Dispose() is used to release unmanaged resources manually and immediately when you are done using an object.
- It comes from the IDisposable interface.
- Why Dispose is Needed?
   - .NET Garbage Collector only manages managed memory.
   - But some resources are unmanaged, like: File handles, Database connections, Network sockets, OS handles, Streams
   - These must be released manually → using Dispose()

✅ 2. Finalize in .NET (Destructor)
- Finalize is called by Garbage Collector automatically before the object is removed from memory.
- You cannot control when Finalize runs. GC decides.
It is written using destructor syntax:
```
public class FileManager
{
    ~FileManager()
    {
        Console.WriteLine("Finalize called");
    }
}
```
Most Important Interview Point ⭐ : Finalizer should only be used when you have unmanaged resources and no safer alternative.

<br>
<br>


what is difference between throw and throw ex:
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

what are Nullable types in .Net:
---------------------------------------
- In .NET, nullable data types allow value types (like int, bool, DateTime, etc.) to hold a null value.
- Normally:
   - Value types cannot be null
   - Reference types can be null
- Nullable types solve this limitation for value types.

✅ 1. Why Nullable Types Are Needed
```
int age = null;   // ❌ Compile-time error
```
- Because int is a value type.
- But sometimes we need:
   - Database values (NULL from DB)
   - Optional fields
   - Missing data
   - User input not provided
- So .NET provides Nullable<T>

✅ 2. Syntax of Nullable Types   <br>
Method 1 — Using ? (Most Common)
```
int? age = null;
bool? isActive = null;
DateTime? joiningDate = null;
```
Method 2 — Using Nullable<T>
```
Nullable<int> age = null;
```
- Both are the same. int? is just shorthand for Nullable<int>.

✅ 3. Null Coalescing Operator : ??
```
int? age = null;
int result = age ?? 18;  
Console.WriteLine(result); // 18
```
- Provides default value if null.

✅ 4. Null Conditional Operator ?.
```
int? number = null;
Console.WriteLine(number?.ToString());
```
- Used with reference or nullable types. Prevents exception.

summary:
| Operator | Name                       | Example   |
| -------- | -------------------------- | --------- |
| ?        | Nullable type              | int?      |
| ??       | Null coalescing            | x ?? 0    |
| ?.       | Null conditional           | obj?.Name |
| ??=      | Null coalescing assignment | x ??= 5   |



<br>
<br>

Async and await:
-------------------




<br>
<br>

**Task and Task<T>:**
----------------------------
- In C#, Task and Task<T> are the cornerstones of Asynchronous Programming. They represent an operation that will finish in the future, allowing your application to stay responsive (like keeping a UI from freezing or a server from idling) while waiting for long-running work to complete.
  
**1). Task (The "Void" Version)**
- A Task represents a single operation that does not return a value. It is the asynchronous equivalent of a void method. You use it when you care that the work finished, but you don't need data back. 
- Common uses:
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

**2). Task<T> (The "Return" Version)**
- A Task<T> represents an asynchronous operation that returns a value of type T. It is the asynchronous equivalent of a method that returns a specific type (like int, string, or a custom object). 
- Common uses:
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
<br>


Ref vs Out Keyword
----------------------------------------
- The REF and OUT keywords in C# are used for passing the arguments to a method as a reference type.
- By default, arguments are passed to a method by value.
- By using these REF and OUT keywords in C#, we can pass arguments by reference.
- In this case, any changes made to these arguments in the method body will be reflected in those variable when the control returns to the calling method.
```
        class Program
        {
            static void Main(string[] args)
            {
                int Result = Math(100, 200);
                Console.WriteLine($"Result: {Result}");
                Console.ReadKey();
            }
            public static int Math(int number1, int number2)
            {
                return number1 + number2;
            }
        }
```
- Output: Result: 300

- Now, my requirement is, when I call the Math function, I want to return the Addition, Multiplication, Subtraction, and Division of the two numbers passed to this function. 
- But, if you know, it is only possible to return a single value from a function in C# at any given point in time i.e. only one output from a function.


**Example using REF to Return Multiple Outputs From a Function in C#:**
```
        class Program
        {
            static void Main(string[] args)
            {
                //First Declare the Variables
                int Addition = 0;
                int Multiplication = 0;
                int Subtraction = 0;
                int Division = 0;

                //While calling the Method, decorate the ref keyword for ref arguments
                Math(200, 100, ref Addition, ref Multiplication, ref Subtraction, ref Division);

                Console.WriteLine($"Addition: {Addition}");                //300
                Console.WriteLine($"Multiplication: {Multiplication}");    //20000
                Console.WriteLine($"Subtraction: {Subtraction}");          //100
                Console.WriteLine($"Division: {Division}");                //2
            
                Console.ReadKey();
            }

            //Declaring Method with Ref Parameters
            public static void Math(int number1, int number2, ref int Addition, 
                                    ref int Multiplication, ref int Subtraction, ref int Division)
            {
                Addition = number1 + number2;              
                Multiplication = number1 * number2;         
                Subtraction = number1 - number2;           
                Division = number1 / number2;              //This will Update the Division variable Declared in Main Method similarly all other variables
            }
        }
```


**Example using OUT Parameter to Return Multiple Outputs from a Function in C#:**
```
        class Program
        {
            static void Main(string[] args)
            {
                //First Declare the Variables
                int Addition = 0;
                int Multiplication = 0;
                int Subtraction = 0;
                int Division = 0;

                //While calling the Method, decorate the out keyword for out arguments
                Math(200, 100, out Addition, out Multiplication, out Subtraction, out Division);

                Console.WriteLine($"Addition: {Addition}");
                Console.WriteLine($"Multiplication: {Multiplication}");
                Console.WriteLine($"Subtraction: {Subtraction}");
                Console.WriteLine($"Division: {Division}");
            
                Console.ReadKey();
            }

            //Declaring Method with out Parameters
            public static void Math(int number1, int number2, out int Addition,
                                    out int Multiplication, out int Subtraction, out int Division)
            {
                Addition = number1 + number2; 
                Multiplication = number1 * number2;
                Subtraction = number1 - number2; 
                Division = number1 / number2;           //This will Update the Division variable Declared in Main Method
            }
        }
```
- Fine. We are getting the same result. That means using out, we are also getting the updated values from the Math function. 
- So, it is working very similarly to the ref parameter.

<br>
<br>

What are the Differences Between OUT and REF Keyword in C#?
-------------------------------------------------------------
**1) Use ref when:**
- The variable already has a value
- The method may read and modify that value

📜 Rules:
- ✔ Variable must be initialized before passing
- ✔ Method can read & update it
- ✔ Caller and callee both use ref
```
        int x = 10;      // must be initialized
        Increment(ref x);
        Console.WriteLine(x); // Output: 11

        void Increment(ref int number)
        {
            number = number + 1;
        }
```

**2) Use out when:**
- The method’s job is to produce a value
- The input value is not important

📜 Rules
- ✔ Variable does NOT need to be initialized
- ✔ Method must assign a value before returning
- ✔ Caller and callee both use out
```
        int sum;             // no initialization required
        GetSum(5, 3, out sum);
        Console.WriteLine(sum); // Output: 8

        void GetSum(int a, int b, out int result)
        {
            result = a + b;   // must assign
        }
```

**Side-by-Side Comparison:**

   | Feature                         | `ref`                 | `out`                    |
   | ------------------------------- | --------------------- | ------------------------ |
   | Must initialize before passing  | ✅ Yes                 | ❌ No                     |
   | Method must assign value        | ❌ No                  | ✅ Yes                    |
   | Can read value before assigning | ✅ Yes                 | ❌ No                     |
   | Used for                        | Modify existing value | Return value from method |
   | Intent                          | Input + Output        | Output only              |



<br>
<br>
   

Var vs Dynamic keyword
--------------------------------
**1. var keyword (Compile-time typing)**
- var is NOT a data type.
- It is just compile-time type inference.
- The compiler figures out the type at compile time and locks it.
```
        var number = 10;
        var name = "Jaith";
        var isActive = true;
```
Compiler converts this to:
```
        int number = 10;
        string name = "Jaith";
        bool isActive = true;

        🚫 You must initialize var
        var x;      // ❌ Compile-time error

        🔍 Behavior of var
        var x = 10;
        x = "hello";    // ❌ Compile-time error. Because x is int, not flexible.
```

**2. dynamic keyword (Run-time typing)**
- ✔ Type checking is done at runtime, not compile time.
```
        dynamic value = 10;
        value = "hello";
        value = true;
```
- ✔ Compiles successfully
- ✔ Type changes at runtime

⚠ Runtime Error Example:
```
        dynamic x = 10;
        Console.WriteLine(x.Length);   // ❌ Runtime error
        Compiler allows it, but crashes at runtime:
        RuntimeBinderException: 'int' does not contain a definition for 'Length'
```


**4. Important Interview Trick**
- What is the type of var at runtime?
```
        var x = 10;
        Console.WriteLine(x.GetType());
```
- output: System.Int32
- ➡️ var is fully typed, just hidden.


**When to use var**
- ✔ LINQ queries
- ✔ Anonymous types
- ✔ Long generic types
- ✔ Improves readability (when RHS is obvious)
```
        var users = dbContext.Users
                     .Where(u => u.IsActive)
                     .Select(u => new { u.Name, u.Email })
                     .ToList();
```

<br>
<br>

is .net webapi is a multithreading app if so explain
----------------------------------------------------------
Yes, .NET Web API is a multithreaded application by design. 
- By default, the framework handles multiple incoming requests concurrently using a Thread Pool, which means you do not have to manually create threads for every new user request. 

**How it Works:** <br>
- Thread Pool Management: When an HTTP request reaches the server, the .NET runtime retrieves an available thread from a managed ThreadPool to process that specific request.
- Request Independence: Each request is handled independently on its own thread. This allows the server to process many requests at once rather than one after another.
- Scalability: The runtime automatically manages the size of this thread pool based on the workload and available CPU cores. 

**Multithreading vs. Asynchronous Programming:** <br>
While Web API is multithreaded, modern .NET development heavily relies on the Async/Await pattern to improve how these threads are used: 
- Non-blocking I/O: When your code uses await for a database call or file operation, the thread handling that request is released back to the pool to handle other incoming requests while waiting for the operation to finish.
- Efficiency: This allows a small number of threads to handle thousands of concurrent requests, preventing "thread pool exhaustion" where the server runs out of available threads. 

**Key Considerations**
- Thread Safety: Because multiple threads may execute your code at the same time, any shared state (like static variables or shared caches) must be handled carefully using synchronization techniques like lock to avoid race conditions.
- Scoped Services: In most Web API projects, objects like database contexts are "scoped" to a single request, meaning they aren't shared across different threads by default, which simplifies thread safety

What is difference between IConfiguration & IOptions in .NET Core ?
-----------------------------------------------------------



Difference between Dependency Injection and Dependency Inversion:
-----------------------------------------------------------------


What is Shallow copy and deep copy in C#:
--------------------------------------------------


Explain Caching strategies in .NET
--------------------------------------------------------------------

Questions on Caching, Multi-Threading, Events and Delegates, deadlock, race conditions,
--------------------------------------------------------------------

Scalability, Idempotancy, Concurrency
-----------------------------------------------

What is the difference between String and StringBuilder in C#;
------------------------------------------------------------
- String is immutable,
    - meaning once it is created, its value cannot be changed. Any modification (like appending or replacing) creates a completely new string object in memory and discards the old one.
- StringBuilder is mutable,
    - meaning it can be modified in-place without creating a new object. It uses an internal buffer to handle modifications efficiently.
```
using System;
using System.Text;

class Program {
    static void Main() {
        // String Example: Each '+' creates a NEW string object
        string s = "Hello";
        s += " World";
        s += "!"; 
        Console.WriteLine(s); // Output: Hello World!

        // StringBuilder Example: Modifies the SAME object
        StringBuilder sb = new StringBuilder("Hello");
        sb.Append(" World");
        sb.Append("!");
        Console.WriteLine(sb.ToString()); // Output: Hello World!
    }
}
```

what is difference between static, const, readonly variables:
-------------------------------------------------------
- const are static by default
```
public class Example
{
    // Constant: Must be set here, never changes.
    public const string AppName = "MySoftware";

    // Static: Shared by all "Example" objects.
    public static int InstanceCount = 0;

    // Readonly: Set once (here or in constructor), then locked.
    public readonly DateTime CreatedAt;

    public Example()
    {
        CreatedAt = DateTime.Now; // Set at runtime
        InstanceCount++;          // Update shared static variable
    }
}
```

Difference between the Equality Operator (==) and Equals() Method in C#:
-----------------------------------------------------------------
- Equals() Checks for actual value
- == Checks for reference if both objects have same reference

<br>

what is boxing and Unboxing in C#:
------------------------------------
1) Boxing is converting value types to Reference Type.
```
int myNumber = 10;          // Value type on the Stack
object boxedObj = myNumber; // Boxing: int is moved to the Heap
```

2) Unboxing is converting Reference Type to value type.
```
object boxedObj = 10;       // Boxed value
int unboxedNum = (int)boxedObj; // Unboxing: requires an explicit cast
```
|Feature 	  | Boxing	| Unboxing|
|--------------|---------|---------|
|Direction  |Value Type to Object	| Object to Value Type|
|Conversion | Implicit (automatic) | Explicit (requires casting)|
|Memory	  |Moves data from Stack to Heap |	Moves data from Heap to Stack|
|Performance| Very expensive (allocates memory) | Less expensive (requires type checking)|



<br>
<br>

Difference between IEnumerable and ICollection
--------------------------------------------------------
In C#, the primary difference is that IEnumerable is for reading and iterating through a collection, while ICollection is for modifying that collection. ICollection inherits from IEnumerable, so it includes all iteration capabilities but adds members for counting, adding, and removing items. 

Key Differences at a Glance
Feature    |	IEnumerable<T>	                 |ICollection<T> |
-----------|------------------------------------|--------------- |
Purpose	  |Read-only iteration (forward-only).|	Iteration + Modification. |
Hierarchy	|Base interface for all collections.|	Inherits from IEnumerable. |
Key Members	|GetEnumerator().	                  |Add(), Remove(), Clear(), Count, Contains(). |
Modification|	No (cannot add/remove).        |	Yes. |
Lazy Loading|	Supports deferred/lazy execution.|	Generally represents data already in memory. |










