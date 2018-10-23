using Messenger.Contracts;
using Messenger.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Publisher.Models;
using RabbitMQ.Client;
using System;

namespace Publisher
{
    public class Publisher
    {
        private readonly IPublisherService _publishService;
        private readonly AppSettings _config;
        private readonly ILogger<Publisher> _logger;

        public Publisher(IPublisherService publishService,
            IOptions<AppSettings> config,
            ILogger<Publisher> logger)
        {
            _publishService = publishService;
            _config = config.Value;
            _logger = logger;
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

                bool sendAnotherMessage = true;
                while (sendAnotherMessage)
                {

                    Console.WriteLine("Please enter your name:");

                    _logger.LogInformation("Capturing user input.");
                    string name;
                    while (String.IsNullOrEmpty(name = Console.ReadLine()))
                    {
                        Console.WriteLine("You haven't entered a name. Please try again.");
                    }

                    _logger.LogInformation("Sending message the message service.");
                    var message = $"Hello my name is, {name}";
                    Console.WriteLine($"Sending: {message}");
                    PublishMessage(message, name, routingKey);

                    Console.WriteLine("");
                    string answer;
                    do
                    {
                        Console.WriteLine("Do you want to send another message? Y/N");
                        answer = Console.ReadLine()?.ToLower();

                        if (answer == "y" || answer == "n")
                        {
                            sendAnotherMessage = answer == "y";
                            break;
                        }
                    }
                    while (true);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error has occured: {ex.Message}";
                _logger.LogError(errorMessage);
                Console.WriteLine(errorMessage);
                Console.ReadLine();
            }

        }

        private void PublishMessage(string message, string name, string routingKey)
        {
            var messageModel = new MessageModel
            {
                Message = message,
                Name = name
            };

            var rabbitModel = new RabbitModel
            {
                SerializedMessage = JsonConvert.SerializeObject(messageModel),
                RoutingKey = routingKey,
                HostName = _config.HostName
            };

            _publishService.PublishMessage(rabbitModel);
        }
    }
}
