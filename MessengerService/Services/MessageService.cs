using Messenger.Contracts;
using Messenger.Models;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Messenger.Extensions;

namespace Messenger.Services
{
    public class MessageService : IPublisherService, ISubscribeService
    {
        private const string ExchangeName = "wonga_topic";
        private readonly ILogger<MessageService> _logger;

        public MessageService(ILogger<MessageService> logger)
        {
            _logger = logger;
        }

        public void PublishMessage(RabbitModel rabbitModel)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = rabbitModel.HostName };

                _logger.LogInformation($"Creating connection");
                using (var connection = factory.CreateConnection())
                {
                    _logger.LogInformation($"Creating channel");
                    using (var channel = connection.CreateModel())
                    {

                        _logger.LogInformation($"Declaring exchange: {ExchangeName}");
                        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic);

                        var body = Encoding.UTF8.GetBytes(rabbitModel.SerializedMessage);

                        _logger.LogInformation($"Publishing serialized message: {rabbitModel.SerializedMessage}");
                        channel.BasicPublish(exchange: ExchangeName,
                                             routingKey: rabbitModel.RoutingKey,
                                             basicProperties: null,
                                             body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void RecieveMessage(RabbitModel rabbitModel)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = rabbitModel.HostName };

                _logger.LogInformation($"Creating connection");
                using (var connection = factory.CreateConnection())
                {
                    _logger.LogInformation($"Creating channel");
                    using (var channel = connection.CreateModel())
                    {
                        _logger.LogInformation($"Declaring exchange: {ExchangeName}");
                        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic);

                        var queueName = rabbitModel.RoutingKey;
                        _logger.LogInformation($"Declaring queue: {queueName}");
                        channel.QueueDeclare(queueName);

                        channel.QueueBind(queue: queueName,
                                          exchange: ExchangeName,
                                          routingKey: rabbitModel.RoutingKey);

                        Console.WriteLine("Waiting for messages.");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            var messageModel = JsonConvert.DeserializeObject<MessageModel>(message);

                            if (messageModel.Name.ValidateName())
                            {
                                Console.WriteLine($"Recieved: {messageModel.Message}");
                                Console.WriteLine($"Hello {messageModel.Name}, I am your father!");
                            }
                            else {
                                Console.WriteLine("An empty name was recieved");
                            }
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };

                        _logger.LogInformation($"Consuming queue: {queueName}");
                        channel.BasicConsume(queue: queueName,
                                             autoAck: false,
                                             consumer: consumer);
                        Console.ReadLine(); 
                    }
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            } 
        }



        private void LogException(Exception ex)
        {
            var errorMessage = $"An error has occured: {ex.Message}";
            _logger.LogError(errorMessage);
            Console.WriteLine(errorMessage);
        }
    }

}
