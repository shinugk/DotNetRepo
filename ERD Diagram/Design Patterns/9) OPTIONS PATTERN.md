 - ✔ Bind configuration settings (from appsettings.json) to strongly-typed C# classes
 - ✔ Access configuration safely (with intellisense & compile-time checking)
 - ✔ Avoid magic strings like "ConnectionStrings:DefaultConnection" everywhere
 - ✔ Centralize configuration values

 - In simple words:
   - Options Pattern = Read configuration settings into strongly-typed classes.


 📦 EXAMPLE CONFIGURATION (appsettings.json)
 ----------------------------------------------------
 ```
 {
   "MyAppSettings": {
     "ApplicationName": "My Web API",
     "Version": "1.0",
     "EnableFeatureX": true
   }
 }
```
 We want to access these values cleanly inside controllers/services.


 🧱 STEP 1 — CREATE A SETTINGS CLASS
 -------------------------------------------------
 ```
 public class MyAppSettings
 {
     public string ApplicationName { get; set; }
     public string Version { get; set; }
     public bool EnableFeatureX { get; set; }
 }
```
 This class must match the structure in appsettings.json.


 🧱 STEP 2 — REGISTER OPTIONS IN PROGRAM.CS
 ----------------------------------------------
 ```
 builder.Services.Configure<MyAppSettings>(
     builder.Configuration.GetSection("MyAppSettings"));
```
 - This tells .NET:
   - "Bind the appsettings.json section MyAppSettings to the class MyAppSettings."


 🧱 STEP 3 — INJECT IOPTIONS IN CONTROLLER
 -----------------------------------------------------------
 ```
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
```
 --------------------------------------------------------------
 Output:
 ```
 {
   "applicationName": "My Web API",
   "version": "1.0",
   "enableFeatureX": true
 }
```


 ⭐ VARIANTS OF OPTIONS PATTERN
 --------------------------------------------------------------------------------
 ASP.NET Core provides multiple “Options” types:
 | Type                 | Use Case                                                     |
 | -------------------- | ------------------------------------------------------------ |
 | **IOptions**         | Read configuration once at startup                           |
 | **IOptionsSnapshot** | Read configuration per *request* (scoped) — good for Web API |
 | **IOptionsMonitor**  | Real-time change notifications when configuration changes    |


---------------------------------------------
 1) IOptions (Singleton)
 - Loaded once at startup
 - Does not update if config changes
 - Good for static settings like API keys, URLs
 - Usage:
     - `public ConfigController(IOptions<MyAppSettings> options)`

-------------------------------------------------------------
 2) IOptionsSnapshot (Scoped per request — best for Web API)
 - Re-evaluates configuration on each web request
 - Useful in development with reload-on-change
 - Only works in scoped services (Web API default)
 - Usage:
     - `public ConfigController(IOptionsSnapshot<MyAppSettings> options)`

-------------------------------------------------------------
 3) IOptionsMonitor (Real-time change notifications)
 - Notifies when configuration changes
 - Can react dynamically (like updating feature flags)
 - Usage:
     - `public ConfigController(IOptionsMonitor<MyAppSettings> options)`



 🧪 EXAMPLE: USING OPTIONS IN A SERVICE (NOT ONLY CONTROLLERS)
 --------------------------------------------------------------------
 ```
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
```
 Register your service: 
     - `builder.Services.AddScoped<FeatureService>();`





 ⭐ WHY USE THE OPTIONS PATTERN?
 -----------------------------------------------------------------------
 | Benefit                          | Explanation                               |
 | -------------------------------- | ----------------------------------------- |
 |     Strongly Typed Configuration | Avoids string-based config keys           |
 |     Intellisense Support         | Easier for developers                     |
 |     Centralized Settings         | Cleaner architecture                      |
 |     Supports config reloading    | With IOptionsSnapshot / Monitor           |
 |     Perfect for Web API          | API keys, connection strings, URLs, flags |



 🚀 REAL WEB API USE CASES
 -----------------------------------------------
 - ✔ Using settings for JWT configuration
 - ✔ Reading connection strings
 - ✔ Feature flags (EnableFeatureX)
 - ✔ API keys for third-party services
 - ✔ Email configuration (SMTP server, port, username)
 - ✔ Paging defaults (PageSize, MaxItems)



 🔚 Summary
 -------------------------------------------------------
 - Options Pattern in ASP.NET Core Web API allows you to:
     - ✔ Bind config → Strong typed classes
     - ✔ Inject config → Through DI using IOptions / Snapshot / Monitor
     - ✔ Manage settings cleanly → Without scattering config strings everywhere
 - It is one of the best practices for handling configuration in modern .NET applications.
