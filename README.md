🎯 Project Use Case: [Interview-Tracker](https://it-clientapp.onrender.com/login)
-----------------------------------------------------------------------------------
- A user logs in using Google GIS and also using own jwt token for secure API
- After login:
	- User profile is stored in our database
	- That user can create / view / update employers
	- Each employer belongs only to the logged-in user
	- No other user can access it

 - Project URL: https://it-clientapp.onrender.com/login
 - Swagger: https://dotnetrepo.onrender.com/swagger/index.html


About Project:
-------------------------------------------------------------
Interview-Tracker is a web-based application designed to simplify and organize the job interview process for candidates. The application enables users to record and track interview details such as employer information, company type (product-based or service-based), interview levels, current interview status, and compensation details like offered CTC. Additionally, users can upload and store job offer letters for selected roles. By maintaining all interview-related information in one place, Interview-Tracker helps users monitor progress, compare opportunities, and manage their job search more effectively.

Features:
---------------------------


Tech Stack Used:
----------------------------------------------------------------
- C# .NET Core
- WebAPI
- Mariadb
- Angular
- EF Core


STEPS FOLLOWED:
----------------------------------------------------------------------
1) Created New project from template .NET Core WebAPI
2) Copied ClientApp from another project 
3) Using CLI downloaded 
	  * dotnet add package Microsoft.AspNetCore.SpaProxy --version 6.0.5
4) In LaunchSettings.json added "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy" in profiles env variables
5) In .csproj added
      **<SpaRoot>ClientApp\</SpaRoot>**
    	**<SpaProxyServerUrl>https://localhost:44488</SpaProxyServerUrl>**
    	**<SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>**
7) Added a NUnit Test Project from a template in a same solution

Packages Explanation & Purpose
| Package                                                 | Purpose                                                                                                                                                                                          |
| ------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Microsoft.AspNetCore.JsonPatch**                      | Implements [JSON Patch (RFC 6902)](https://datatracker.ietf.org/doc/html/rfc6902) support in ASP.NET Core so you can partially update resources with PATCH requests (using `JsonPatchDocument`). |
| **AspNetCore.Security.CAS**                             | Middleware for Central Authentication Service (CAS) login integration. Used for Single Sign-On (SSO) scenarios.                                                                                  |
| **AutoMapper.Extensions.Microsoft.DependencyInjection** | Makes it easy to register AutoMapper profiles in ASP.NET Core DI (`services.AddAutoMapper()`), so you can map between DTOs and entities automatically.                                           |
| **Microsoft.EntityFrameworkCore.Design**                | Provides design-time EF Core tools for migrations and scaffolding (`dotnet ef migrations add`, `dotnet ef database update`).                                                                     |
| **Swashbuckle.AspNetCore**                              | Adds Swagger/OpenAPI generation for ASP.NET Core Web APIs.                                                                                                                                       |
| **Microsoft.AspNetCore.Mvc.NewtonsoftJson**             | Enables JSON serialization/deserialization using Newtonsoft.Json instead of System.Text.Json. Useful for advanced JSON handling or legacy compatibility.                                         |
| **Morcatko.AspNetCore.JsonMergePatch.SystemText**       | Adds [JSON Merge Patch (RFC 7396)](https://datatracker.ietf.org/doc/html/rfc7396) support using `System.Text.Json`. Useful for partial resource updates in APIs.                                 |
| **Microsoft.EntityFrameworkCore.Relational**            | Base EF Core package for relational database providers (MySQL, SQL Server, PostgreSQL, etc.). Required when using EF migrations with relational DBs.                                             |
| **AutoMapper**                                          | The core AutoMapper library — performs object-object mapping between models.                                                                                                                     |
| **Microsoft.EntityFrameworkCore.InMemory**              | In-memory database provider for EF Core (often used for unit testing without a real DB).                                                                                                         |
| **Pomelo.EntityFrameworkCore.MySql**                    | MySQL database provider for EF Core.                                                                                                                                                             |
| **Pomelo.EntityFrameworkCore.MySql.Json.Newtonsoft**    | Extension for Pomelo MySQL provider to store JSON columns using Newtonsoft.Json serialization.                                                                                                   |
| **Microsoft.AspNetCore.OData**                          | Adds OData query capabilities (filtering, sorting, pagination, etc.) to ASP.NET Core APIs.                                                                                                       |


8) Installing for .NET 8 - To ensure you get the correct version for .NET 8, you can specify --framework net8.0.
If you omit the version, dotnet will automatically pull the latest compatible release for .NET 8.

9) CLI commands (all in one)
	* `dotnet add package Microsoft.AspNetCore.JsonPatch --framework net8.0`
	* `dotnet add package AspNetCore.Security.CAS --framework net8.0`
	* `dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --framework net8.0`
	* `dotnet add package Microsoft.EntityFrameworkCore.Design --framework net8.0`
	* `dotnet add package Swashbuckle.AspNetCore --framework net8.0`
	* `dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --framework net8.0`
	* `dotnet add package Morcatko.AspNetCore.JsonMergePatch.SystemText --framework net8.0`
	* `dotnet add package Microsoft.EntityFrameworkCore.Relational --framework net8.0`
	* `dotnet add package AutoMapper --framework net8.0`
	* `dotnet add package Microsoft.EntityFrameworkCore.InMemory --framework net8.0`
	* `dotnet add package Pomelo.EntityFrameworkCore.MySql --framework net8.0`
	* `dotnet add package Pomelo.EntityFrameworkCore.MySql.Json.Newtonsoft --framework net8.0`
	* `dotnet add package Microsoft.AspNetCore.OData --framework net8.0`

10) Configure DB for both InMemory and Disk in DbConfigure.cs
11) Install dotnet ef tools for migrations commands
	* `dotnet tool install --global dotnet-ef`
12) Create Intial Migration after configuring DB and create sample table and test
    * `dotnet ef migrations add InitialCreate`
	* `dotnet ef database update`

14) Created User(changed as per OAuth requirements), Employer and HrDetail Models and configured validation using both data annotation and fluent api.
15) Create migration (update the database with these tables)
	- `dotnet ef migrations add CreateUserEmployerHrDetail`
	- `dotnet ef database update`

Note: `dotnet build -c Release --output ./dist` which creates dll files for all projects and to run our specific app we need to run `dotnet InterviewTracker.dll`


If you delete anything by mistake in sql client: Recovery option
----------------------------------------------------------------------
- First delete all rows in _efmigrationhistory table
- then run `dotnet ef database update`
- then refresh heidisql client you will get all tables back


Creating a new console app for AI integration
-----------------------------------------------------------------------------------------------------------------------
- steps: (https://github.com/marketplace/models/azure-openai/gpt-4-1/playground/code?prompt=List+out+grossary+items)
    - created a new console project under main solution
    - Go to the manage NuGet package in Project view and browse for OpenAI
    - Install the OpenAI package (if you want to use this package in other project, pls add the dependecy in .csproj file)
    - OpenAPI Readme on how to use these in .NET: https://github.com/openai/openai-dotnet/blob/main/README.md#getting-started (This shows how to register using dependecy injection and using in controller)
    - follow the steps mentioned in https://github.com/marketplace/models/azure-openai/gpt-4-1/playground/code?prompt=List+out+grossary+items
    - run the console app (check GitHub token is added as said in use this model link in above url


<br>
<br>
<br>

<br>
<br>


**Why we are using `Google.Apis.Auth` instead of `Microsoft.AspNetCore.Authentication.Google` package Difference**
---------------------------------------------------------------------------------------------------------------------------------------------------------
| Package                                          | Purpose                                                    | Used In                            |
| ------------------------------------------------ | ---------------------------------------------------------- | ---------------------------------- |
| **`Microsoft.AspNetCore.Authentication.Google`** | Implements **Google login as middleware** (redirect-based) | MVC / Razor / Server-rendered apps |
| **`Google.Apis.Auth`**                           | **Validates Google ID Tokens**                             | REST APIs, SPA, Mobile apps        |

Why Google.Apis.Auth is ✅ CORRECT for your use case
--------------------------------------------------
- `Google.Apis.Auth` is NOT authentication middleware.
- It is a token validation library.

What it does exactly:
- ✔ Verifies Google ID Token signature
- ✔ Verifies token issuer is Google
- ✔ Verifies token is not expired
- ✔ Verifies token audience (client_id)
- ✔ Extracts user identity claims

<br>
<br>
<br>

Created new Angular app under same dotnet project:
----------------------------------------------------------
- create new folder ng2-app and run `ng new ClientAppIT`
  - selected scss (superset of css) as stylesheet format
- To integrate the Google SDK for OAuth token generation in an Angular application
   you should use the official Google Identity Services library for JavaScript, which handles the OAuth 2.0 flow securely. 
- This process involves configuring your Google Cloud project, installing the necessary library in your Angular app, and implementing a callback function to handle the ID or access token response.

**updated the angular version from Angular16 to Angular20:**
-------------------------------------------------------------
1. **Update Globally**
- First, update your global Angular CLI to the latest version: 
	- `npm uninstall -g @angular/cli`
	- `npm install -g @angular/cli@latest`

2. **Follow the Incremental Path**
- Run the following commands in your project folder to move through each major version. For each step, verify that your application still builds
- To Angular 17:
	- `ng update @angular/core@17 @angular/cli@17`
- To Angular 18:
	- `ng update @angular/core@18 @angular/cli@18`
- To Angular 19:
	- `ng update @angular/core@19 @angular/cli@19`
- To Angular 20 (Latest):
	- `ng update @angular/core@20 @angular/cli@20`

3. **Update Other Dependencies**
- If you use other official packages like Angular Material, update them alongside the core packages: `ng update @angular/material@20`
- Peer Dependency Issues: If you encounter errors like the one you saw previously, you may need to add the --force or --legacy-peer-deps flag, though it is better to update libraries to their compatible versions first.


<br>
<br>
<br>

IMPLEMENTING AUTHENTICATION Google Identity Services for login popup and Own Jwt for Protecting API's:
-------------------------------------------------------------------------------------
✅ Final Architecture (Industry Standard)
```
┌────────────┐
│   Browser  │
│ (Angular)  │
└─────┬──────┘
      │
      │ 1️⃣ Load Google Identity SDK
      │    https://accounts.google.com/gsi/client
      ▼
┌─────────────────────────────┐
│ Google Identity Services     │
│ (accounts.id)               │
└─────────┬───────────────────┘
          │
          │ 2️⃣ User selects Google account
          │    (One Tap / Popup)
          ▼
┌─────────────────────────────┐
│  Google Authentication      │
│  (Handled by Google)        │
└─────────┬───────────────────┘
          │
          │ 3️⃣ ID Token (JWT)
          │    response.credential
          ▼
┌────────────┐
│   Angular  │
│   App      │
└─────┬──────┘
      │
      │ 4️⃣ POST idToken
      │    /api/auth/google
      ▼
┌─────────────────────────────┐
│ .NET Web API Backend         │
│                              │
│ 5️⃣ Verify Google ID Token   │
│   - Signature               │
│   - Audience (client_id)    │
│   - Issuer                  │
│   - Expiry                  │
│                              │
│ 6️⃣ Create / Find User       │
│                              │
│ 7️⃣ Generate App JWT         │
└─────────┬───────────────────┘
          │
          │ 8️⃣ App JWT
          ▼
┌────────────┐
│   Angular  │
│   App      │
└─────┬──────┘
      │
      │ 9️⃣ Store JWT
      │    localStorage
      ▼
┌─────────────────────────────┐
│ Protected Routes & APIs     │
│ (Authorization: Bearer JWT) │
└─────────────────────────────┘

```

STEP 1. Configure Your Google Cloud Project 
--------------------------------------------------
- Go to the Google API Console.
- Select an existing project or create a new one.
- Navigate to APIs & Services > Credentials and click Create Credentials > OAuth client ID. (https://console.cloud.google.com/auth/clients)
- Select Web application as the Application type.
- In the Authorized JavaScript origins field, add your Angular app's URL (e.g., http://localhost:4200 for local development).
- In the Authorized redirect URIs, added two URL http://localhost:4200/app and http://localhost:4200
- Click Create and save your Client ID. 


✅ Google Identity Services (GIS) – ID Token flow
---------------------------------------------------
- No authorization code
- No redirect to Google login page
- No backend token exchange with Google
- Google directly gives you an ID token (JWT) in the browser
- This is NOT OAuth Authorization Code flow.

🧠 Big picture
```
User → Google JS SDK → ID Token (JWT) → Backend → Your App JWT → Login
```

Step-by-step (using YOUR code)
-------------------------------------------------
1️⃣ Load Google Identity Services script
```
script.src = 'https://accounts.google.com/gsi/client';
📌 This loads Google Identity Services SDK, not OAuth endpoints.
No redirect happens here.
```

2️⃣ Initialize Google Sign-In
```
window.google.accounts.id.initialize({
  client_id: '...',
  callback: (response) => {
    this.sendIdTokenToBackend(response.credential);
  }
});
```
What happens here?
- You register your Google Client ID
- You tell Google: “When login succeeds, give me an ID token”
- 📌 This is frontend-only authentication

3️⃣ User clicks Login → prompt()
```
window.google.accounts.id.prompt();
```
What Google does:
- Shows: One Tap popup OR Account chooser dialog
- User selects Google account
- No page reload
- No redirect
📌 This is NOT OAuth redirect flow

4️⃣ Google returns ID Token directly to browser
```
callback: (response) => {
  response.credential   // ← THIS IS THE ID TOKEN (JWT)
}
Example ID token (JWT):
eyJhbGciOiJSUzI1NiIsImtpZCI6Ij...
```
📌 This token:
- Is signed by Google
- Proves user identity
  - Contains claims: sub (Google user ID), email, name, picture

5️⃣ Angular sends ID token to backend
```
POST /api/auth/google
{
  "idToken": "eyJhbGciOiJSUzI1NiIs..."
}
```
📌 Important:
- Angular does NOT talk to Google anymore
- Backend now takes responsibility

6️⃣ Backend validates ID token (CRITICAL STEP)
Your backend must:
- Verify Google signature
Verify:
 - issuer = accounts.google.com
 - audience = your client_id
 - expiry time
 - Extract claims
📌 If validation fails → reject login

7️⃣ Backend creates / finds user
```
Using claims:
sub     → googleId
email   → email
name    → name
picture → profilePictureUrl
```
- If user exists → login
- If not → create new user

8️⃣ Backend generates YOUR OWN JWT
This is not Google’s token.
Your backend creates:
{
  "userId": 12,
  "email": "user@gmail.com"
}
Signed with your secret key.

9️⃣ Angular stores app JWT
```
localStorage.setItem('jwt', res.token);
this.router.navigate(['/home']);
```
Now: Angular guards use this JWT
API calls include:
Authorization: Bearer <your-jwt>
```
🔐 Logout behavior (important)
logout() {
  localStorage.removeItem('jwt');
}
```
📌 This:
- Logs out from your app
- DOES NOT log user out of Google
- Next login → Google auto-selects account (expected behavior)


What ID Token contains:
```
{
  "sub": "109872364987236498723",
  "email": "user@gmail.com",
  "email_verified": true,
  "name": "Jaith Kolkarni",
  "picture": "https://...",
  "iss": "https://accounts.google.com",
  "aud": "YOUR_GOOGLE_CLIENT_ID"
}
```
**📌 Frontend sends this ID Token to your API and next stpes**
-  API Endpoint: Accept Google ID Token
-  Validate Google ID Token (VERY IMPORTANT)
-  Create or Find User in Database
-  Issue YOUR OWN JWT (Most Important Step)
-  Secure Your API Using JWT



STEP 4. .NET Web API – Google Token Validation
-----------------------------------------------------------
4.1 Install Google.Apis.Auth
- In Cmd `dotnet add package Google.Apis.Auth` -> Validate Google ID token in APIs
- It is different from `Microsoft.AspNetCore.Authentication.Google` it is used for MVC / Razor login

4.2 Auth Controller
DTO
```
public class GoogleLoginDto
{
    public string IdToken { get; set; }
}
```
AuthController.cs
```

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITrackerDbContext _db;
        private readonly JwtTokenService _jwtService;
                       
        public AuthController(ITrackerDbContext db, JwtTokenService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)  <-- Receive IdToken from Angular app  POST -> /api/auth/google 
        {
            // 1️) Validate Google ID Token
            var payload = await ValidateGoogleToken(request.IdToken);
            if (payload == null)
                return Unauthorized("Invalid Google token");

            // 2️) Find or create user
            var user = _db.Users.SingleOrDefault(u => u.email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    email = payload.Email,
                    name = payload.Name,
                    googleId = payload.Subject
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            // 3️) Issue YOUR JWT
            var jwt = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token = jwt
            });
        }


        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)   <-- Validating googleId method
        {
            try
            {
                return await GoogleJsonWebSignature.ValidateAsync(idToken);
            }
            catch
            {
                return null;
            }
        }
    }
```

STPE 5. NET Generating Own JWT token for protecting API's (after validating IdToken from OAuth)
----------------------------------------------------------------------------------------------------
In appSettings.json
```
  "Jwt": {
    "Key": "THIS_IS_A_VERY_SECRET_KEY_12345_AndItShouldBeAtleast32Characters",
    "Issuer": "InterviewTrackerApi",
    "Audience": "ITAngularApp"
  }
```
JwtTokenService.cs this is to issue our own jwt token after validating googleId token
```
 public class JwtTokenService
 {
     private readonly IConfiguration _config;

     public JwtTokenService(IConfiguration config)
     {
         _config = config;
     }

     public string GenerateToken(User user)
     {
         var claims = new[]
         {
         new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
         new Claim(JwtRegisteredClaimNames.Email, user.email),
         new Claim("name", user.name)
         };

         var key = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(_config["Jwt:Key"])
         );

         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
             audience: _config["Jwt:Audience"],
             claims: claims,
             expires: DateTime.UtcNow.AddHours(2),
             signingCredentials: creds
         );

         return new JwtSecurityTokenHandler().WriteToken(token);
     }
 }
```
In Program.cs
```
// Add authentication: this generates jwt token for protecting api's after validating googleid token
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// This is for Authorization (when you use [Authorize] in controller)
builder.Services.AddAuthorization();

and

app.UseAuthentication();
app.UseAuthorization();
```

Issue Your OWN JWT (Core Rule)
-------------------------------------
JWT contains:
```
{
  "sub": "123",
  "email": "user@gmail.com",
  "name": "John Doe"
}
```
👉 This JWT is:
- Short-lived
- Signed by your API
- Used for authorization

STEP.6 Protect All other APIs for ex: Add [Authorize] to controller POST method
-------------------------------------
```
[Authorize]
[HttpPost]
public async Task<IActionResult> AddEmployer(Employer employer)
{
    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    employer.UserId = userId;

    _context.Employers.Add(employer);
    await _context.SaveChangesAsync();

    return Ok();
}
```
✔ Ensures each user accesses only their data

STEP.7 Angular JWT Interceptor use this above generated own JWT toke for all subsequemt requests
-------------------------------------
```
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = localStorage.getItem('jwt');

    if (token) {
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    return next.handle(req);
  }
}
```

🏁 Final Summary
--------------------------------------
| Layer    | Responsibility           |
| -------- | ------------------------ |
| Angular  | Google Login             |
| Google   | Identity verification    |
| .NET API | User creation + JWT      |
| JWT      | Authorization            |
| DB       | User → Employers mapping |

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

<br>
<br>
<br>
<br>



Next Steps followed (angular):
---------------------------------------------
Setup Angular Material
- Installed Angular Material for better styling and features
	- `ng add @angular/material` and selected Azure and Blue theme while installing
- edit styles.css for customizing css features for whole app
- Import Required Material Modules
    - Create a separate Material Module(material.module.ts): `ng generate module material` -> best practice to have all required material modules in one place
- Import MaterialModule in AppModule(app.module.ts)
- Add Google material symbol Icons (Font Icons) : https://fonts.google.com/icons?icon.size=24&icon.color=%23e3e3e3&icon.set=Material+Symbols&icon.style=Rounded
  	- Open src/index.html and add inside <head>:
	```
    <link
      rel="stylesheet"
      href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200"
    />
	```
 	- To use icon: now you can use these icons with class "material-symbols-rounded"
    ```
        <span class="material-symbols-rounded">home</span>
	```
- for angular material global style add `"@angular/material/prebuilt-themes/indigo-pink.css"` in angular.json file under -> styles 





Hosting mysql, .net app and angular:
---------------------------------------------
1) for mysql: created new mysql service in https://console.aiven.io/account/a58455ebf06f/project/interviewtracker555/services/mysqldb-it/overview and get connection string for .net app
 - Connect to aiven mysql from your laptop with command:
```
mysql ^
  -u avnadmin ^
  --password="AVNS_wiUbZeBycH0w2nT7r6Q" ^
  -h mysqldb-it-interviewtracker555.l.aivencloud.com ^
  -P 28173 ^
  --ssl ^
  --ssl-ca="C:\Users\SKI75\Desktop\DOTNET\cert\ca.pem" ^
  defaultdb
```
2) Then in .Net app update these above connection string in appsetting.json
  ```
  "DB": {
    "NAME": "IT-database",
    "ITracker_InMemory_DB": false,
    "ITracker_Connection_String": "Server=mysqldb-it-interviewtracker555.l.aivencloud.com;Port=28173;Database=defaultdb;User=avnadmin;Password=Something;SslMode=Required;",
    "VERBOSE_LOGGING": true
  }
  ```

3) Then update the Dockerfile, which we have to give while configuring in Render app https://dashboard.render.com/web/srv-d5ae4l9r0fns7389savg and for every commit it will autodeploy and you can see the logs
  	.net app live link : https://dotnetrepo.onrender.com/swagger/index.html
   
5) Then go to Render -> click on create static site for hosting angular app
   - Before this create `environment.prod.ts` file and save apibaseurl inside that
   - also run `ng build --configuration production` which creates correct output folder at DotNetRepo\InterviewTracker\ng2-app\ClientAppIT\dist\client-app-it
   - Then create static site and fill proper configuration values in settings ang give root folder and publish folder path
   - also add Client routing in Google cloud console for hosted ng app https://it-clientapp.onrender.com/
   - Then your live ng app is available at https://it-clientapp.onrender.com/
  
   - In local run `ng serve --ssl` for https://localhost:4200 which works for google Oauth (oauth flow does not support http)
   - configure redirect/rewrite rules for angular service. which loads proper page for every route



