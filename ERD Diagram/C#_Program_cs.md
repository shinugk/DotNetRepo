
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Environment
bool isDev = builder.Environment.IsDevelopment();

//Localization
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});


//Database (EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Authentication (JWT)
var secret = Encoding.ASCII.GetBytes("THIS_IS_SUPER_SECRET_KEY");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//Authorization
builder.Services.AddAuthorization();


//OData Configuration (EDM)
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Product>("Products");


//Controllers + Filters + JSON + OData
builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
        options.Filters.Add<ValidationFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddOData(options =>
    {
        options
            .AddRouteComponents("odata", odataBuilder.GetEdmModel())
            .Select()
            .Filter()
            .Expand()
            .OrderBy()
            .Count()
            .SetMaxTop(100);
    });



//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});



//Dependency Injection (All Lifetimes)
builder.Services.AddTransient<IEmailService, EmailService>(); // Transient
builder.Services.AddScoped<IProductService, ProductService>(); // Scoped
builder.Services.AddSingleton<ICacheService, MemoryCacheService>(); // Singleton

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Session
builder.Services.AddSession();

//Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

//Hosted Background Service
builder.Services.AddHostedService<CleanupBackgroundService>();

//Middleware DI
builder.Services.AddScoped<RequestLoggingMiddleware>();


var app = builder.Build();


---------Middleware Pipeline--------

if (!isDev)
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();



//Swagger UI
app.UseSwagger();
app.UseSwaggerUI();


//Localization Middleware
var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures("en", "fr")
    .AddSupportedUICultures("en", "fr");

app.UseRequestLocalization(localizationOptions);


//Endpoints
app.MapControllers();


//Database Migration at Startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.Run();
