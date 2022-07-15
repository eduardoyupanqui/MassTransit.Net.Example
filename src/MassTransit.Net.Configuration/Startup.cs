using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using MassTransit.Net.Configuration.Consumers;
using MassTransit.Net.Configuration.Messages;
using MassTransit.Net.Configuration.Observers;

namespace MassTransit.Net.Configuration
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddMvc();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderConsumer>();

                x.UsingRabbitMq((provider, cfg) =>
                {

                    cfg.Host("rabbitmq://localhost");
                    //cfg.Host(new Uri("rabbitmq://a-machine-name/a-virtual-host"), host =>
                    //{
                    //    host.Username("username");
                    //    host.Password("password");
                    //});

                    cfg.ReceiveEndpoint("submit-order", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("customer_update_queue", e =>
                    {
                        // simple
                        e.Consumer<UpdateCustomerConsumer>();

                        // an anonymous factory method
                        //e.Consumer(() => new YourConsumer());

                        // an existing consumer factory for the consumer type
                        //e.Consumer(consumerFactory);

                        // a type-based factory that returns an object (container friendly)
                        //e.Consumer(consumerType, type => Activator.CreateInstance(type));

                        // an anonymous factory method, with some middleware goodness
                        //e.Consumer(() => new YourConsumer(), x =>
                        //{
                        //    // add middleware to the consumer pipeline
                        //    x.UseExecuteAsync(context => Console.Out.WriteLineAsync("Consumer created"));
                        //});


                        //Handler
                        //https://masstransit-project.com/usage/consumers.html#handler
                        e.Handler<UpdateCustomerAddress>(context => { return Console.Out.WriteLineAsync($"Update customer address received: {context.Message.CustomerId}"); });

                        //Observer
                        //https://masstransit-project.com/usage/consumers.html#observer
                        e.Observer<CustomerAddressUpdated>(new CustomerAddressUpdatedObserver());

                    });


                });
            });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World MassTransit.Net!");
                });
                endpoints.MapControllers();

                // The readiness check uses all registered checks with the 'ready' tag.
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    // Exclude all checks and return a 200-Ok.
                    Predicate = (_) => false
                });
            });

        }
    }
}
