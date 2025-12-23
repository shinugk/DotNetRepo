🎯 Project Use Case (Restated Clearly)
--------------------------------------------
- A user logs in using Google OAuth 2.0
- After login:
	- User profile is stored in our database
	- That user can create / view / update employers
	- Each employer belongs only to the logged-in user
	- No other user can access it
```
Client (Angular / UI)
        |
        |  Google Login
        v
Google OAuth Server
        |
        |  ID Token
        v
.NET Web API
        |
        |  Validate token
        |  Create / Fetch User
        |  Issue JWT
        v
Client (Authorized requests)
```

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
	* dotnet add package Microsoft.AspNetCore.JsonPatch --framework net8.0
	* dotnet add package AspNetCore.Security.CAS --framework net8.0
	* dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --framework net8.0
	* dotnet add package Microsoft.EntityFrameworkCore.Design --framework net8.0
	* dotnet add package Swashbuckle.AspNetCore --framework net8.0
	* dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --framework net8.0
	* dotnet add package Morcatko.AspNetCore.JsonMergePatch.SystemText --framework net8.0
	* dotnet add package Microsoft.EntityFrameworkCore.Relational --framework net8.0
	* dotnet add package AutoMapper --framework net8.0
	* dotnet add package Microsoft.EntityFrameworkCore.InMemory --framework net8.0
	* dotnet add package Pomelo.EntityFrameworkCore.MySql --framework net8.0
	* dotnet add package Pomelo.EntityFrameworkCore.MySql.Json.Newtonsoft --framework net8.0
	* dotnet add package Microsoft.AspNetCore.OData --framework net8.0

10) Configure DB for both InMemory and Disk in DbConfigure.cs
11) Install dotnet ef tools for migrations commands
	* dotnet tool install --global dotnet-ef
12) Create Intial Migration after configuring DB and create sample table and test
    * dotnet ef migrations add InitialCreate
	* dotnet ef database update


-----------------------------------------------------------------------------------------------------------------------
13) Creating a new console app for AI integration
    https://github.com/marketplace/models/azure-openai/gpt-4-1/playground/code?prompt=List+out+grossary+items
    * steps:
    	* created a new console project under main solution
        * Go to the manage NuGet package in Project view and browse for OpenAI
        * Install the OpenAI package (if you want to use this package in other project, pls add the dependecy in .csproj file)
        * OpenAPI Readme on how to use these in .NET: https://github.com/openai/openai-dotnet/blob/main/README.md#getting-started
    	* follow the steps mentioned in https://github.com/marketplace/models/azure-openai/gpt-4-1/playground/code?prompt=List+out+grossary+items
    	* run the console app (check GitHub token is added as said in use this model link in above url
---------------------------------------------------------------------------------------------------------------------------------

14) Created User(changed as per OAuth requirements), Employer and HrDetail Models and configured validation using both data annotation and fluent api.
15) Create migration (update the database with these tables)
	- dotnet ef migrations add CreateUserEmployerHrDetail
	- dotnet ef database update


<details>
<summary> Implementing OAuth </summary>
</details>

<br>
<br>
<br>

<br>
<br>




<br>
<br>
<br

Why we are using Google.Apis.Auth: Difference
---------------------------------------------------------------------------------------------------------------------------------------------------------
| Package                                          | Purpose                                                    | Used In                            |
| ------------------------------------------------ | ---------------------------------------------------------- | ---------------------------------- |
| **`Microsoft.AspNetCore.Authentication.Google`** | Implements **Google login as middleware** (redirect-based) | MVC / Razor / Server-rendered apps |
| **`Google.Apis.Auth`**                           | **Validates Google ID Tokens**                             | REST APIs, SPA, Mobile apps        |

Why Microsoft.AspNetCore.Authentication.Google is ❌ NOT used here:
------------------------------------------------------------
This package is designed for this flow:
```
Browser → Your MVC App → Redirect to Google → Redirect back
```
It:
- Uses cookies
- Handles HTTP redirects
- Assumes server-side UI
- Stores auth state in ASP.NET cookies

❌ Problems for your use case
You are building:
- ✅ .NET Web API
- ✅ JWT-based auth
- ✅ Angular / React / Mobile client
- ❌ No server-side UI
- ❌ No cookies
➡️ Therefore, this package is NOT suitable.


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

Configure Your Google Cloud Project 
--------------------------------------------------
- Go to the Google API Console.
- Select an existing project or create a new one.
- Navigate to APIs & Services > Credentials and click Create Credentials > OAuth client ID.
- Select Web application as the Application type.
- In the Authorized JavaScript origins field, add your Angular app's URL (e.g., http://localhost:4200 for local development).
- Click Create and save your Client ID. 


**angular-oauth2-oidc for Angular integration with Google OAuth 2.0 / OpenID Connect.**
---------------------------------------------------------------------------------------------
Below is a full industry-standard explanation, end-to-end, mapped exactly to your flow and your .NET Web API + Angular use case.

✅ Final Architecture (Industry Standard)
```
Angular App
   ↓
Google OAuth Login (OIDC)
   ↓
Google returns ID Token
   ↓
Angular sends ID Token to .NET API
   ↓
.NET validates Google ID Token
   ↓
.NET creates / finds User in DB
   ↓
.NET issues its OWN JWT
   ↓
Angular stores JWT
   ↓
Angular calls protected APIs using JWT
```

**🚨 Important rule (as you mentioned):**
- ❌ Never use Google token for authorization
- ✅ Always use your own JWT for APIs

1️⃣ Why angular-oauth2-oidc? (Answering your question)
---------------------------------------------------------
❓ Can we use angular-oauth2-oidc?
- 👉 YES — this is the recommended Angular approach

❓ Why this library?
- Because it:
	- Fully supports OAuth 2.0 + OpenID Connect
- Handles:
	- Google login
	- Redirects
	- Token parsing
	- Silent refresh
	- Is framework-agnostic and production-ready
	- This is the same library used in enterprise Angular apps.

- 🔑 Google = Identity Provider
- 🔐 Your API = Authorization Server

- Google only proves WHO the user is.
- Your API decides WHAT the user can do.

That’s why
- ❌ Never authorize using Google token
- ✅ Always issue your own JWT
            
1️⃣ Google OAuth Login (Frontend Responsibility)
-----------------------------------------------
What happens
- User clicks “Login with Google”
- Google authenticates user
- Google returns ID Token (JWT)

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
**📌 Frontend sends this ID Token to your API**
- 2️⃣ API Endpoint: Accept Google ID Token
- 3️⃣ Validate Google ID Token (VERY IMPORTANT)
- 4️⃣ Create or Find User in Database
- 5️⃣ Issue YOUR OWN JWT (Most Important Step)
- 6️⃣ Secure Your API Using JWT


🎯 Interview-Ready Summary
------------------------------------------------------
“We use Google OAuth for identity verification.
After validating the Google ID token, we create or fetch the user in our database and issue our own JWT.
All authorization is handled using our JWT, not Google’s token.”




2️⃣ Angular Setup (Step-by-Step)
--------------------------------------
2.1 Install package
```
npm install angular-oauth2-oidc
```

2.2 Configure OAuth in Angular
auth.config.ts
```
import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'https://accounts.google.com',
  redirectUri: window.location.origin,
  clientId: 'GOOGLE_CLIENT_ID.apps.googleusercontent.com',
  scope: 'openid profile email',
  strictDiscoveryDocumentValidation: false,
};

2.3 Auth Service (Angular)
auth.service.ts
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthService {

  constructor(
    private oauthService: OAuthService,
    private http: HttpClient
  ) {
    this.configure();
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }

  loginWithGoogle() {
    this.oauthService.initLoginFlow();
  }

  logout() {
    localStorage.removeItem('jwt');
    this.oauthService.logOut();
  }

  async exchangeTokenWithBackend() {
    const idToken = this.oauthService.getIdToken();

    return this.http.post<any>(
      'https://localhost:5001/api/auth/google',
      { idToken }
    ).subscribe(res => {
      localStorage.setItem('jwt', res.jwt);
    });
  }
}
```

2.4 Login Button (Angular Component)
```
<button (click)="login()">Login with Google</button>

login() {
  this.authService.loginWithGoogle();
}
```

2.5 Call Backend After Login
In AppComponent or AuthCallbackComponent:
```
ngOnInit() {
  if (this.oauthService.hasValidIdToken()) {
    this.authService.exchangeTokenWithBackend();
  }
}
```

3️⃣ .NET Web API – Google Token Validation
-----------------------------------------------------------
3.1 Why Google.Apis.Auth
 It is different from `Microsoft.AspNetCore.Authentication.Google`

Package	Used for
- `Microsoft.AspNetCore.Authentication.Google`	MVC / Razor login
- `Google.Apis.Auth`	✅ Validate Google ID token in APIs
👉 We use Google.Apis.Auth because Angular logs in, not .NET

3.2 Install package (.NET)
dotnet add package Google.Apis.Auth

3.3 Auth Controller
AuthController.cs
```
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == payload.Email);

        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                Name = payload.Name,
                GoogleId = payload.Subject,
                ProfilePicture = payload.Picture
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var jwt = _jwtService.GenerateToken(user);

        return Ok(new { jwt });
    }
}
```

3.4 DTO
```
public class GoogleLoginDto
{
    public string IdToken { get; set; }
}
```

4️⃣ Data Model Changes (Very Important)
--------------------------------------------
✅ Updated User Entity
```
public class User
{
    public int Id { get; set; }

    [Required]
    public string GoogleId { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Name { get; set; }

    public string ProfilePicture { get; set; }

    public ICollection<Employer> Employers { get; set; }
}
```

🔑 Why this change?
- Password ❌ (Google handles auth)
- GoogleId used as external identity
- Email is unique

5️⃣ Issue Your OWN JWT (Core Rule)
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

6️⃣ Protect Employer APIs
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

7️⃣ Angular JWT Interceptor
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

8️⃣ Why This Flow Is Industry Standard
-----------------------------------
- ✅ SPA controls login
- ✅ Backend controls authorization
- ✅ Google = Identity Provider
- ✅ JWT = API Security
- ✅ No vendor lock-in

🏁 Final Summary
--------------------------------------
| Layer    | Responsibility           |
| -------- | ------------------------ |
| Angular  | Google Login             |
| Google   | Identity verification    |
| .NET API | User creation + JWT      |
| JWT      | Authorization            |
| DB       | User → Employers mapping |



<br>
<br>

