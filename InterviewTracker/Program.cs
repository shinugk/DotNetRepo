using InterviewTracker.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

bool isDev = true;
////Below Middleware used for Http logging
//builder.Services.AddHttpLogger(builder.Configuration);

//// Add authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//    options.CallbackPath = "/api/v1/signin-google"; // Must match Google Cloud Console
//});
//builder.Services.AddAuthorization();

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


//-------------------------------------------------------------------------------
var app = builder.Build();
//-------------------------------------------------------------------------------



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//TODO: Do we need this? Will other platform services be considered different origins if they make calls from their UI directly to our service?
app.UseCors("AllowOrigin");

// (use the options configured above in AddSession)
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

//This is for dev env
app.MapControllers()/*.AllowAnonymous()*/;

//app.MapGet("/", () => "Hello World!");

//app.MapGet("/login", async context =>
//{
//    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
//    {
//        RedirectUri = "/profile"
//    });
//});

//app.MapGet("/profile", async context =>
//{
//    var user = context.User;
//    if (user.Identity?.IsAuthenticated ?? false)
//    {
//        var email = user.FindFirst(ClaimTypes.Email)?.Value;
//        var name = user.FindFirst(ClaimTypes.Name)?.Value;
//        await context.Response.WriteAsync($"Logged in as {name} ({email})");
//    }
//    else
//    {
//        context.Response.Redirect("/login");
//    }
//});

app.MapFallbackToFile("index.html");

//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio-code
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
