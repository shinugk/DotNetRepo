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
