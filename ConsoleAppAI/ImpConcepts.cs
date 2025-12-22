using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAI
{
    internal class ImpConcepts
    {
        #region T in C#
        /* 
       ✅ What is T in C#?
        ---------------------------------------------------
        - T stands for Type Parameter in generics.
        - Generics allow you to write reusable classes, methods, interfaces, and data structures that work with any data type.
        
        👉 T is just a placeholder for a type.
        - When you use the generic type, you replace T with an actual type like:
            int
            string
            User
            Product
            your own classes


        Example:List<int> numbers = new List<int>();   // T = int
                List<string> names = new List<string>(); // T = string


        🔍 Why letter T?
        ----------------------------
        T stands for Type.
        It's just a naming convention. You can use any name:  class Box<MyType> { }

        But developers typically use:
        T → Type
            TKey
            TValue
            TResult
            TItem


        SIMPLE EXAMPLE: A Generic Class
        -----------------------------------
        public class Box<T>
        {
            public T Value { get; set; }

            public Box(T value)
            {
                Value = value;
            }
        }
        ----------------------------------------     T becomes the type you pass.
        Usage:
        var intBox = new Box<int>(10);          // T = int
        var stringBox = new Box<string>("Hi");  // T = string
        var boolBox = new Box<bool>(true);      // T = bool


        EXAMPLE1: Generic Method
        -------------------------------------
        public T GetFirst<T>(List<T> items)
        {
            return items[0];
        }
        -------------------------------------
        int firstNum = GetFirst(new List<int> { 1, 2, 3 });       // T = int
        string firstName = GetFirst(new List<string> { "A", "B" }); // T = string


        EXAMPLE2: Generics in .NET built-in classes
        -------------------------------------
        Example: List<T>
        List<int> ids = new List<int>();        // T = int
        List<string> names = new List<string>(); // T = string

        Example: Dictionary<TKey, TValue>
        Dictionary<string, int> ages = new Dictionary<string, int>();
        // TKey = string
        // TValue = int


        EXAMPLE3: Generic Repository Example (Web API)
        ----------------------------------------
        public interface IRepository<T> where T : class
        {
            Task<List<T>> GetAllAsync();
            Task<T> GetByIdAsync(int id);
            Task AddAsync(T entity);
        }
        ------------------------------------------           Now the SAME repository code works for any entity.
        Usage:
        IRepository<User> userRepo;     // T = User
        IRepository<Product> productRepo; // T = Product


        SUMMARY:
        | Example                    | Meaning                    |
        | -------------------------- | -------------------------- |
        | `List<T>`                  | List of any type           |
        | `Dictionary<TKey, TValue>` | Generic key/value          |
        | `Task<T>`                  | Async result of type T     |
        | `IRepository<T>`           | Repository for any model   |
        | `Box<T>`                   | Class that stores any type |
 
         */
        #endregion

        #region Task, Task<T>
        /*
         In .NET, Task and Task<T> (the T part) are all about asynchronous programming and generics.


        1. What is Task in C#?
        ----------------------------------------------------------------------------
        Task (from System.Threading.Tasks) represents an operation that runs asynchronously.
        Think of it like a promise that “this work is happening and will finish later”.


        Key points about Task:
        ----------------------------------------------------------------------------------
        - Represents work in progress (may be running, completed, failed, or canceled).
        - Used heavily with async / await.
        - Task = “does some work, but does NOT return a value”.
        - Task<T> = “does some work, and returns a value of type T later”.


        SIMPLE EXAMPLE: Task without return value
        -----------------------------------------------------------
        using System;
        using System.Threading.Tasks;

        class Program
        {
            static async Task Main()
            {
                Console.WriteLine("Before calling DoWorkAsync");
                await DoWorkAsync();             // await the Task
                Console.WriteLine("After calling DoWorkAsync");
            }

            // This is an async method that returns Task (no result)
            static async Task DoWorkAsync()
            {
                Console.WriteLine("Work started...");
                await Task.Delay(2000);          // Simulate some async work (2 seconds)
                Console.WriteLine("Work finished!");
            }
        }
        -----------------------------------------------------------------
        What’s happening?
        - DoWorkAsync returns a Task that represents the operation.
        - await DoWorkAsync() waits asynchronously until that task completes.
        - The method signature: async Task DoWorkAsync() → no return value, just a Task.


        IN WEB API (NO RETURN VALUE EXAMPLE):
        ----------------------------------------------------------
        [HttpPost("cleanup")]
        public async Task<IActionResult> CleanupData()
        {
            await _cleanupService.RunCleanupAsync();   // returns Task
            return NoContent();
        }






        2. What is Task<T>?
        ---------------------------------------------------------------------------------
        Task<T> represents an asynchronous operation that produces a result of type T.
        Example: Task<int> → async operation that returns an int.
        Example: Task<User> → async operation that returns a User object.
        Example: Task<List<Product>> → async operation that returns a list of products.


        EXAMPLE: Task<int>
        ----------------------------------------------------------------------------
        using System;
        using System.Threading.Tasks;

        class Program
        {
            static async Task Main()
            {
                int result = await AddAsync(10, 20);     // Await Task<int>
                Console.WriteLine($"Result = {result}");
            }

            static async Task<int> AddAsync(int a, int b)
            {
                await Task.Delay(1000);                  // Simulate async work
                return a + b;                            // This becomes the Task<int> result
            }
        }
        ---------------------------------------------------------------------------------
        - AddAsync returns Task<int>.
        - Inside Main, await AddAsync(10, 20) gives you an int.


        In Web API with Task<T>
        -------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id); // returns Task<Product>

            if (product == null)
                return NotFound();

            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            return Ok(dto);    // here T is ProductDto for ActionResult<ProductDto>
        }
        -----------------------------------------------------------------------
        Method return type: Task<ActionResult<ProductDto>>
        - Task<...> → async.
        - ActionResult<ProductDto> → what the API ultimately returns to the client.
        - FindAsync returns Task<Product>, we await it to get Product.


         */
        #endregion

        #region Events and Delegates
        /*
        In C# .NET, delegates and events are fundamental concepts for implementing decoupled and extensible systems, 
        often utilized in Web API scenarios for notification and callback mechanisms.


        DELEGATES:
        A delegate in C# is a type-safe function pointer. 
        It defines a method signature (return type and parameters) and can hold references to one or more methods that match that signature.

        ----------------------------------------------------------------------------
        EXAMPLE: A DELEGATE STORING A FUNCTION
            public delegate void MyDelegate(string message);     ➡ A method that returns void and takes a string.

        NOW USE IT:
        void PrintMessage(string msg)
        {
            Console.WriteLine(msg);
        }

        MyDelegate d = PrintMessage;   // assign method to delegate
        d("Hello!");                    // invoke delegate → calls PrintMessage

        ---------------------------------------------------------------------------

        ⭐ WHY DELEGATES EXIST?
        Delegates enable:
        - callbacks
        - custom logic injection
        - implementing events
        - passing a method as a parameter


        REAL-WORLD EXAMPLE (USING DELEGATE AS CALLBACK)
        --------------------------------------------------------------------
        public delegate int MathOperation(int a, int b);

        public static int Add(int x, int y) 
        {
            return x + y;
        }

        public static int Calculate(int a, int b, MathOperation op)
        {
            return op(a, b);   // calling the delegate
        }

        var result = Calculate(10, 20, Add);           //You passed a method (Add) into another method. This is callback behavior.
        Console.WriteLine(result);   // 30
        ---------------------------------------------------------------------------









        EVENTS:
        An event is a wrapper around a delegate that follows a publish–subscribe (observer) pattern.
            👉 A delegate is the type,
            👉 an event is the mechanism.        
        Events are declared using the event keyword and are always associated with a delegate type.

        Events allow objects to:
        - publish notifications
        - other objects can subscribe to receive those notifications

       ⭐ REAL-WORLD ANALOGY
        Delegate = your phone number
        Event = WhatsApp group
        Subscribers = people who joined the group
        Publish = someone sends a message → all subscribers get it


        -------Example of an event:----------
        public event EventHandler SomethingHappened;



        EVENT EXAMPLE (SIMPLE):
        ---------------------------------------------------------------------------
        Step 1: Define event using delegate
        public class Process
        {
            public delegate void Notify();  // delegate type

            public event Notify OnProcessCompleted;  // event

            public void Start()
            {
                Console.WriteLine("Process started...");
                Thread.Sleep(2000);
                OnProcessCompleted?.Invoke(); // fire event
            }
        }


        Step 2: Subscribe to event
        static void Main()
        {
            Process p = new Process();
            p.OnProcessCompleted += () => Console.WriteLine("Process completed!");
            p.Start();
        }

        output:
        Process started...
        Process completed!
       -------------------------------------------------------------------------------




        🧠 RELATIONSHIP BETWEEN DELEGATES AND EVENTS
        | Concept          | Meaning                                 |
        | ---------------- | --------------------------------------- |
        | **Delegate**     | Function pointer (stores a method)      |
        | **Event**        | A delegate with publish–subscribe rules |
        | **EventHandler** | Built-in delegate used for events       |
        | **EventArgs**    | Data passed during event notification   |

        ------Events REQUIRE delegates internally.-------------
        */

        #endregion

        #region Extension Methods
        /*
        DEFINITION:
        An extension method in C# is a special kind of static method that allows you to add new methods to an existing class 
        without modifying the original class, without inheritance, and without using partial classes.

        - It looks like the new method belongs to the class you're extending — but it actually lives in a separate static class.


        🎯 WHY EXTENSION METHODS?
        ----------------------------------------------------------
        - Add extra functionality to existing .NET types
        - Add methods to classes you can’t modify (sealed or from external libraries)
        - Make code more readable (fluent syntax)
        - Used widely in LINQ (Select, Where, OrderBy, etc.)


        ⭐ HOW TO CREATE AN EXTENSION METHOD
        --------------------------------------------
        - Create a static class
        - Create a static method
        - The first parameter must start with this keyword
        - That parameter defines which class you’re extending


        EXAMPLE: EXTENSION METHOD FOR STRING
        ------------------------------------
        public static class StringExtensions
        {
            public static bool IsCapitalized(this string input)
            {
                return !string.IsNullOrEmpty(input) && char.IsUpper(input[0]);
            }
        }

        USE IT LIKE THIS:
        ---------------------------
        string name = "Hello";
        bool result = name.IsCapitalized();      --> is same as StringExtensions.IsCapitalized(name);
        Console.WriteLine(result);  // true


        IMP:
        - Even though we never modified the string class, now it looks like string has a method called .IsCapitalized().
        - name.IsCapitalized(); Is just syntax sugar for StringExtensions.IsCapitalized(name);



        ✔ 1. EXTENSION METHOD FOR INT
        -------------------------------------
        public static class IntExtensions
        {
            public static bool IsEven(this int number)
            {
                return number % 2 == 0;
            }
        }
        Usage:
        int n = 10;
        Console.WriteLine(n.IsEven()); // true


        ✔ 2. EXTENSION METHOD FOR LIST<T>
        -----------------------------------------
        public static class ListExtensions
        {
            public static void PrintAll<T>(this List<T> items)
            {
                foreach (var item in items)
                    Console.WriteLine(item);
            }
        }
        Usage:
        var numbers = new List<int> { 1, 2, 3 };
        numbers.PrintAll(); 


        ✔ 3. EXTENSION METHOD IN .NET WEB API (COMMON!)
        ---------------------------------------
        Example: Middleware Extension Method
        public static class MiddlewareExtensions
        {
            public static IApplicationBuilder UseRequestLog(this IApplicationBuilder app)
            {
                return app.UseMiddleware<RequestLoggingMiddleware>();
            }
        }
        Usage in Program.cs:
        app.UseRequestLog();


        ✔ EXTENSION METHODS IN LINQ
        All LINQ methods are extension methods:
        var result = numbers.Where(x => x > 10).ToList();
        Where, Select, OrderBy, ToList → ALL are extension methods.

        📌 RULES OF EXTENSION METHODS:
        | Rule                                  | Explanation                                    |
        | ------------------------------------- | ---------------------------------------------- |
        | Must be in a **static class**         | Required                                       |
        | Must be a **static method**           | Required                                       |
        | First parameter starts with `this`    | Indicates which class you are extending        |
        | Works only when namespace is imported | `using YourExtensions;`                        |
        | Cannot override existing methods      | But can add same-name method (not recommended) |

         */
        #endregion

        #region How to create custom middleware in C# .net
        /*
        WHAT IS MIDDLEWARE?
        Middleware is a component that:
        - receives an HTTP request
        - can process it
        - can pass it to the next middleware
        - can also modify the response
        All middleware components form a pipeline.


        ----------STEPS TO CREATE CUSTOM MIDDLEWARE------------

        ✔ STEP 1: CREATE THE MIDDLEWARE CLASS   
        EXAMPLE: LOGGING MIDDLEWARE
        --------------------------------------------------------------------------
        using Microsoft.AspNetCore.Http;
        using System.Threading.Tasks;

        public class RequestLoggingMiddleware
        {
            private readonly RequestDelegate _next;

            // Middleware requires RequestDelegate in constructor
            public RequestLoggingMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                // Logic to execute BEFORE the next middleware
                Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

                await _next(context); // Call the next middleware in the pipeline

                // Logic to execute AFTER the next middleware
                Console.WriteLine($"Outgoing Response: {context.Response.StatusCode}");
            }
        }

        Explanation:
        InvokeAsync runs for every HTTP request.
        _next(context) → forwards the request to the next middleware.


        ✔ STEP 2: CREATE EXTENSION METHOD (RECOMMENDED)  This makes it cleaner to register middleware.
        -------------------------------------------------------------
        public static class RequestLoggingMiddlewareExtensions
        {
            public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<RequestLoggingMiddleware>();
            }
        }


        ✔ STEP 3: REGISTER MIDDLEWARE IN PROGRAM.CS
        --------------------------------------------------------------
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseRequestLogging();       // <--- Your custom middleware  OR you can skip step2 and write app.UseMiddleware<RequestLoggingMiddleware>();

        app.MapControllers();
        app.Run();

        🎉 DONE! YOUR CUSTOM MIDDLEWARE NOW LOGS EVERY REQUEST & RESPONSE.





        ANOTHER EXAMPLE: EXCEPTION HANDLING MIDDLEWARE
        -------------------------------------------------------
        public class GlobalExceptionMiddleware
        {
            private readonly RequestDelegate _next;

            public GlobalExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context); 
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "An error occurred",
                        Error = ex.Message
                    });
                }
            }
        }

        Register:
        app.UseMiddleware<GlobalExceptionMiddleware>();



        EXPLANATION:
        RequestDelegate _next: This delegate represents the next middleware component in the pipeline. Calling _next(context) passes control to the subsequent middleware.
        HttpContext context: This object provides access to the current HTTP request and response, allowing you to inspect and modify them within your middleware.
        InvokeAsync: This is the entry point for your middleware's logic. You can perform actions before calling _next(context) (e.g., request validation, logging) and after (e.g., response modification, additional logging).
        app.UseMiddleware<YourMiddlewareClassName>(): This method adds your custom middleware to the ASP.NET Core request pipeline. When a request comes in, it will flow through the middleware components in the order they are registered. 


        | Feature                                       | Middleware | Filters |
        | --------------------------------------------- | ---------- | ------- |
        | Runs before routing                           | ✔ Yes      | ❌ No    |
        | Runs after routing                            | ✔ Yes      | ✔ Yes   |
        | Can short-circuit request                     | ✔ Yes      | ✔ Yes   |
        | Works outside MVC (Static files, Razor, etc.) | ✔ Yes      | ❌ No    |
        | Used for cross-cutting concerns               | ✔ Yes      | ✔ Yes   |
        | Controller-specific                           | ❌ No       | ✔ Yes   |

         */
        #endregion

        #region How .NET Core handles Concurrency
        /*
        ASP.NET Core handles concurrency in a few different layers:
        - Threading & request handling (Kestrel + thread pool)
        - Async/await & non-blocking I/O
        - EF Core / database-level concurrency
        - In-memory vs distributed data (shared state)
        

        1. REQUEST HANDLING: KESTREL + THREAD POOL
        ------------------------------------------------------------------------------------
        - ASP.NET Core runs on Kestrel, an event-driven web server.
        - Each incoming HTTP request is processed by a thread from the .NET thread pool.
        - ASP.NET Core is designed to handle many concurrent requests with a limited number of threads by using async I/O.

        KEY IDEA:
        - If your code is blocking (e.g., Thread.Sleep, Task.Wait(), long CPU loops), that thread is occupied and cannot serve other requests → low throughput.
        - If your code is async (e.g., await dbContext.SaveChangesAsync()), the thread is released back to the pool while waiting (DB, I/O, network), and can serve another request → high concurrency.
        So, ASP.NET Core + async/await = scales to many concurrent users.



        2. ASYNC/AWAIT AND CONCURRENCY
        --------------------------------------------------------------------------------------
        In ASP.NET Core, you almost always write controller actions like:
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id); // non-blocking
            if (user == null) return NotFound();
            return Ok(user);
        }

        UNDER THE HOOD:
        - FindAsync starts an async I/O operation to DB.
        - The current request thread is freed while DB is responding.
        - When DB finishes, the framework resumes the method, often on another thread.
        So, multiple requests can be in-flight even with fewer threads → concurrency.



        3. CONCURRENCY IN EF CORE (DATA CONCURRENCY)
        ---------------------------------------------------------------------------------
        Now think two users editing the same row at the same time.
        Concurrency here means: handling conflicts on shared data.

        3.1 TWO MAIN STRATEGIES
        -------------------------------------
        a) Pessimistic concurrency (DB locks)
        * Lock the row so others must wait.
        * Done via explicit transactions/locking in DB.
        * Less common in typical EF Core Web APIs.

        b) Optimistic concurrency (EF default)
        * Assume conflicts are rare.
        * Allow multiple users to read and attempt to update.
        * When saving, EF checks whether data in DB changed since it was read.
        * If changed → throws DbUpdateConcurrencyException.

        3.2 OPTIMISTIC CONCURRENCY WITH A ROWVERSION / TIMESTAMP
        ------------------------------------------------------------
        Entity example:
        public class Product
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Price { get; set; }

            [Timestamp]                  // or .IsRowVersion() in Fluent API
            public byte[] RowVersion { get; set; }
        }

        What happens:
        * Client GETs product → receives RowVersion value.
        * Client sends PUT/PUT with modified data and original RowVersion.
        * EF generates SQL with a WHERE RowVersion = @originalValue.
        * If another user updated this product, DB RowVersion changed → UPDATE affects 0 rows → EF throws DbUpdateConcurrencyException.

        You handle it like:
        public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.RowVersion = dto.RowVersion; // from client

            try
            {
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "Data was modified by another user. Please refresh." });
            }
        }

        This is data-level concurrency control, crucial in multi-user systems.



        4. CONCURRENCY AND DI LIFETIMES (SINGLETON, SCOPED, TRANSIENT)
        -----------------------------------------------------------------------
        ASP.NET Core’s built-in Dependency Injection also relates to concurrency:
        - Transient: new instance each time injected.
        - Scoped: one instance per request.
        - Singleton: one instance for the entire app lifetime.

        Why it matters?
        - Scoped services are safe for per-request data (like EF DbContext).
        - services must be thread-safe, because they’re shared across requests.

        Classic rule:
        ❌ Never inject a DbContext into a Singleton.
        ✔ DbContext should be Scoped.



        7. TYPICAL INTERVIEW POINTS ABOUT CONCURRENCY IN ASP.NET CORE
        --------------------------------------------------------------------
        Why is async/await important in Web API?
        → Improves scalability by freeing threads while waiting for I/O.

        What happens if you block with .Result or .Wait()?
        → Possible deadlocks, thread starvation, reduced concurrency.

        How does EF Core handle concurrent updates?
        → Optimistic concurrency using concurrency tokens/RowVersion + DbUpdateConcurrencyException.

        Is DbContext thread-safe?
        → No. One DbContext per request (Scoped), don’t share across threads.

        How do you handle shared in-memory data safely?
        → Use locks or thread-safe collections, or move state to external store (Redis, DB).

        Why are singletons dangerous if they hold mutable state?
        → Many requests may access them concurrently → race conditions unless synchronized.


        In C#, thread-safe collections are classes that allow multiple threads to access and modify data concurrently without causing data corruption or race conditions. 
        They are primarily found in the System.Collections.Concurrent namespace.
        -------------------------------------------------------------------------------------------
        ConcurrentDictionary<TKey, TValue> : A thread-safe dictionary that allows concurrent reading and writing. 
                                            It provides atomic operations like TryAdd, TryRemove, and GetOrAdd to ensure data integrity during simultaneous access.
        ConcurrentQueue<T>: A thread-safe, first-in-first-out (FIFO) queue. Multiple threads can safely enqueue (add) and dequeue (remove) items at the same time.
        ConcurrentStack<T>: A thread-safe, last-in-first-out (LIFO) stack. It supports concurrent push (add) and pop (remove) operations.
        ConcurrentBag<T>: A thread-safe, unordered collection optimized for scenarios where multiple threads frequently add and remove items. 
                          It does not guarantee the order of elements.
         */
        #endregion

        #region IMPORTANT INTERFACES

        #region ICOLLECTION
        /*
        ICollection<T> is an interface in C# that represents a generic collection of items.
        It is commonly used in Entity Framework Core to represent navigation properties for relationships like One-to-Many.

        ICollection<Order> means:
        ➡ A Customer can have a collection (list) of Orders.
        It is basically a container that can hold multiple elements.



        🧩 WHY EF CORE USES ICOLLECTION<T> FOR NAVIGATION PROPERTIES?
        ✔ It supports adding/removing items
        ---------------------------------------------
        Unlike IEnumerable<T>, ICollection allows:
        customer.Orders.Add(newOrder);
        customer.Orders.Remove(order);

        ✔ EF Core can track changes easily
        ------------------------------------------
        EF Core needs a collection type that supports:
        Add()
        Remove()
        Count()

        ✔ It is flexible
        -------------------------------------
        You can assign:
        List<T>
        HashSet<T>
        ObservableCollection<T>
        All to an ICollection property.



        🔍 EXAMPLE:
        ----------------------------------------------
        public class Customer
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }

            public ICollection<Order> Orders { get; set; }
        }

        Usage:
        var customer = new Customer
        {
            Orders = new List<Order>()
        };
        customer.Orders.Add(new Order { TotalAmount = 500 });




        🤔 Why not use List<T> directly?
        You can, and EF Core will still work.
        public List<Order> Orders { get; set; }

        But ICollection<T> is preferred because:
        - It is more abstract
        - Allows you to change implementation later
        - Still supports Add/Remove


        | Interface/Class  | Description                                           |
        | ---------------- | ----------------------------------------------------- |
        | `IEnumerable<T>` | Read-only iteration (no add/remove)                   |
        | `ICollection<T>` | Supports Add, Remove, Count — recommended for EF Core |
        | `List<T>`        | Concrete implementation of ICollection                |


         */
        #endregion


        #endregion

        #region Ref vs Out Keyword
        /*
        - The REF and OUT keywords in C# are used for passing the arguments to a method as a reference type.
        - By default, arguments are passed to a method by value.
        - By using these REF and OUT keywords in C#, we can pass arguments by reference.
        - In this case, any changes made to these arguments in the method body will be reflected in those variable when the control returns to the calling method.


        -------------------------------------------------
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
        ----------------------------------------------------- Output: Result: 300
        Now, my requirement is, when I call the Math function, I want to return the Addition, Multiplication, Subtraction, and Division of the two numbers passed to this function. 
        But, if you know, it is only possible to return a single value from a function in C# at any given point in time i.e. only one output from a function.




        Example using REF to Return Multiple Outputs From a Function in C#
        -----------------------------------------------------------------------
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
        --------------------------------------------------------------------------------



        Example using OUT Parameter to Return Multiple Outputs from a Function in C#:
        --------------------------------------------------------------------------------
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
        ----------------------------------------------------------------------------------
        Fine. We are getting the same result. That means using out, we are also getting the updated values from the Math function. 
        So, it is working very similarly to the ref parameter.




        What are the Differences Between OUT and REF Keyword in C#?
        -------------------------------------------------------------
        Use ref when:
        -------------------------------
        The variable already has a value
        The method may read and modify that value

        📜 Rules:
        ✔ Variable must be initialized before passing
        ✔ Method can read & update it
        ✔ Caller and callee both use ref
        ------------------------------------
        int x = 10;      // must be initialized
        Increment(ref x);
        Console.WriteLine(x); // Output: 11

        void Increment(ref int number)
        {
            number = number + 1;
        }
        ------------------------------------------


        Use out when:
        --------------------------------------------
        The method’s job is to produce a value
        The input value is not important

        📜 Rules
        ✔ Variable does NOT need to be initialized
        ✔ Method must assign a value before returning
        ✔ Caller and callee both use out
        ---------------------------------------------------
        int sum;             // no initialization required
        GetSum(5, 3, out sum);
        Console.WriteLine(sum); // Output: 8

        void GetSum(int a, int b, out int result)
        {
            result = a + b;   // must assign
        }
        -----------------------------------------------------


        Side-by-Side Comparison:
        | Feature                         | `ref`                 | `out`                    |
        | ------------------------------- | --------------------- | ------------------------ |
        | Must initialize before passing  | ✅ Yes                 | ❌ No                     |
        | Method must assign value        | ❌ No                  | ✅ Yes                    |
        | Can read value before assigning | ✅ Yes                 | ❌ No                     |
        | Used for                        | Modify existing value | Return value from method |
        | Intent                          | Input + Output        | Output only              |



        
        */
        #endregion

        #region Var vs Dynamic keyword
        /*
        1. var keyword (Compile-time typing)
        -----------------------------------------------
        var is NOT a data type.
        It is just compile-time type inference.
        The compiler figures out the type at compile time and locks it.

        var number = 10;
        var name = "Jaith";
        var isActive = true;

        Compiler converts this to:
        int number = 10;
        string name = "Jaith";
        bool isActive = true;

        🚫 You must initialize var
        var x;      // ❌ Compile-time error

        🔍 Behavior of var
        var x = 10;
        x = "hello";    // ❌ Compile-time error. Because x is int, not flexible.


        2. dynamic keyword (Run-time typing)
        --------------------------------------------------
        ✔ Type checking is done at runtime, not compile time.

        dynamic value = 10;
        value = "hello";
        value = true;

        ✔ Compiles successfully
        ✔ Type changes at runtime

        ⚠ Runtime Error Example:
        dynamic x = 10;
        Console.WriteLine(x.Length);   // ❌ Runtime error
        Compiler allows it, but crashes at runtime:
        RuntimeBinderException: 'int' does not contain a definition for 'Length'





        4. Important Interview Trick
        ----------------------------------------
        What is the type of var at runtime?
        var x = 10;
        Console.WriteLine(x.GetType());
        ---------Output:------------
        System.Int32
        ➡️ var is fully typed, just hidden.


        ✅ When to use var
        ---------------------------
        ✔ LINQ queries
        ✔ Anonymous types
        ✔ Long generic types
        ✔ Improves readability (when RHS is obvious)
        var users = dbContext.Users
                     .Where(u => u.IsActive)
                     .Select(u => new { u.Name, u.Email })
                     .ToList();


         */
        #endregion



    }
}
