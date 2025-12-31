using InterviewTracker.Database;
using InterviewTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

bool isDev = true;

////Below Middleware used for Http logging
//builder.Services.AddHttpLogger(builder.Configuration);


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



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Add session dependencies
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddDataProtection(); // Optional but avoids your error explicitly
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//db middleware to configure database
builder.Services.ConfigureDatabase(builder.Configuration, isDev);

// Add CORS services (Need to come back to this)
var allowedOrigins = builder.Configuration.GetSection("Frontend:Urls").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Add all services here that need in DI in any class
builder.Services.AddScoped<JwtTokenService>();   // ✅ REQUIRED




//-------------------------------------------------------------------------------
var app = builder.Build();
//-------------------------------------------------------------------------------



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

//This is because we hosted ng app on render & that needs this header
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();

//TODO: Do we need this? Will other platform services be considered different origins if they make calls from their UI directly to our service?
//app.UseCors("AllowOrigin");

// Use the CORS policy
app.UseCors("FrontendPolicy");

// (use the options configured above in AddSession)
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

//This is for dev env
app.MapControllers()/*.AllowAnonymous()*/;

//app.MapGet("/", () => "Hello World!");

//app.MapFallbackToFile("index.html");

//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio-code
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
