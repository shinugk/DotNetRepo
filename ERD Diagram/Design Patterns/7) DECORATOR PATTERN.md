 - Category: Structural
 - Description: Dynamically adds responsibilities to objects in a flexible and reusable way without modifying their code.
 - Why Important: Useful for extending functionality (e.g., adding logging, caching, or validation) in a modular way, especially with .NET Core’s DI and middleware.
 - Use Cases: 
     - Adding logging or metrics to services.
     - Enhancing API responses with additional data.
--------------------------------------------------------------
 - ✔ Adds new behavior to an object dynamically (at runtime)
 - ✔ Without changing the original class
 - ✔ By wrapping the object inside another object (decorator)
-------------------------------------------------------------------


 🧠 WHY DO WE USE DECORATOR PATTERN?
 ------------------------------------------------------
 - Use it when you want to add functionality like:
     - Logging
     - Caching
     - Validation
     - Authentication
     - Compression
     - Encryption
 - without modifying the original class.
 - This helps follow Open–Closed Principle → code should be open for extension but closed for modification.


 DECORATOR PATTERN IN C# .NET WEB API
 ---------------------------------------------------------
 - In a Web API, the Decorator Pattern is mainly used when you want to add extra behavior to a service without modifying its original code.


 📌 REAL-TIME EXAMPLE: LOGGING DECORATOR FOR A SERVICE
 -------------------------------------------------------------------------
 Imagine you have a ProductService in Web API:
 ```
 public interface IProductService
 {
     Task<Product> GetProduct(int id);
 }
```
 -------------------------------------------------------------------------
 Simple implementation:
 ```
 public class ProductService : IProductService
 {
     public async Task<Product> GetProduct(int id)
     {
         // Imagine DB call here
         return new Product { Id = id, Name = "Laptop" };
     }
 }
```
 ---------------------------------------------------------------------------
 - You now want to add logging, but you do NOT want to modify ProductService itself.
 - This is where the Decorator Pattern fits perfectly.
 -------------------------------------------------------------------------
 
 🧱 STEP 1 — CREATE DECORATOR CLASS
 ```
 public class LoggingProductServiceDecorator : IProductService
 {
     private readonly IProductService _inner;
     private readonly ILogger<LoggingProductServiceDecorator> _logger;

     public LoggingProductServiceDecorator(IProductService inner, 
                                           ILogger<LoggingProductServiceDecorator> logger)
     {
         _inner = inner;
         _logger = logger;
     }

     public async Task<Product> GetProduct(int id)
     {
         _logger.LogInformation($"Getting product with ID: {id}");

         var result = await _inner.GetProduct(id);

         _logger.LogInformation($"Product fetched: {result.Name}");

         return result;
     }
 }
```
 ------------------------------------------------------------------------------
 - Here:
   - _inner is the original service
   - Decorator adds logging before and after the main job
 ------------------------------------------------------------------------------
 🧱 STEP 2 — REGISTER IN DI (PROGRAM.CS)
 - You replace the original implementation with the decorated one:
```
     builder.Services.AddScoped<IProductService, ProductService>();
     builder.Services.Decorate<IProductService, LoggingProductServiceDecorator>();
```
 - If .Decorate() is not available by default, you install:
```
     dotnet add package Scrutor
```
 - Then use:
```
     builder.Services.Decorate<IProductService, LoggingProductServiceDecorator>();
```
 - ✔ Now every time IProductService is injected, the decorator is used, wrapping the original.
 ----------------------------------------------------------------------------------
 STEP 3 — CONTROLLER USES THE DECORATED SERVICE
 ```
 [ApiController]
 [Route("api/[controller]")]
 public class ProductsController : ControllerBase
 {
     private readonly IProductService _service;

     public ProductsController(IProductService service)
     {
         _service = service;
     }

     [HttpGet("{id}")]
     public async Task<IActionResult> Get(int id)
     {
         var product = await _service.GetProduct(id);
         return Ok(product);
     }
 }
```
 -------------------------------------------------------------------------------
 - The controller doesn't know about decoration.
 - It just calls the service normally.
 - But behind the scenes:
     - Controller → LoggingProductServiceDecorator → ProductService
 -------------------------------------------------------------------------------
 📌 Output Example (Logs):
 - Getting product with ID: 5
 - Product fetched: Laptop




 🧠 WHY DECORATOR IS USEFUL IN WEB API?
 ------------------------------------------------------------------------------------------
 | Feature                                    | Benefit                                   |
 | ------------------------------------------ | ----------------------------------------- |
 | **Add behavior without modifying service** | No risk of breaking existing logic        |
 | **Works with Dependency Injection**        | Wrap automatically via DI                 |
 | **Reusable behavior**                      | Logging, caching, retry, etc.             |
 | **Cleaner code**                           | No cross-cutting concerns inside services |
 | **Follow Open–Closed Principle**           | Extend without modifying original code    |
