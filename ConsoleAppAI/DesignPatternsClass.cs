using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppAI
{
    public class DesignPatternsClass
    {
        public void Content()
        {

            #region Categories of Design patterns
            /*
            1) CREATIONAL PURPOSE
                * How and when objects are created
                a) Factory Method
                b) Abstract Factory
                c) Builder
                d) Prototype
                e) Singleton

            2) STRUCTURAL PURPOSE
                * How classes and objects relate
                a) Adapter
                b) Bridge
                c) Composite
                d) Flyweight
                e) Decorator
                f) Facade
            
            3) BEHAVIORAL PURPOSE
                * Define how interactions are handled between classes and objects
                a) Iterator
                b) Interpretor
                c) Command
                d) Observer
                e) Template Method
                f) State
                g) Chain of Responsibilty
                h) Mediator
                i) Strategy
                j) Memento
                k) Visitor
            
            4) CONCURRENCY PURPOSE
                * Handling concurrent actions
                                      
             */
            #endregion

            #region 1) DEPENDENCY INJECTION PATTERN
            /*
            - Category: Modern / Architectural
            
            - Description: Provides dependencies to objects externally(via constructor, method, or property injection) 
                           rather than having objects create their own, promoting loose coupling and testability.

            - Why Important: Built into ASP.NET Core’s DI container(IServiceCollection), 
                             it’s fundamental for managing services, repositories, and other dependencies in WebAPIs and microservices.
            - Use Cases: 
                Injecting services(e.g., database contexts, logging) into controllers or services.
                Enabling unit testing by mocking dependencies.




            🚫 WITHOUT DI(TIGHT COUPLING):-
            ----------------------------------------------------
            public class EmailService
            {
                public void SendEmail() 
                {
                    Console.WriteLine("Email sent");
                }
            }

            public class Notification
            {
                private EmailService _emailService = new EmailService();   <------// ❌ Hard dependency

                public void Notify()
                {
                    _emailService.SendEmail();
                }
            }
            -------------------------------------------------------
            Problems:
            - Class creates its own dependency (new EmailService()).
            - Hard to test (cannot mock EmailService).
            - Hard to replace with SMSService tomorrow.
            - Hard to maintain.




            ✅ WITH DI (LOOSE COUPLING)
            --------------------------------------------------------
            Step 1: Create an Interface (Abstraction)

            public interface IEmailService
            {
                void SendEmail();
            }

            public class EmailService : IEmailService
            {
                public void SendEmail()
                {
                    Console.WriteLine("Email sent");
                }
            }
            ------------------------------------------------------
            Step 2: Inject Dependency

            public class Notification
            {
                private readonly IEmailService _emailService;

                public Notification(IEmailService emailService) // <---- INJECT HERE
                {
                    _emailService = emailService;
                }

                public void Notify()
                {
                    _emailService.SendEmail();
                }
            }
            --------------------------------------------------------



            🌟 How DI Works in ASP.NET Core
            - DI is built-in. You register dependencies in Program.cs:
                    builder.Services.AddScoped<IEmailService, EmailService>();
            - ASP.NET Core automatically provides EmailService wherever IEmailService is needed.



            ✨ Example in Controller
            ---------------------------------------------------------------
            [ApiController]
            [Route("api/[controller]")]
            public class NotificationController : ControllerBase
            {
                private readonly IEmailService _emailService;

                public NotificationController(IEmailService emailService) // <----INJECTED BY DI CONTAINER
                {
                    _emailService = emailService;
                }

                [HttpGet("send")]
                public IActionResult Send()
                {
                    _emailService.SendEmail();
                    return Ok("Email sent");
                }
            }
            -----------------------------------------------------------------------
          

            🎯 Why Dependency Injection is Important?
            | Benefit                   | Explanation                                        |
            | ------------------------  | -------------------------------------------------- |
            | ✔ Loose Coupling         | Classes depend on interfaces, not concrete classes |
            | ✔ Easy Testing           | You can mock dependencies in unit tests            |
            | ✔ Better Maintainability | Change one service without touching others         |
            | ✔ Follows SOLID → DIP    | Dependency Inversion Principle                     |
            | ✔ Built-in container     | ASP.NET Core supports DI natively                  |


            🏗️ Types of Dependency Injection
            | Type                      | Description                                        |
            | ------------------------- | -------------------------------------------------- |
            | Constructor Injection     | Inject via class constructor (most common in .NET) |
            | Method Injection          | Inject dependency as a method parameter            |
            | Property Injection        | Inject via property (rarely used)                  |


            🧱 Service Lifetime in DI
            | Lifetime      | Meaning                    | Example          |
            | ------------- | -------------------------- | ---------------- |
            | Singleton     | One instance for whole app | `AddSingleton<>` |
            | Scoped        | One instance per request   | `AddScoped<>`    |
            | Transient     | New instance every time    | `AddTransient<>` |


            builder.Services.AddScoped<IEmailService, EmailService>();       <---PROGRAM.CS WE HAVE TO REGISTER

            */
            #endregion

            #region 2) SINGLETON PATTERN
            /*
            - Category: Creational
            - Description: Ensures that ✔ Only one instance of a class is created
                                        ✔ The instance is shared globally
                                        ✔ The object is created only once (lazy or eager)
            - This is used when you need exactly one object for the entire application's lifetime.
            - Why Important: Used for managing shared resources like configuration managers, loggers, or caches in .NET Core applications.
                             The DI container supports singleton lifetime scopes.
            - Use Cases: 
                 Logging services (e.g., ILogger).
                 Configuration or connection pool management.



            🚫 WITHOUT SINGLETON (MULTIPLE INSTANCES)
            ------------------------------------------------
            var logger1 = new Logger();
            var logger2 = new Logger();  // Creates another instance
            -------------------------------------------------
            Each time you create an object, memory is allocated again.


            ✅ WITH SINGLETON (SINGLE SHARED OBJECT)
            You restrict object creation and provide a single, global access point.
            ------------------------------------------------------------------------
                🧱 IMPLEMENTATION #1 — CLASSIC SINGLETON (THREAD-SAFE)
                public sealed class Logger
                {
                    private static readonly Logger _instance = new Logger();

                    // Private constructor → prevents external instantiation
                    private Logger() { }

                    public static Logger Instance
                    {
                        get
                        {
                            return _instance;
                        }
                    }

                    public void Log(string message)
                    {
                        Console.WriteLine(message);
                    }
                }

                Usage:
                Logger.Instance.Log("Hello");       <--Only one Logger instance exists.
            ----------------------------------------------------------------------------

                🧱 IMPLEMENTATION #3 — USING .NET DEPENDENCY INJECTION (ASP.NET CORE)
                - In ASP.NET Core, you never manually write Singletons.
                - Instead, you register them in DI:
                      builder.Services.AddSingleton<ILogService, LogService>();          <------ register in Program.cs

                The framework ensures:
                - Only one instance of LogService exists
                - Same instance is injected everywhere

                Example:
                public class HomeController : ControllerBase
                {
                    private readonly ILogService _log;

                    public HomeController(ILogService log)
                    {
                        _log = log;  // <------- same instance for entire application
                    }
                }

            ---------------------------------------------------------------------------------




            🔍 WHEN SHOULD YOU USE SINGLETON?
            | Use Case                          | Why Singleton Works                |
            | --------------------------------- | ---------------------------------- |
            | Logging                           | One logger instance for entire app |
            | Configuration / Settings          | Settings are global & shared       |
            | Caching service                   | One central memory cache           |
            | Database connection pool          | Shared connection management       |
            | Telemetry, Monitoring Service     | Single global instance             |




            🔚 SUMMARY
            ✔ Singleton Pattern ensures only one instance exists
            ✔ Prevents object creation using private constructor
            ✔ Provides global access
            ✔ Use Lazy<T> for thread-safe lazy creation
            ✔ In ASP.NET Core, prefer AddSingleton() instead of manual Singleton
            ✔ Use for shared global services like logging, caching, configuration

            */
            #endregion

            #region 3) REPOSITORY PATTERN
            /*
            - Category: Modern/Architectural
            - Description: Encapsulates data access logic, providing a collection-like interface for accessing domain objects, abstracting the data layer.
                            ✔ Separates data-access logic from business logic
                            ✔ Provides a clean abstraction over data operations
                            ✔ Makes code easier to maintain, test, and mock
                            ✔ Avoids repeating CRUD logic everywhere
            - Why Important: Widely used with Entity Framework Core to simplify database operations, improve testability, and separate business logic from data access.
            - Use Cases: 
                CRUD operations in WebAPIs.
                Abstracting database interactions in microservices.

            - In simple words:
            Repository acts as a middle layer between your application and the database.
            Your services/controllers talk to repository instead of directly talking to EF Core or SQL.



            🧱 WITHOUT REPOSITORY (TIGHTLY COUPLED CODE)
            ---------------------------------------------------------------------
            public class ProductService
            {
                private readonly AppDbContext _context;

                public ProductService(AppDbContext context)
                {
                    _context = context;
                }

                public List<Product> GetProducts()
                {
                    return _context.Products.ToList();     // <------------ DIRECT EF CORE CALL
                }
            }
            --------------------------------------------------------------------------
            Problems:
            - Business layer knows EF Core details
            - Hard to unit test (DbContext is not mock-friendly)
            - No central place for queries



            🧱 WITH REPOSITORY (LOOSE COUPLING)
            - You create a repository interface + class that handles all DB operations.
            - Service layer uses repository instead of DbContext.
            ------------------------------------------------------------------------
            ✔ STEP 1: CREATE MODEL
            public class Product
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public double Price { get; set; }
            }

            ------------------------------------------------------------------------
            ✔ STEP 2: CREATE REPOSITORY INTERFACE
            public interface IProductRepository
            {
                Task<IEnumerable<Product>> GetAll();
                Task<Product> GetById(int id);
                Task Add(Product product);
                Task Update(Product product);
                Task Delete(int id);
            }

            -----------------------------------------------------------------------
            ✔ STEP 3: IMPLEMENT REPOSITORY
            public class ProductRepository : IProductRepository
            {
                private readonly AppDbContext _context;

                public ProductRepository(AppDbContext context)
                {
                    _context = context;
                }

                public async Task<IEnumerable<Product>> GetAll()
                {
                    return await _context.Products.ToListAsync();
                }

                public async Task<Product> GetById(int id)
                {
                    return await _context.Products.FindAsync(id);
                }

                public async Task Add(Product product)
                {
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }

                public async Task Update(Product product)
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }

                public async Task Delete(int id)
                {
                    var product = await _context.Products.FindAsync(id);
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }

            -------------------------------------------------------------------------
            ✔ STEP 4: REGISTER IN DI (PROGRAM.CS)
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddDbContext<AppDbContext>();
    
            -------------------------------------------------------------------------
            ✔ STEP 5: USE REPOSITORY IN CONTROLLER / SERVICES
            [ApiController]
            [Route("api/[controller]")]
            public class ProductsController : ControllerBase
            {
                private readonly IProductRepository _repo;

                public ProductsController(IProductRepository repo)
                {
                    _repo = repo;
                }

                [HttpGet]
                public async Task<IActionResult> Get()
                {
                    return Ok(await _repo.GetAll());
                }
            }

            -----------------------------------------------------------------------
            Now your controller is clean and loosely coupled.
            ------------------------------------------------------------------------



            🎯 WHY USE THE REPOSITORY PATTERN?
            | Benefit                         | Explanation                                        |
            | ------------------------------  | -------------------------------------------------- |
            | ✔ Loosely Coupled              | Business layer does not depend on EF Core          |
            | ✔ Easy Unit Testing            | You can mock the repository                        |
            | ✔ Clean Separation of Concerns | DB code and business logic are separated           |
            | ✔ Reusable                     | CRUD logic reused across controllers               |
            | ✔ Helps follow SOLID           | Especially the **Single Responsibility Principle** |



            ❌ WHEN NOT TO USE REPOSITORY PATTERN?
            - When using EF Core, which is already a repository/unit-of-work implementation
            - When abstraction becomes unnecessary and adds extra layers
            - For very complex queries where EF DbContext is easier to use directly



            🔚 SUMMARY
            ✔ Repository is a middle layer between your app and DB
            ✔ Improves testability, clean architecture, loose coupling
            ✔ Uses interfaces and DI
            ✔ Keeps controllers/service layers clean
            ✔ Optional: use generic repositories


            */
            #endregion

            #region 4) FACTORY METHOD PATTERN
            /*
            Factory Method is a creational design pattern that delegates object creation to subclasses or a dedicated method.
            Instead of using new everywhere, you create objects using a factory method.
            👉 Acceptable when:
            - You want to centralize object creation
            - you want to create different types based on conditions
            - You want to avoid tightly coupling your code with concrete classes

            📌 EXAMPLE: FACTORY METHOD (SIMPLE FACTORY)
            ----------------------------------------------------------------
            Step 1: Create Interface / Base Class
            public interface IShape
            {
                void Draw();
            }

            public class Circle : IShape
            {
                public void Draw() => Console.WriteLine("Drawing Circle");
            }

            public class Square : IShape
            {
                public void Draw() => Console.WriteLine("Drawing Square");
            }

            -------------------------------------------------------------------
            Step 2: Factory Method
            public static class ShapeFactory
            {
                public static IShape GetShape(string type)
                {
                    return type.ToLower() switch
                    {
                        "circle" => new Circle(),
                        "square" => new Square(),
                        _ => throw new Exception("Invalid shape")
                    };
                }
            }

            ------------------------------------------------------------------
            Usage:
            IShape shape = ShapeFactory.GetShape("circle");
            shape.Draw();
            -----------------------------------------------------------------
            ✔ Centralized object creation
            ✔ No need to modify multiple places if logic changes

          */
            #endregion

            #region 5) ABSTRACT FACTORY PATTERN
            /*
            - The Abstract Factory pattern creates related objects without specifying their concrete classes.
            - It is a factory that creates other factories.

            Used when:
            - You want to create families of related objects
            - You want each family to follow the same theme or rules

             

            📦 REAL-WORLD EXAMPLE: UI THEMES (LIGHT THEME, DARK THEME)
            Imagine you want two UI themes:
                Light Theme: Light Button, Light Checkbox
                Dark Theme: Dark Button, Dark Checkbox
            The client should NOT know which concrete classes are being used.

            ----------------------------------------------------------------
            STEP 1: PRODUCT INTERFACES
            public interface IButton
            {
                void Render();
            }

            public interface ICheckbox
            {
                void Check();
            }

            -----------------------------------------------------------------
            STEP 2: CONCRETE PRODUCTS
            public class LightButton : IButton
            {
                public void Render() => Console.WriteLine("Light Button");
            }

            public class DarkButton : IButton
            {
                public void Render() => Console.WriteLine("Dark Button");
            }

            public class LightCheckbox : ICheckbox
            {
                public void Check() => Console.WriteLine("Light Checkbox");
            }

            public class DarkCheckbox : ICheckbox
            {
                public void Check() => Console.WriteLine("Dark Checkbox");
            }

            -------------------------------------------------------------
            STEP 3: ABSTRACT FACTORY
            public interface IUIFactory
            {
                IButton CreateButton();
                ICheckbox CreateCheckbox();
            }

            -----------------------------------------------------------------
            STEP 4: CONCRETE FACTORIES
            public class LightThemeFactory : IUIFactory
            {
                public IButton CreateButton() => new LightButton();
                public ICheckbox CreateCheckbox() => new LightCheckbox();
            }

            public class DarkThemeFactory : IUIFactory
            {
                public IButton CreateButton() => new DarkButton();
                public ICheckbox CreateCheckbox() => new DarkCheckbox();
            }

            ------------------------------------------------------------------
            STEP 5: CLIENT CODE (USES FACTORY WITHOUT KNOWING WHICH OBJECTS IT CREATES)
            public class Application
            {
                private readonly IButton _button;
                private readonly ICheckbox _checkbox;

                public Application(IUIFactory factory)
                {
                    _button = factory.CreateButton();
                    _checkbox = factory.CreateCheckbox();
                }

                public void Render()
                {
                    _button.Render();
                    _checkbox.Check();
                }
            }

            -------------------------------------------------------------------
            Usage:
            IUIFactory factory = new DarkThemeFactory();
            var app = new Application(factory);
            app.Render();

            ----------------------------------------------------------------------

            NOTE:
            ✔ Creates consistent families of objects
            ✔ Client does NOT know the concrete classes
            ✔ Easy to switch themes (just change factory)


            🎯 WHEN TO USE ABSTRACT FACTORY?
            Use it when:
            - You have multiple related products
            - You want to ensure compatibility
                (e.g., LightButton must match LightCheckbox)
            - You want to switch families easily (Light → Dark)

             */
            #endregion

            /*
            FACTORY METHOD VS ABSTRACT FACTORY
            | Feature    | Factory Method                | Abstract Factory                |
            | ---------- | ----------------------------- | ------------------------------- |
            | Creates    | One object                    | Many related objects            |
            | Structure  | One factory method            | Factory of factories            |
            | Use Case   | Decide which object to create | Create related object families  |
            | Example    | ShapeFactory → Circle/Square  | ThemeFactory → Button, Checkbox |
            | Complexity | Simple                        | More complex                    |

             */

            #region 6) STRATEGY PATTERN
            /*
            - Category: Behavioral
            - Description: Defines a family of algorithms, encapsulates each one, and makes them interchangeable at runtime.
            - Why Important: Enables runtime selection of behaviors, which is useful for implementing features like payment processing, sorting algorithms, or validation rules.
            - Use Cases: 
                Selecting payment providers(e.g., PayPal, Stripe).
                Dynamic validation or business rule application.

            In simple words:
            Strategy Pattern lets you switch algorithms (behavior) at runtime.


            🧠 WHY USE STRATEGY PATTERN?
            Use it when:
            - You have multiple ways of doing the same task
            - You want to avoid long if/else or switch statements
            - You want to follow the Open-Closed Principle (OCP) — add new strategies without modifying existing code


            📦 REAL-LIFE ANALOGY
            Think of payment options at checkout:
                - Pay by UPI
                - Pay by Credit Card
                - Pay by Cash
            The checkout process is the same, only the payment method changes.
            This is exactly how Strategy Pattern works.



            --------------------------------------------------------------
            🧱 1. Define the Strategy Interface
            public interface IPaymentStrategy
            {
                void Pay(double amount);
            }

            ----------------------------------------------------------------
            🛠 2. Concrete Strategy Classes 
            ------Payment by UPI:----------
            public class UpiPayment : IPaymentStrategy
            {
                public void Pay(double amount)
                {
                    Console.WriteLine($"Paid {amount} using UPI");
                }
            }

            -------Payment by Credit Card:-------
            public class CreditCardPayment : IPaymentStrategy
            {
                public void Pay(double amount)
                {
                    Console.WriteLine($"Paid {amount} using Credit Card");
                }
            }

            ---------Payment by Cash:---------
            public class CashPayment : IPaymentStrategy
            {
                public void Pay(double amount)
                {
                    Console.WriteLine($"Paid {amount} in Cash");
                }
            }

            -------------------------------------------------------------------
            🧩 3. Context Class (uses a strategy)
            public class PaymentService
            {
                private IPaymentStrategy _paymentStrategy;

                public void SetPaymentStrategy(IPaymentStrategy strategy)
                {
                    _paymentStrategy = strategy;
                }

                public void MakePayment(double amount)
                {
                    _paymentStrategy.Pay(amount);
                }
            }

            ---------------------------------------------------------------------
            🚀 4. Usage (Switch strategy at runtime)
            var paymentService = new PaymentService();

            paymentService.SetPaymentStrategy(new UpiPayment());
            paymentService.MakePayment(500);

            paymentService.SetPaymentStrategy(new CreditCardPayment());
            paymentService.MakePayment(1000);

            ----------------------------------------------------------------------
            Output:
            Paid 500 using UPI
            Paid 1000 using Credit Card





            ⭐ BENEFITS OF STRATEGY PATTERN
            | Benefit                             | Explanation                                  |
            | ----------------------------------- | -------------------------------------------- |
            | **Removes if-else complexity**      | No long conditional blocks                   |
            | **Open-Closed Principle**           | Add new strategies without touching old code |
            | **Easily interchangeable behavior** | Change algorithms at runtime                 |
            | **Better Testing**                  | Each strategy can be unit-tested separately  |
            | **Cleaner and maintainable code**   | Behavior is separated into small classes     |



            ⚠️ WHEN NOT TO USE STRATEGY PATTERN
            Avoid this pattern when:
            - You only have one behavior (no variations)
            - Variations are very small and do not require full separate classes
            - Too many small strategy classes make the design over-complicated


            📌 Real .NET Examples of Strategy Pattern
            1️ ASP.NET Core Authentication
                Different authentication schemes (JWT, Cookies, OAuth) are strategies.
            2️ Sorting algorithms
                You can switch between QuickSort, MergeSort, BubbleSort strategies.
            3️ Compression algorithms
                Zip vs Rar vs Tar compression strategies.
            */
            #endregion

            #region 7) DECORATOR PATTERN
            /*
            - Category: Structural
            - Description: Dynamically adds responsibilities to objects in a flexible and reusable way without modifying their code.
            - Why Important: Useful for extending functionality (e.g., adding logging, caching, or validation) in a modular way, especially with .NET Core’s DI and middleware.
            Use Cases: 
                Adding logging or metrics to services.
                Enhancing API responses with additional data.

            
            ✔ Adds new behavior to an object dynamically (at runtime)
            ✔ Without changing the original class
            ✔ By wrapping the object inside another object (decorator)



            🧠 WHY DO WE USE DECORATOR PATTERN?
            Use it when you want to add functionality like:
                Logging
                Caching
                Validation
                Authentication
                Compression
                Encryption
            … without modifying the original class.
            This helps follow Open–Closed Principle → code should be open for extension but closed for modification.


            DECORATOR PATTERN IN C# .NET WEB API
            In a Web API, the Decorator Pattern is mainly used when you want to add extra behavior to a service without modifying its original code.



            📌 REAL-TIME EXAMPLE: LOGGING DECORATOR FOR A SERVICE
            -------------------------------------------------------------------------
            Imagine you have a ProductService in Web API:
            public interface IProductService
            {
                Task<Product> GetProduct(int id);
            }

            -------------------------------------------------------------------------
            Simple implementation:
            public class ProductService : IProductService
            {
                public async Task<Product> GetProduct(int id)
                {
                    // Imagine DB call here
                    return new Product { Id = id, Name = "Laptop" };
                }
            }
            ---------------------------------------------------------------------------
            You now want to add logging, but you do NOT want to modify ProductService itself.
            This is where the Decorator Pattern fits perfectly.


            -------------------------------------------------------------------------
            🧱 STEP 1 — CREATE DECORATOR CLASS
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

            ------------------------------------------------------------------------------
            Here:
            _inner is the original service
            Decorator adds logging before and after the main job
            ------------------------------------------------------------------------------
            🧱 STEP 2 — REGISTER IN DI (PROGRAM.CS)
            You replace the original implementation with the decorated one:
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.Decorate<IProductService, LoggingProductServiceDecorator>();
            If .Decorate() is not available by default, you install:
                dotnet add package Scrutor
            Then use:
                builder.Services.Decorate<IProductService, LoggingProductServiceDecorator>();
            ✔ Now every time IProductService is injected, the decorator is used, wrapping the original.

            ----------------------------------------------------------------------------------
            STEP 3 — CONTROLLER USES THE DECORATED SERVICE
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

            -------------------------------------------------------------------------------
            The controller doesn't know about decoration.
            It just calls the service normally.
            But behind the scenes:
                Controller → LoggingProductServiceDecorator → ProductService
            -------------------------------------------------------------------------------
            📌 Output Example (Logs):
            Getting product with ID: 5
            Product fetched: Laptop




            🧠 WHY DECORATOR IS USEFUL IN WEB API?
            | Feature                                    | Benefit                                   |
            | ------------------------------------------ | ----------------------------------------- |
            | **Add behavior without modifying service** | No risk of breaking existing logic        |
            | **Works with Dependency Injection**        | Wrap automatically via DI                 |
            | **Reusable behavior**                      | Logging, caching, retry, etc.             |
            | **Cleaner code**                           | No cross-cutting concerns inside services |
            | **Follow Open–Closed Principle**           | Extend without modifying original code    |




             */
            #endregion

            #region 8) MIDDLEWARE PATTERN
            /*
            - Middleware is a pipeline pattern where an HTTP request flows through a sequence of components, and each component:
                ✔ Can process the request
                ✔ Can modify the response
                ✔ Can pass control to the next middleware
                ✔ Can short-circuit (stop) the pipeline
            
            In simple terms: Middleware = Filters that run before/after your controller.
            - Every request in ASP.NET Core Web API passes through this pipeline before reaching your controller.



            VISUAL DIAGRAM (CONCEPTUALLY)
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
            Example: Logging → Authentication → Routing → Controller → Response → Logging




            🧱 WHY MIDDLEWARE PATTERN?
            - Use middleware for cross-cutting concerns:
                Logging
                Authentication
                Authorization
                CORS
                Exception handling
                Rate limiting
                Routing
                Response compression
                Request validation
                Caching
            - Middleware applies to every request, unlike per-controller filters.




            🧩 MIDDLEWARE STRUCTURE (SIMPLE)
            A middleware is a class with:
                A constructor that receives RequestDelegate next
                An Invoke or InvokeAsync method that handles requests




            🛠 EXAMPLE 1: CUSTOM LOGGING MIDDLEWARE
            ------------------------------------------------------------------
            STEP 1 — CREATE MIDDLEWARE
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

            -----------------------------------------------------------------------
            STEP 2 — REGISTER IN PIPELINE
            Program.cs:
                app.UseMiddleware<LoggingMiddleware>();
            Now every request logs input and output automatically.





            🛠 EXAMPLE 2: EXCEPTION HANDLING MIDDLEWARE
            ------------------------------------------------------------------------------
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
            ------------------------------------------------------------------------------
            Register:
                app.UseMiddleware<ExceptionHandlingMiddleware>();





            🧠 ORDER MATTERS!
            Middleware is executed in the order you register them.
                app.UseCors();
                app.UseAuthentication();
                app.UseAuthorization(); // must come after authentication
                app.UseMiddleware<LoggingMiddleware>();
                app.MapControllers();
            If you change the order → application behaves differently.




            🕹 HOW MIDDLEWARE PATTERN WORKS INTERNALLY?
            - All middleware forms a chain of delegates
            - .NET composes them like:
                middleware1 → middleware2 → middleware3 → endpoint
            - Each middleware method:
                await _next(context);
             passes the request forward.



            ⭐ BUILT-IN MIDDLEWARE EXAMPLES IN ASP.NET CORE
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
            | Feature               | Middleware             | Filters                        |
            | --------------------- | ---------------------- | ------------------------------ |
            | Runs on every request | ✔ Yes                  | ❌ Controller/Action level only |
            | Runs before routing   | ✔ Yes                  | ❌ No                           |
            | Runs after routing    | ✔ Yes                  | ✔ Yes (but inside MVC)         |
            | Best for              | Cross-cutting concerns | Controller-specific logic      |
            | Examples              | Auth, CORS, Logging    | Validation, Action filters     |




            🔚 SUMMARY
            ✔ Middleware is a pipeline of components
            ✔ Every request flows through the pipeline before reaching controllers
            ✔ Used for cross-cutting features (auth, logging, exceptions, etc.)
            ✔ Highly extensible and follows the Chain of Responsibility Pattern
            ✔ Order matters
            ✔ Easy to create custom middleware in Web API


            
             */
            #endregion

            #region 9) OPTIONS PATTERN
            /*
            ✔ Bind configuration settings (from appsettings.json) to strongly-typed C# classes
            ✔ Access configuration safely (with intellisense & compile-time checking)
            ✔ Avoid magic strings like "ConnectionStrings:DefaultConnection" everywhere
            ✔ Centralize configuration values

            In simple words:
            Options Pattern = Read configuration settings into strongly-typed classes.


            📦 EXAMPLE CONFIGURATION (APPSETTINGS.JSON)
            ----------------------------------------------------
            {
              "MyAppSettings": {
                "ApplicationName": "My Web API",
                "Version": "1.0",
                "EnableFeatureX": true
              }
            }

            We want to access these values cleanly inside controllers/services.


            🧱 STEP 1 — CREATE A SETTINGS CLASS
            -------------------------------------------------
            public class MyAppSettings
            {
                public string ApplicationName { get; set; }
                public string Version { get; set; }
                public bool EnableFeatureX { get; set; }
            }
            This class must match the structure in appsettings.json.


            🧱 STEP 2 — REGISTER OPTIONS IN PROGRAM.CS
            ----------------------------------------------
            builder.Services.Configure<MyAppSettings>(
                builder.Configuration.GetSection("MyAppSettings"));

            This tells .NET:
            "Bind the appsettings.json section MyAppSettings to the class MyAppSettings."


            🧱 STEP 3 — INJECT IOPTIONS IN CONTROLLER
            -----------------------------------------------------------
            using Microsoft.Extensions.Options;

            [ApiController]
            [Route("api/[controller]")]
            public class ConfigController : ControllerBase
            {
                private readonly MyAppSettings _settings;

                public ConfigController(IOptions<MyAppSettings> options)
                {
                    _settings = options.Value;   // Access the actual config values
                }

                [HttpGet]
                public IActionResult Get()
                {
                    return Ok(_settings);
                }
            }
            --------------------------------------------------------------
            Output:
            {
              "applicationName": "My Web API",
              "version": "1.0",
              "enableFeatureX": true
            }



            ⭐ VARIANTS OF OPTIONS PATTERN
            ASP.NET Core provides multiple “Options” types:
            | Type                 | Use Case                                                     |
            | -------------------- | ------------------------------------------------------------ |
            | **IOptions**         | Read configuration once at startup                           |
            | **IOptionsSnapshot** | Read configuration per *request* (scoped) — good for Web API |
            | **IOptionsMonitor**  | Real-time change notifications when configuration changes    |



            1) IOptions (Singleton)
            - Loaded once at startup
            - Does not update if config changes
            - Good for static settings like API keys, URLs
            Usage:
                public ConfigController(IOptions<MyAppSettings> options)


            2) IOptionsSnapshot (Scoped per request — best for Web API)
            - Re-evaluates configuration on each web request
            - Useful in development with reload-on-change
            - Only works in scoped services (Web API default)
            Usage:
                public ConfigController(IOptionsSnapshot<MyAppSettings> options)


            3) IOptionsMonitor (Real-time change notifications)
            - Notifies when configuration changes
            - Can react dynamically (like updating feature flags)
            Usage:
                public ConfigController(IOptionsMonitor<MyAppSettings> options)



            🧪 EXAMPLE: USING OPTIONS IN A SERVICE (NOT ONLY CONTROLLERS)
            --------------------------------------------------------------------
            public class FeatureService
            {
                private readonly MyAppSettings _settings;

                public FeatureService(IOptionsSnapshot<MyAppSettings> options)
                {
                    _settings = options.Value;
                }

                public bool IsFeatureXEnabled()
                {
                    return _settings.EnableFeatureX;
                }
            }

            Register your service: 
                builder.Services.AddScoped<FeatureService>();





            ⭐ WHY USE THE OPTIONS PATTERN?
            | Benefit                          | Explanation                               |
            | -------------------------------- | ----------------------------------------- |
            |     Strongly Typed Configuration | Avoids string-based config keys           |
            |     Intellisense Support         | Easier for developers                     |
            |     Centralized Settings         | Cleaner architecture                      |
            |     Supports config reloading    | With IOptionsSnapshot / Monitor           |
            |     Perfect for Web API          | API keys, connection strings, URLs, flags |



            🚀 REAL WEB API USE CASES
            ✔ Using settings for JWT configuration
            ✔ Reading connection strings
            ✔ Feature flags (EnableFeatureX)
            ✔ API keys for third-party services
            ✔ Email configuration (SMTP server, port, username)
            ✔ Paging defaults (PageSize, MaxItems)



            🔚 Summary
            Options Pattern in ASP.NET Core Web API allows you to:
                ✔ Bind config → Strong typed classes
                ✔ Inject config → Through DI using IOptions / Snapshot / Monitor
                ✔ Manage settings cleanly → Without scattering config strings everywhere
            It is one of the best practices for handling configuration in modern .NET applications.


             */
            #endregion

            #region 10) UNIT OF WORK PATTERN
            /*
            The Unit of Work (UoW) pattern:
            ✔ Groups multiple database operations into a single transaction
            ✔ Ensures all operations succeed or none are saved
            ✔ Keeps your repositories coordinated
            ✔ Prevents partial updates if something fails
            ✔ Centralizes SaveChanges() calls

            In simple words:
            Unit of Work = One transaction for all DB actions during a request.

            THIS IS ESPECIALLY IMPORTANT FOR WEB API OPERATIONS THAT MODIFY MULTIPLE TABLES.



            🎯 WHY USE UNIT OF WORK IN WEB API?
            | Problem without UoW                              | How UoW solves it                      |
            | ------------------------------------------------ | -------------------------------------- |
            | Each repository calls `SaveChanges()` separately | One `SaveChanges()` for all operations |
            | Partial updates if one operation fails           | Transaction rollback                   |
            | Scattered DB logic                               | Centralized DB access                  |
            | Hard to maintain / test                          | Clean abstraction layer                |


            📦 TYPICAL WEB API SCENARIO
            Example: Creating an Order also creates OrderItems.
            -------------------------------------------------------
            Without UoW:
                Repository1.SaveChanges()
                Repository2.SaveChanges()
                Repository3.SaveChanges()
            → If 2nd step fails, data becomes inconsistent.
            -------------------------------------------------------
            With UoW:
            Do all operations
            Call Save once
            Transaction happens automatically




            🧱 STEP-BY-STEP IMPLEMENTATION IN C# .NET WEB API

            🧱 1. CREATE REPOSITORIES (SIMPLE EXAMPLE)
            ---------------------------------------------------------------------
            - PRODUCT REPOSITORY:
            public interface IProductRepository
            {
                Task<Product> GetAsync(int id);
                void Add(Product product);
            }

            public class ProductRepository : IProductRepository
            {
                private readonly AppDbContext _context;
                public ProductRepository(AppDbContext context)
                {
                    _context = context;
                }

                public Task<Product> GetAsync(int id)
                {
                    _context.Products.FindAsync(id).AsTask();
                } 

                public void Add(Product product)
                {
                    _context.Products.Add(product);
                }
            }

            - ORDER REPOSITORY:
            public interface IOrderRepository
            {
                void Add(Order order);
            }

            public class OrderRepository : IOrderRepository
            {
                private readonly AppDbContext _context;
                public OrderRepository(AppDbContext context)
                {
                    _context = context;
                }

                public void Add(Order order)
                {
                    _context.Orders.Add(order);
                }
                   
            }

                        
            🧱 2. CREATE UNIT OF WORK INTERFACE
            ------------------------------------------------------------
            public interface IUnitOfWork
            {
                IProductRepository Products { get; }
                IOrderRepository Orders { get; }
                Task<int> CompleteAsync();
            }

            🧱 3. IMPLEMENT UNIT OF WORK
            ------------------------------------------------------------
            public class UnitOfWork : IUnitOfWork
            {
                private readonly AppDbContext _context;

                public IProductRepository Products { get; }
                public IOrderRepository Orders { get; }

                public UnitOfWork(AppDbContext context,
                                  IProductRepository productRepo,
                                  IOrderRepository orderRepo)
                {
                    _context = context;
                    Products = productRepo;
                    Orders = orderRepo;
                }

                public Task<int> CompleteAsync()
                {
                    return _context.SaveChangesAsync();
                }
            }

            ✔ CompleteAsync() is where all changes are committed.


            🧱 4. REGISTER REPOSITORIES + UOW IN DI (PROGRAM.CS)
            ---------------------------------------------------------------------------
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            🧱 5. USE UNIT OF WORK IN A WEB API CONTROLLER
            -----------------------------------------------------------------------------
            [ApiController]
            [Route("api/[controller]")]
            public class OrdersController : ControllerBase
            {
                private readonly IUnitOfWork _uow;

                public OrdersController(IUnitOfWork uow)
                {
                    _uow = uow;
                }

                [HttpPost]
                public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
                {
                    var product = await _uow.Products.GetAsync(dto.ProductId);

                    if (product == null)
                        return NotFound("Product not found");

                    var order = new Order { ProductId = product.Id, Quantity = dto.Quantity };

                    _uow.Orders.Add(order);

                    // Only ONE call commits everything
                    await _uow.CompleteAsync();                         <----- ONE CALL COMMITS EVERYTHING

                    return Ok(order);
                }
            }

            ----------------------------------------------------------------------------
            ✔ All DB writes are saved together
            ✔ If an error occurs, nothing is written
            ---------------------------------------------------------------------------




            ⭐ BENEFITS OF UNIT OF WORK IN WEB API
            | Benefit                           | Explanation                             |
            | --------------------------------- | --------------------------------------- |
            | **Atomic Transactions**           | All DB operations succeed or roll back  |
            | **Cleaner Services**              | Only one call to save                   |
            | **Decoupling**                    | Controller does not worry about EF Core |
            | **Easier Testing**                | Mock repositories + UoW                 |
            | **Reduces duplicate SaveChanges** | One commit at the end                   |



            🧠 HOW UNIT OF WORK WORKS WITH EF CORE?
            - EF Core's DbContext already:
                Tracks changes
                Performs transactions around SaveChanges()
                Allows multiple repositories to use the same context
            - So UoW simply exposes those capabilities in a cleaner architecture.




            ⚠️ WHEN NOT TO USE UNIT OF WORK?
            - If you use only EF Core repositories & services → EF itself is UoW
            - For extremely small applications
            - When every API only performs a single DB operation
            - But many teams still use UoW for cleaner, testable architecture.



            🔚 SUMMARY
            Unit of Work Pattern in Web API:
            ✔ Combines repository calls into one transaction
            ✔ Prevents partial data updates
            ✔ Improves architecture and testability
            ✔ Works perfectly with EF Core
            ✔ Keeps your controllers and services clean
             */
            #endregion

        }
    }
}
