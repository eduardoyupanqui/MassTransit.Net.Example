using MassTransit.Net.EventHandling.Application.IntegrationEvents.EventHandling;
using MassTransit.Net.EventHandling.Application.IntegrationEvents.Events;
using MassTransit.Net.EventHandling.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Net.EventHandling.Infrastructure.Extensions;

namespace MassTransit.Net.EventHandling
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
            services.AddCustomConfigureEventBusJob(Configuration);
            services.AddCustomRegisterEventBusJob(Configuration);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MassTransit.Net.EventHandling", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MassTransit.Net.EventHandling v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomConfigureEventBusJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MasstransitConfig>(configuration.GetSection(nameof(MasstransitConfig)));

            //
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                
                x.AddConsumer<JobEnqueueIntegrationEventHandler>();
                x.AddConsumer<JobStartedIntegrationEventHandler>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                //x.AddBus(provider => Bus.Factory.CreateUsingAmazonSqs(cfg =>
                {
                    var config = provider.GetRequiredService<IOptions<MasstransitConfig>>().Value;
                    cfg.Host(config.Host, c =>
                    //cfg.Host(config.Region, c =>
                    {
                        //For RabbitMQ
                        c.Username(config.UserName);
                        c.Password(config.Password);

                        //c.PublisherConfirmation = false;
                        ////For Amazon SQS
                        //c.AccessKey(config.AccessKey);
                        //c.SecretKey(config.SecretKey);
                        //// specify a scope for all queues
                        //c.Scope(config.Scope);
                        //// scope topics as well
                        //c.EnableScopedTopics();

                    });
                    cfg.MessageTopology.SetEntityNameFormatter(new FancyNameFormatter(cfg.MessageTopology.EntityNameFormatter));

                    //cfg.ReceiveEndpoint(typeof(JobCommand).Name.ToUnderscoreCase(), ep =>
                    //{
                    //    ep.Consumer<JobEnqueueIntegrationEventHandler>(provider);

                    //    //Asegurar el procesamiento de forma secuencial FIFO
                    //    //ep.PrefetchCount = (ushort)config.PrefetchCount;
                    //    //ep.UseConcurrencyLimit(1);
                    //});

                    cfg.ReceiveEndpoint(typeof(JobEnqueueIntegrationEvent).Name.ToUnderscoreCase().ToConcatHost(configuration["HostName"]), e =>
                    {
                        //e.WaitTimeSeconds = 1;
                        //e.PrefetchCount = 1;
                        e.Consumer<JobEnqueueIntegrationEventHandler>(provider);
                        //EndpointConvention.Map<JobEnqueueIntegrationEvent>(e.InputAddress);
                    });
                    cfg.ReceiveEndpoint(typeof(JobStartedIntegrationEvent).Name.ToUnderscoreCase().ToConcatHost(configuration["HostName"]), e =>
                    {
                        e.Consumer<JobStartedIntegrationEventHandler>(provider);
                        //e.ConfigureConsumer<JobStartedIntegrationEventHandler>(provider, e => { e.Message<JobStartedIntegrationEvent>(f=> { })});
                        //EndpointConvention.Map<JobStartedIntegrationEvent>(e.InputAddress);
                    });

                }));

            });

            return services;

        }

        public static IServiceCollection AddCustomRegisterEventBusJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransitHostedService();
            //
            return services;
        }
    }
}
