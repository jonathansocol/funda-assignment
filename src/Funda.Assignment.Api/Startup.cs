using System;
using System.Threading;
using Funda.Assignment.Application.Interfaces;
using Funda.Assignment.Application.Services;
using Funda.Assignment.Infrastructure.Handlers;
using Funda.Assignment.Infrastructure.Services;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Funda.Assignment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IListingHttpClient, FundaListingHttpClient>(client =>
                    client.BaseAddress =
                        new Uri($"{Configuration["HttpClientSettings:BaseUrl"]}{Configuration["HttpClientSettings:ApiKey"]}")
                ).AddHttpMessageHandler(() =>
                    new RequestsLimitHttpMessageHandler(
                        int.Parse(Configuration["HttpClientSettings:MaxRequests"]),
                        int.Parse(Configuration["HttpClientSettings:Timelapse"])
                    )
                ).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen();

            services.AddScoped<IAgentsRankingService, AgentsRankingService>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
