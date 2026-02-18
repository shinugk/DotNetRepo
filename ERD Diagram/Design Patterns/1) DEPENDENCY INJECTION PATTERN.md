- **Category**: Modern / Architectural
- **Description**: Provides dependencies to objects externally(via constructor, method, or property injection) rather than having objects create their own, promoting loose coupling and testability.
- **Why Important**: Built into ASP.NET Core’s DI container(IServiceCollection), it’s fundamental for managing services, repositories, and other dependencies in WebAPIs and microservices.
- **Use Cases**: 
    - Injecting services(e.g., database contexts, logging) into controllers or services.
    - Enabling unit testing by mocking dependencies.


🚫 WITHOUT DI (TIGHT COUPLING):-
-----------------------------------------------------------
```
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
```
Problems:
---------------------------------------------------
- Class creates its own dependency (new EmailService()).
- Hard to test (cannot mock EmailService).
- Hard to replace with SMSService tomorrow.
- Hard to maintain.


✅ WITH DI (LOOSE COUPLING)
--------------------------------------------------------
Step 1: Create an Interface (Abstraction)
```
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
```
Step 2: Inject Dependency
```
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
```
This is registered in Program.cs:
```
builder.services.AddScoped<IEmailService, EmailService>();
```
--------------------------------------------------------



🌟 How DI Works in ASP.NET Core
-----------------------------------------------------------
- DI is built-in. You register dependencies in Program.cs:
  - builder.Services.AddScoped<IEmailService, EmailService>();
- ASP.NET Core automatically provides EmailService wherever IEmailService is needed.



✨ Example in Controller
---------------------------------------------------------------
```
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
```
-----------------------------------------------------------------------
          

🎯 Why Dependency Injection is Important?
-------------------------------------------------------------------------------------
| Benefit                   | Explanation                                        |
| ------------------------  | -------------------------------------------------- |
| ✔ Loose Coupling         | Classes depend on interfaces, not concrete classes |
| ✔ Easy Testing           | You can mock dependencies in unit tests            |
| ✔ Better Maintainability | Change one service without touching others         |
| ✔ Follows SOLID → DIP    | Dependency Inversion Principle                     |
| ✔ Built-in container     | ASP.NET Core supports DI natively                  |


🏗️ Types of Dependency Injection
-------------------------------------------------------------------------------------
| Type                      | Description                                        |
| ------------------------- | -------------------------------------------------- |
| Constructor Injection     | Inject via class constructor (most common in .NET) |
| Method Injection          | Inject dependency as a method parameter            |
| Property Injection        | Inject via property (rarely used)                  |


🧱 Service Lifetime in DI
------------------------------------------------------------------------
| Lifetime      | Meaning                    | Example          |
| ------------- | -------------------------- | ---------------- |
| Singleton     | One instance for whole app | `AddSingleton<>` |
| Scoped        | One instance per request   | `AddScoped<>`    |
| Transient     | New instance every time    | `AddTransient<>` |


builder.Services.AddScoped<IEmailService, EmailService>();       <---PROGRAM.CS WE HAVE TO REGISTER
