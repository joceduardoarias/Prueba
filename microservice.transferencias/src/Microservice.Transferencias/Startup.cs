using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Formatters;
using HealthChecks.UI.Client;
using Newtonsoft.Json.Converters;
using Open.Serialization.Json.Newtonsoft;
using System.Text.Json.Serialization;
using WyD.Mess;
using WyD.Mess.Discovery.Consul;
using WyD.Mess.Docs.Swagger;
using WyD.Mess.LoadBalancing.Fabio;
using WyD.Mess.Tracing.Jaeger;
using WyD.Mess.WebApi;
using WyD.Mess.WebApi.Swagger;
using Steeltoe.Management.Tracing;

namespace Microservice.Transferencias
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedTracing();

            var builder = services.AddMess(Configuration)
                .AddWebApi()
                .AddConsul()
                .AddFabio()
                .AddJaeger()
                .AddWebApiSwaggerDocs();

            builder.Services.AddControllers(opt =>
            {
                // remove formatter that turns nulls into 204 - No Content responses
                // this formatter breaks Angular's Http response JSON parsing
                // https://weblog.west-wind.com/posts/2020/Feb/24/Null-API-Responses-and-HTTP-204-Results-in-ASPNET-Core
                opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
               .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
               .AddNewtonsoftJson(opt => opt.SerializerSettings.AddConverter(new StringEnumConverter()));

            builder.Services.AddMvc()
                .AddNewtonsoftJson();

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson();
            builder.Build();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMess()
               .UseWebApi()
               .UseJaeger()
               .UseSwaggerDocs();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecks("/health", new HealthCheckOptions
                //{
                //    Predicate = _ => true,                                         
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse     //TODO
                //});
                //endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                //{
                //    Predicate = r => r.Name.Contains("self")
                //});
                endpoints.MapGet("/ping", async context =>
                {
                    await context.Response.WriteAsync("pong");
                });
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(Configuration.GetValue<string>("mess:name"));
                });
            });
        }
    }
}
