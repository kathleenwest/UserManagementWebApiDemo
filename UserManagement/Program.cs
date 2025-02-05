using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Middleware;

namespace UserManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new web application builder
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container

            // Add NewtonsoftJson support to controllers
            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddLogging(config =>
            {
                config.AddConsole();
                // Add other loggers here if needed (e.g., Debug, EventSource, etc.)
            });

            // SQLite Database Provider Setup
            // Connection string
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("UserDb")));

            // Add Repositories and Services for Dependency Injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Swagger/OpenAPI 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Simple User Management Api",
                    Description = "A simple user management api",
                    Contact = new OpenApiContact
                    {
                        Name = "Kathleen West",
                        Email = "hello.kathleen.west@gmail.com",
                        Url = new Uri("https://portfolio.katiegirl.net")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    Version = "v1"
                });

                // Generate the xml docs that Swagger will utilize
                string xmlFilePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilePath);
                c.IncludeXmlComments(xmlPath);

                // Add Descriptive Api Names for Client References 
                c.CustomOperationIds(apiDescription =>
                {
                    return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                });

            })
            .AddSwaggerGenNewtonsoftSupport(); // Add support for JsonPatch here

            // Create the web application
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline

            // Change configurations based on environmental settings
            // HINT: See Project Properties --> Debug --> Launch Profiles --> Select
            // Add ASPNETCORE_ENVIRONMENT = "Development"
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple User Management Api v1");

                    // Displays the Operational Name of the Api endpoints
                    c.DisplayOperationId();
                });
            }
            else
            {
                // By-Pass the Developer Exception Page
                app.UseExceptionHandler("/error");
            }

            // Redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Add custom logging middleware here
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            // Let's get this party started!
            app.Run();
        }
    }
}