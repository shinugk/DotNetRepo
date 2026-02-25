 - Definition : 
   - A child class should be usable in place of its parent class without breaking the system.
   - Child class should not break behavior of parent.

 - Meaning: <br>
    - If class B inherits A ✔  <br>
    - You should be able to use B anywhere A is expected ✔ <br>
    - without errors, exceptions, or changed behavior ❌



Implementing LSP:
----------------------
Step 1 — Interface
```
public interface INotificationService
{
    void Send(string message);
}
```
Step 2 — Implementations
```
public class EmailNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SmsNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");   //If this thows exception then it is violating LSP, in which child behaving different than parent.
    }
}
```
Step 3 — Register in DI
```
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
```
Step 4 — Controller Using Abstraction
```
[ApiController]
[Route("api/notify")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notification;     //we are using interface type (which one gets registered in Program.cs that one gets served) controller does not care which service is used

    public NotificationController(INotificationService notification)
    {
        _notification = notification;
    }

    [HttpPost]
    public IActionResult Send(string message)
    {
        _notification.Send(message);
        return Ok("Notification sent");
    }
}
```

- Controller does not care whether: Email, SMS, WhatsApp is used
- All are substitutable ✔️
- This follows LSP.
