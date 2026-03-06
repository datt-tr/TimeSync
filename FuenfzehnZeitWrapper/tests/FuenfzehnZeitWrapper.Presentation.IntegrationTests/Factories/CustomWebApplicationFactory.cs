using FuenfzehnZeitWrapper.Presentation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuenfzehnZeitWrapper.Presentation.IntegrationTests.Factories;
    public sealed class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        internal Configurations Configurations { get; private set; } = new Configurations();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var configs = scope.ServiceProvider.GetRequiredService<IOptions<Configurations>>();
                Configurations = configs.Value;
            });
        }
    }