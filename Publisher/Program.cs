using Messenger.Contracts;
using Messenger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Publisher.Models;
using System;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Publisher>().Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new LoggerFactory()
                  .AddDebug());
            services.AddLogging();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appSettings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<AppSettings>(configuration.GetSection("Configuration"));


            services.AddSingleton<IPublisherService, MessageService>();
            services.AddTransient<Publisher>();
        }
    }
}
