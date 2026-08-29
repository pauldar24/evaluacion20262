using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using evaluacion20262.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = ResolveConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port) ||
    string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase))
{
    app.Urls.Add("http://0.0.0.0:" + (port ?? "10000"));
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

string ResolveConnectionString(string? connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        connectionString = "Data Source=tecnogas.db";
    }

    var sqliteBuilder = new SqliteConnectionStringBuilder(connectionString);
    if (!string.IsNullOrWhiteSpace(sqliteBuilder.DataSource) && Path.IsPathRooted(sqliteBuilder.DataSource))
    {
        return sqliteBuilder.ConnectionString;
    }

    string databaseDirectory;
    if (string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase))
    {
        databaseDirectory = Directory.Exists("/var/render") ? "/var/render" : Path.GetTempPath();
    }
    else
    {
        databaseDirectory = Directory.GetCurrentDirectory();
    }

    Directory.CreateDirectory(databaseDirectory);
    var fileName = string.IsNullOrWhiteSpace(sqliteBuilder.DataSource) ? "tecnogas.db" : Path.GetFileName(sqliteBuilder.DataSource);
    sqliteBuilder.DataSource = Path.Combine(databaseDirectory, fileName);
    return sqliteBuilder.ConnectionString;
}
