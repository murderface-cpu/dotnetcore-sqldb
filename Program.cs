using DotNetCoreSqlDb.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Azure logging (already fine)
builder.Logging.AddAzureWebAppDiagnostics();

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure SQL Database Connection
// It will automatically use the connection string from Azure App Service
var connectionString = builder.Configuration.GetConnectionString("MyDbConnection");

builder.Services.AddDbContext<MyDatabaseContext>(options =>
    options.UseSqlServer(connectionString));

// Automatically apply migrations on startup
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyDatabaseContext>();
    db.Database.Migrate();
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todos}/{action=Index}/{id?}");

app.Run();
