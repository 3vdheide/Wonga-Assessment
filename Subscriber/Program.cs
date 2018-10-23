using Messenger.Contracts;
using Messenger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Subscriber.Models;
using System;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Subscriber>().Run();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(new LoggerFactory()
                     .AddDebug());
            services.AddLogging();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<AppSettings>(configuration.GetSection("Configuration"));

            services.AddSingleton<ISubscribeService, MessageService>();
            services.AddTransient<Subscriber>();
        }
    }
}
