Definition:
- Don’t force a class to implement methods it does not need. Instead, create smaller, specific interfaces.

🎯 Why ISP Matters in C# .NET?
------------------------------------------------------
- Because:
    - ❌ Large interfaces force classes to implement unused methods
    - ❌ Violates SRP
    - ❌ Makes testing harder
    - ❌ Makes code fragile
    - ❌ Breaks LSP

- ISP keeps interfaces:
    - ✔ Small
    - ✔ Focused
    - ✔ Easy to implement
    - ✔ Easy to test
    - ✔ Easy to extend



❌ BAD EXAMPLE (VIOLATES ISP)
-------------------------------------------    
Suppose you create one big interface for notifications
```
public interface INotificationService
{
    void SendEmail(string email);
    void SendSms(string phoneNumber);
    void SendPush(string deviceId);
}


public class EmailService : INotificationService      Now a class that only sends email is forced to implement methods it does NOT need
{
    public void SendEmail(string email)
    {
        // send email
    }

    public void SendSms(string phoneNumber)
    {
        throw new NotImplementedException();
    }

    public void SendPush(string deviceId)
    {
        throw new NotImplementedException();
    }
}
```
❌ Problems:
- Unused methods
- Throws exceptions → violates LSP
- Hard to test
- Hard to maintain



<br>

✅ GOOD EXAMPLE (FOLLOWS ISP)
----------------------------------------------------------    
Split the large interface into smaller, purpose-based interfaces.

STEP 1: CREATE FOCUSED INTERFACES
----------------------------------------------
```
public interface IEmailService
{
    void SendEmail(string email);
}

public interface ISmsService
{
    void SendSms(string phoneNumber);
}

public interface IPushService
{
    void SendPush(string deviceId);
}
```

STEP 2: IMPLEMENT ONLY WHAT IS NEEDED
-----------------------------------------------
```
Email service:
public class EmailService : IEmailService
{
    public void SendEmail(string email)
    {
        // send email
    }
}

SMS service:
public class SmsService : ISmsService
{
    public void SendSms(string phoneNumber)
    {
        // send SMS
    }
}

Push service:
public class PushNotificationService : IPushService
{
    public void SendPush(string deviceId)
    {
        // send push notification
    }
}
```

STEP 3: USE ONLY RELEVANT INTERFACES IN CONTROLLERS
-----------------------------------------------------------
```
public class AccountController : ControllerBase
{
    private readonly IEmailService _email;

    public AccountController(IEmailService email)
    {
        _email = email;
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        _email.SendEmail(user.Email);
        return Ok("User registered");
    }
}
```
- ✔ The controller only depends on what it needs.
- ✔ No unused methods.
- ✔ Clean architecture.
