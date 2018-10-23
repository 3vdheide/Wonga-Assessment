using Messenger.Contracts;
using Messenger.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Subscriber.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Subscriber
    {
        private readonly ISubscribeService _subscripbeService;
        private readonly ILogger<Subscriber> _logger;
        private readonly AppSettings _config;

        public Subscriber(ISubscribeService subscripbeService,
            IOptions<AppSettings> config,
            ILogger<Subscriber> logger)
        {
            _subscripbeService = subscripbeService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("Please enter a routing key:");
                string routingKey;
                _logger.LogInformation("Capturing user input.");

                while (String.IsNullOrEmpty(routingKey = Console.ReadLine()))
                {
                    Console.WriteLine("You haven't entered a routing key. Please try again.");
                }

                RecieveMessage(routingKey);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error has occured: {ex.Message}";
                _logger.LogError(errorMessage);
                Console.WriteLine(errorMessage);
                Console.ReadLine();
            }
        }

        private void RecieveMessage(string routingKey)
        {
            var model = new RabbitModel
            {
                RoutingKey = routingKey,
                HostName = _config.HostName
            };
            
            _subscripbeService.RecieveMessage(model);
        }
    }
}
