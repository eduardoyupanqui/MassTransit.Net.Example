using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Net.Courier.Activities;
using MassTransit.Net.Courier.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Net.Courier
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddExecuteActivity<ValidateImageActivity, ValidateImageArguments>();
                x.AddExecuteActivity<ProcessImageActivity, ProcessImageArguments>();
                x.AddActivity<DownloadImageActivity, DownloadImageArguments, DownloadImageLog>();

                x.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost/vhost-courier");

                    cfg.ReceiveEndpoint("test_courier_execute", receiveEndpointConfigurator =>
                    {
                        var compnsateUri = new Uri("queue:test_courier_compensate");
                        receiveEndpointConfigurator.ExecuteActivityHost<ValidateImageActivity, ValidateImageArguments>(compnsateUri, provider);
                        receiveEndpointConfigurator.ExecuteActivityHost<ProcessImageActivity, ProcessImageArguments>(compnsateUri, provider);
                        receiveEndpointConfigurator.ExecuteActivityHost<DownloadImageActivity, DownloadImageArguments>(compnsateUri, provider);
                    });

                    cfg.ReceiveEndpoint("test_courier_compensate", receiveEndpointConfigurator =>
                    {
                        receiveEndpointConfigurator.CompensateActivityHost<DownloadImageActivity, DownloadImageLog>(provider);
                    });
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
