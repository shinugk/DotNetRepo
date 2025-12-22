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

**IMPLEMENTING OAUTH**
----------------------------------
**1️⃣ Authentication Architecture (Correct Way)**
- ✅ Recommended flow (Industry Standard)
- Google OAuth 2.0 → Your API issues JWT
```
Browser
   ↓
Google OAuth Login
   ↓
Google returns ID Token
   ↓
Your API validates token
   ↓
Your API creates / finds user in DB
   ↓
Your API issues JWT (your system)
   ↓
Client uses JWT for all API calls


👉 Never rely on Google token for authorization
👉 Use your own JWT for all protected endpoints

```
🔐 Authentication Strategy (Correct Approach):
-----------------------------------------------------------------
We will use HYBRID AUTH:
| Purpose           | Tech             |
| ----------------- | ---------------- |
| Identity          | Google OAuth 2.0 |
| API Authorization | JWT              |
| User ownership    | `UserId` FK      |

🪪 Step 1: What Google Gives You
-------------------------------------------
After Google login, you get an ID Token containing:
```
{
  "sub": "1092384729384729384",
  "email": "user@gmail.com",
  "name": "Jaith Kolkarni",
  "picture": "https://..."
}
```
⚠️ IMPORTANT:
- sub is Google User ID
- It is unique and never changes
- This is what you must store

🧩 Step 2: Modify Your User Model (VERY IMPORTANT)
----------------------------------------------------
- Your current User model is not OAuth-ready.
- ❌ Problem with current model
	- Requires Age, PhoneNumber, Resume
	- Google doesn’t give these at login
	- Login would fail

🎤 Interview-Ready Explanation (VERY IMPORTANT)
-----------------------------------------------------------
“We use Google OAuth for authentication and issue our own JWT for authorization.
The Google sub claim uniquely identifies the user and is stored in our database.
All employers are linked via UserId, ensuring strict data ownership.”

✅ Final Summary
-------------------------
| Concern        | Solution           |
| -------------- | ------------------ |
| Google login   | OAuth 2.0          |
| API auth       | JWT                |
| User identity  | Google `sub`       |
| Data ownership | FK (`UserId`)      |
| Validation     | Enums + Fluent API |
| Security       | Claims-based       |

<br>
<br>


Implementation:
---------------------
✅ High-Level Architecture (Why this flow exists)
- 🔑 Google = Identity Provider
- 🔐 Your API = Authorization Server
<br>

- Google only proves WHO the user is.
- Your API decides WHAT the user can do.
<br>

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

2️⃣ API Endpoint: Accept Google ID Token
--------------------------------------------
```
POST /api/auth/google
Authorization: Bearer <google_id_token>
```

3️⃣ Validate Google ID Token (VERY IMPORTANT)
-----------------------------------------------
Why?
- ❌ Anyone can send a fake token
- ✅ You must validate it cryptographically

Install package: 
	- `dotnet add package Google.Apis.Auth`

**Token validation service:**
```
using Google.Apis.Auth;

public class GoogleTokenValidator
{
    public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { "YOUR_GOOGLE_CLIENT_ID" }
        };

        return await GoogleJsonWebSignature.ValidateAsync(token, settings);
    }
}
```
What validation checks
	- ✔ Token signature
	- ✔ Issuer = Google
	- ✔ Audience = your app
	- ✔ Token not expired

4️⃣ Create or Find User in Database
-------------------------------------------------
🔁 This is where your system starts owning the user
🔴 IMPORTANT CHANGE
- No password
- User is identified by GoogleId

**🔁 Create or find user logic**
```
var payload = await _googleValidator.ValidateAsync(googleToken);

var user = await _context.Users
    .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

if (user == null)
{
    user = new User
    {
        GoogleId = payload.Subject,
        Email = payload.Email,
        FullName = payload.Name,
        ProfilePicture = payload.Picture
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();
}
```
**✅ User now exists in YOUR system**

5️⃣ Issue YOUR OWN JWT (Most Important Step)
---------------------------------------------------
Why not Google token?
- Short lived
- Audience = Google
- Cannot add roles / permissions
- Security risk

JWT Claims (Your System)
```
{
  "sub": "12",
  "email": "user@gmail.com",
  "role": "User"
}
```
JWT Creation Code
```
public string GenerateJwt(User user)
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_jwtSettings.Secret));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```
API Response: 
```
{
  "accessToken": "YOUR_SYSTEM_JWT"
}
```

6️⃣ Secure Your API Using JWT
--------------------------------------
Configure JWT Authentication
```
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Secret))
        };
    });
```
Protect Endpoints:
```
[Authorize]
[HttpPost("employers")]
public async Task<IActionResult> CreateEmployer(EmployerDto dto)
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var employer = new Employer
    {
        CompanyName = dto.CompanyName,
        UserId = userId
    };

    _context.Employers.Add(employer);
    await _context.SaveChangesAsync();

    return Ok();
}
```
- ✔ Only logged-in user can create employers
- ✔ Employers are scoped to user

🎯 Interview-Ready Summary
------------------------------------------------------
“We use Google OAuth for identity verification.
After validating the Google ID token, we create or fetch the user in our database and issue our own JWT.
All authorization is handled using our JWT, not Google’s token.”


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
