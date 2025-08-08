-Created New project from template .NET Core WebAPI
-Copied ClientApp from another project 
-Using CLI downloaded 
	dotnet add package Microsoft.AspNetCore.SpaProxy --version 6.0.5
-In LaunchSettings.json added "ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy" in profiles env variables
-In .csproj added
	<SpaRoot>ClientApp\</SpaRoot>
    	<SpaProxyServerUrl>https://localhost:44488</SpaProxyServerUrl>
    	<SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>
-Added a NUnit Test Project from a template in a same solution

-dotnet add package Microsoft.AspNetCore.JsonPatch 
dotnet add package AspNetCore.Security.CAS 
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection 
dotnet add package Microsoft.EntityFrameworkCore.Design 
dotnet add package Swashbuckle.AspNetCore 
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson  
dotnet add package Morcatko.AspNetCore.JsonMergePatch.SystemText
dotnet add package Microsoft.EntityFrameworkCore.Relational
dotnet add package AutoMapper 
dotnet add package Microsoft.EntityFrameworkCore.InMemory 
dotnet add package Pomelo.EntityFrameworkCore.MySql 
dotnet add package Pomelo.EntityFrameworkCore.MySql.Json.Newtonsoft 
dotnet add package Microsoft.AspNetCore.OData     I want to download these packages for .net version 6 