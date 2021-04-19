using System;
using Impostor.Api.Net.Manager;
using Impostor.Api.Plugins;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Boot.Codes
{
    public class ExamplePluginStartup : IPluginStartup
    {
        public void ConfigureHost(IHostBuilder host) { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGameCodeManager, GameCodeManager>();
        }
    }
}
