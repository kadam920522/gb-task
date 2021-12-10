using GlobalBlue.CustomerManager.Application;
using GlobalBlue.CustomerManager.Persistence;
using GlobalBlue.CustomerManager.WebApi.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalBlue.CustomerManager.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHellangProblemDetails(Environment)
                .AddControllers()
                // Adds MVC conventions to work better with the ProblemDetails middleware.
                //.AddProblemDetailsConventions()
                .AddJsonOptions(x => x.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddPasswordHasher();

            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Customer Manager API";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            //Automatically apply migrations
            app.ApplicationServices.ApplyMigrations();

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
