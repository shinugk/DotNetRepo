
- Meaning → A class should do only one job. it should not include all logic in one class.
- It improves maintainability, testability, and reduces coupling by separating concerns into different classes such as controllers, services, and repositories.
- In a Web API project, SRP helps keep controllers, services, repositories, and helpers clean, testable, and easy to maintain.

 ❌ BAD EXAMPLE: SRP VIOLATION IN WEBAPI (all logic in one controller)
 ---------------------------------------------------------------
 ```
 [ApiController]
 [Route("api/[controller]")]
 public class UsersController : ControllerBase
 {
     [HttpPost("register")]
     public IActionResult Register(User user)
     {
         // 1. Validate user
         if (string.IsNullOrEmpty(user.Email))
             return BadRequest("Email required");

         // 2. Save to database
         using var db = new AppDbContext();
         db.Users.Add(user);
         db.SaveChanges();

         // 3. Send welcome email
         var smtp = new SmtpClient();
         smtp.Send("admin@site.com", user.Email, "Welcome", "Hello!");

         // 4. Log activity
         File.AppendAllText("logs.txt", $"New user: {user.Email}");

         return Ok("User Registered");
     }
 }
```

<br>

 ✅ GOOD EXAMPLE: APPLYING SRP IN WEBAPI 
 -------------------------------------------------------------------------------
 - We split responsibilities into:
     - Controller → Accept requests
     - Service → Business logic
     - Repository → Database logic
     - EmailService → Email operations
     - LoggerService → Logging

 1️) Controller (only handles HTTP requests)
 -------------------------------------------------- 
 ✔ Single responsibility: Accept input + return HTTP response.
 ```
 [ApiController]
 [Route("api/[controller]")]
 public class UsersController : ControllerBase
 {
     private readonly IUserService _service;

     public UsersController(IUserService service)
     {
         _service = service;
     }

     [HttpPost("register")]
     public IActionResult Register(User user)
     {
         _service.RegisterUser(user);
         return Ok("User Registered");
     }
 }
```

 2️) Service Layer (business logic) 
 --------------------------------------------------------------
 ✔ Handles only business flow. <br>
 ✔ No HTTP logic.
 ```
 public interface IUserService
 {
     void RegisterUser(User user);
 }

 public class UserService : IUserService
 {
     private readonly IUserRepository _repo;
     private readonly IEmailService _email;
     private readonly IAppLogger _logger;

     public UserService(IUserRepository repo, IEmailService email, IAppLogger logger)
     {
         _repo = repo;
         _email = email;
         _logger = logger;
     }

     public void RegisterUser(User user)
     {
         _repo.Add(user);                               <---- This is handled in repository for db operations _context.savechanges();
         _email.SendWelcomeEmail(user.Email);           <--- This is handled in Email Service
         _logger.Log($"User registered: {user.Email}");
     }
 }
```
- jojd 



**3) Email Service:**
-----------------------------------------------------
✔ Only email responsibility.
```
 public interface IEmailService
 {
     void SendWelcomeEmail(string to);
 }

 public class EmailService : IEmailService
 {
     public void SendWelcomeEmail(string to)
     {
         // send email logic
     }
 }
```
