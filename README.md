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
