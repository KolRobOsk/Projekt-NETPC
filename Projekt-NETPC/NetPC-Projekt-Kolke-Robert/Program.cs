using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using NetPC_Projekt_Kolke_Robert.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

namespace sqltest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "<Kontakty.sqlite>",
                InitialCatalog = "<your_database>"
            };

            var connectionString = builder.ConnectionString;

            try
            {
                await using var connection = new SqlConnection(connectionString);
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                await connection.OpenAsync();

                var sql = "SELECT name, collation_name FROM sys.databases";
                await using var command = new SqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                }
            }
            catch (SqlException e) when (e.Number == /* specific error number */)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }
    }
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


