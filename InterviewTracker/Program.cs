var builder = WebApplication.CreateBuilder(args);

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
app.MapControllers().AllowAnonymous();

app.MapFallbackToFile("index.html");

//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio-code
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
