- Classes should be open for extension but closed for modification.
- Meaning → <br>
    ✔ You should be able to add new features <br>
    ❌ Without changing existing working code

- This prevents bugs and makes the app scalable.
- Works perfectly with interfaces + dependency injection


❌ BAD EXAMPLE (VIOLATES OCP)
---------------------------------------------  
Example: A logger that needs to support File, Database, Email, Cloud etc.
```
public class Logger
{
    public void Log(string message, string type)
    {
        if (type == "file")
        {
            File.WriteAllText("log.txt", message);
        }
        else if (type == "db")
        {
            SaveToDatabase(message);
        }
        else if (type == "email")
        {
            SendEmail(message);
        }
    }
}
```
- Every time we add a new log type (SMS, Azure, Kafka), we must modify this class → breaks OCP.



<br>

✅ GOOD EXAMPLE (FOLLOWS OCP)  (We use interfaces and polymorphism.)
----------------------------------------------- 
STEP 1: CREATE INTERFACE
----------------------------------------
```
public interface ILogger
{
    void Log(string message);
}
```

STEP 2: CREATE SEPARATE IMPLEMENTATIONS BY INHERITING INTERFACE
--------------------------------------------
```
//FILE LOGGER:
public class FileLogger : ILogger
{
    public void Log(string message)
    {
        File.WriteAllText("log.txt", message);
    }
}

//DATABASE LOGGER:
public class DbLogger : ILogger
{
    public void Log(string message)
    {
        // Save log to database
    }
}

//EMAIL LOGGER (NEW FEATURE ADDED LATER):
public class EmailLogger : ILogger
{
    public void Log(string message)
    {
        // send email
    }
}
```
➡ NOTICE: WE ADDED A NEW LOGGER WITHOUT MODIFYING ANY OLD CODE. THIS IS EXACTLY OCP.


STEP 3: USE THE ABSTRACTION IN THE WEBAPI CONTROLLER. (USE INTERFACE TYPE FOR LOGGER VARIABLE)
----------------------------------------------------------     
- So The controller does not care which logger is used.
```
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger _logger;

    public OrdersController(ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        _logger.Log("Orders fetched");
        return Ok("Order List");
    }
}
```

STEP 4: CONFIGURE DEPENDENCY INJECTION (PROGRAM.CS)
-----------------------------------------------------
- `builder.Services.AddScoped<ILogger, FileLogger>();`
- ➡ Use DB logger instead: `builder.Services.AddScoped<ILogger, DbLogger>();`
- ➡ Add the new Email Logger: `builder.Services.AddScoped<ILogger, EmailLogger>();`
- NO CHANGES to the controller or existing loggers.

<br>

So which logger gets called:
---------------------------
- It depends on what is registered: If you register FileLogger -> it gets called


