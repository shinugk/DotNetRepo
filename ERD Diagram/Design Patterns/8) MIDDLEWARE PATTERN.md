 - Middleware is a pipeline pattern where an HTTP request flows through a sequence of components, and each component:
     - ✔ Can process the request
     - ✔ Can modify the response
     - ✔ Can pass control to the next middleware
     - ✔ Can short-circuit (stop) the pipeline
 
 - In simple terms: Middleware = Filters that run before/after your controller.
 - Every request in ASP.NET Core Web API passes through this pipeline before reaching your controller.



 VISUAL DIAGRAM (CONCEPTUALLY)
 ```
             Request
                ↓
             [Middleware 1]
                ↓
             [Middleware 2]
                ↓
             [Middleware 3]
                ↓
             [Controller Endpoint]
                ↓
             (Response comes back through middleware in reverse)
```
 Example: 
 - Logging → Authentication → Routing → Controller → Response → Logging




 🧱 WHY MIDDLEWARE PATTERN?
 -------------------------------------
 - Use middleware for cross-cutting concerns:
     - Logging
     - Authentication
     - Authorization
     - CORS
     - Exception handling
     - Rate limiting
     - Routing
     - Response compression
     - Request validation
     - Caching
 - Middleware applies to every request, unlike per-controller filters.




 🧩 MIDDLEWARE STRUCTURE (SIMPLE)
 ------------------------------------------------------------
 - A middleware is a class with:
     - A constructor that receives RequestDelegate next
     - An Invoke or InvokeAsync method that handles requests




 🛠 EXAMPLE 1: CUSTOM LOGGING MIDDLEWARE
 ------------------------------------------------------------------
 STEP 1 — CREATE MIDDLEWARE
 ```
 public class LoggingMiddleware
 {
     private readonly RequestDelegate _next;

     public LoggingMiddleware(RequestDelegate next)
     {
         _next = next;
     }

     public async Task InvokeAsync(HttpContext context)
     {
         Console.WriteLine($"➡ Incoming Request: {context.Request.Method} {context.Request.Path}");

         await _next(context);  // Call next middleware

         Console.WriteLine($"⬅ Response Status: {context.Response.StatusCode}");
     }
 }
```
 -----------------------------------------------------------------------
 STEP 2 — REGISTER IN PIPELINE
 - Program.cs:
     - `app.UseMiddleware<LoggingMiddleware>();`
 - Now every request logs input and output automatically.




 🛠 EXAMPLE 2: EXCEPTION HANDLING MIDDLEWARE
 ------------------------------------------------------------------------------
 ```
 public class ExceptionHandlingMiddleware
 {
     private readonly RequestDelegate _next;
     private readonly ILogger<ExceptionHandlingMiddleware> _logger;

     public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
     {
         _next = next;
         _logger = logger;
     }

     public async Task InvokeAsync(HttpContext context)
     {
         try
         {
             await _next(context);
         }
         catch (Exception ex)
         {
             _logger.LogError(ex, "Unhandled exception occurred");
             context.Response.StatusCode = 500;
             await context.Response.WriteAsync("Something went wrong!");
         }
     }
 }
```
 ------------------------------------------------------------------------------
 Register:
     `app.UseMiddleware<ExceptionHandlingMiddleware>();`





 🧠 ORDER MATTERS!
 --------------------------------------------
 - Middleware is executed in the order you register them.
```
     app.UseCors();
     app.UseAuthentication();
     app.UseAuthorization(); // must come after authentication
     app.UseMiddleware<LoggingMiddleware>();
     app.MapControllers();
```
 - If you change the order → application behaves differently.




 🕹 HOW MIDDLEWARE PATTERN WORKS INTERNALLY?
 ---------------------------------------------
 - All middleware forms a chain of delegates
 - .NET composes them like:
     - `middleware1 → middleware2 → middleware3 → endpoint`
 - Each middleware method:
     - `await _next(context);`
  passes the request forward.



 ⭐ BUILT-IN MIDDLEWARE EXAMPLES IN ASP.NET CORE
 -----------------------------------------------------------------
 ASP.NET Core uses middleware everywhere:
 | Middleware               | Purpose                  |
 | ------------------------ | ------------------------ |
 | `UseRouting`             | Matches routes           |
 | `UseAuthentication`      | Validates tokens/cookies |
 | `UseAuthorization`       | Checks user permissions  |
 | `UseCors`                | CORS handling            |
 | `UseHttpsRedirection`    | Redirect HTTP → HTTPS    |
 | `UseStaticFiles`         | Serve static content     |
 | `UseResponseCompression` | Compress responses       |

 These are all middleware implementing this pattern.


 🎯 MIDDLEWARE VS FILTERS (IMPORTANT)
 -----------------------------------------------------------------------------------
 | Feature               | Middleware             | Filters                        |
 | --------------------- | ---------------------- | ------------------------------ |
 | Runs on every request | ✔ Yes                  | ❌ Controller/Action level only |
 | Runs before routing   | ✔ Yes                  | ❌ No                           |
 | Runs after routing    | ✔ Yes                  | ✔ Yes (but inside MVC)         |
 | Best for              | Cross-cutting concerns | Controller-specific logic      |
 | Examples              | Auth, CORS, Logging    | Validation, Action filters     |




 🔚 SUMMARY
 ---------------------------------------------------------------------
 - ✔ Middleware is a pipeline of components
 - ✔ Every request flows through the pipeline before reaching controllers
 - ✔ Used for cross-cutting features (auth, logging, exceptions, etc.)
 - ✔ Highly extensible and follows the Chain of Responsibility Pattern
 - ✔ Order matters
 - ✔ Easy to create custom middleware in Web API
