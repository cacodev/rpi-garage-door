﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Bifrost.Devices.Gpio.Core;
using Bifrost.Devices.Gpio.Abstractions;
using Bifrost.Devices.Gpio;
using rpi_garage_door.Services;
using rpi_garage_door.Models;

namespace rpi_garage_door
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
            services.AddApplicationInsightsTelemetry();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddLogging();

            services.AddMvc();

            services.AddSingleton<IDoorMonitoringService, DoorMonitoringService>();
            services.AddSingleton<IDoorStateService, DoorStateService>();
            services.AddSingleton<IDoorEventService, DoorEventService>();
            services.AddSingleton<IHostedService, SchedulerService>();
            // services.AddSingleton<IHostedService, DoorQueueService>();
            services.AddSingleton(typeof(IGpioController), GpioController.Instance);
            
            services.AddSingleton<IPinService, PinService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}
