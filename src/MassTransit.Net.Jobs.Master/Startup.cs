using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Master.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransit.Net.Jobs.Master
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<JobStartedConsumer>();
                x.AddConsumer<JobTaskCompletedConsumer>();
                x.AddConsumer<JobCompletedConsumer>();
                x.AddConsumer<JobFailedConsumer>();
                x.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost/vhost-job");

                    cfg.ReceiveEndpoint(typeof(JobEvent).Name.ToUnderscoreCase(), ep =>
                    {
                        ep.Consumer<JobStartedConsumer>(provider);
                        ep.Consumer<JobTaskCompletedConsumer>(provider);
                        ep.Consumer<JobCompletedConsumer>(provider);
                        ep.Consumer<JobFailedConsumer>(provider);
                        
                        //Asegurar el procesamiento de forma secuencial FIFO
                        ep.PrefetchCount = 1;
                        ep.UseConcurrencyLimit(1);
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
                    var bus = context.RequestServices.GetService<IBus>();
                    context.Response.ContentType = "application/json";
                    string jsonString = JsonSerializer.Serialize(bus.GetProbeResult());
                    await context.Response.WriteAsync(jsonString); ;
                });
            });
        }
    }
}
