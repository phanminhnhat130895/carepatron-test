using Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace Application.Common.Helpers
{
    public class ServiceBusSettings
    {
        public ServiceBusSettings(IConfiguration configuration) 
        {
            this.ConnectionString = configuration.GetValue<string>("ServiceBusSettings:ConnectionString");
            this.QueueName = configuration.GetValue<string>("ServiceBusSettings:QueueName");
        }
        public string QueueName;
        public string ConnectionString;
    }

    public interface IServiceBusHelper
    {
        Task SendMessage(string data);
    }

    public class ServiceBusHelper : IServiceBusHelper
    {
        private readonly ServiceBusSettings _settings;

        public ServiceBusHelper(ServiceBusSettings settings) { _settings = settings; }

        // the client that owns the connection and can be used to create senders and receivers
        ServiceBusClient client;

        // the sender used to publish messages to the queue
        ServiceBusSender sender;

        public async Task SendMessage(string data)
        {
            client = new ServiceBusClient(_settings.ConnectionString);

            sender = client.CreateSender(_settings.QueueName);

            ServiceBusMessage message = new ServiceBusMessage(data);

            await sender.SendMessageAsync(message);

            Log.Information("Send message to service bus quere: {@data}", data);
        }
    }
}
