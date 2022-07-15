using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit.Net.Quartz.Consumers;

namespace MassTransit.Net.Quartz
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ScheduleNotificationConsumer>();
                x.AddConsumer<NotificationConsumer>();
                x.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.Host("localhost");

                    //1) Para utilizar MassTransit.QuartzService
                    cfg.UseMessageScheduler(new Uri("rabbitmq://localhost/quartz_scheduler"));
                    //2) Para utilizar MassTransit.Quartz In Memory, para pruebas [No Recomendado]
                    //cfg.UseInMemoryScheduler();

                    cfg.ReceiveEndpoint("schedule_test_queue", ep =>
                    {
                        ep.Consumer<ScheduleNotificationConsumer>(provider);
                        ep.Consumer<NotificationConsumer>(provider);
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
