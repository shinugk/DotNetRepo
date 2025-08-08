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

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