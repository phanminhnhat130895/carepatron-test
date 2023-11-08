using Azure;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Serilog;

namespace Application.Common.Helpers
{
    public class ServiceBusSettings
    {
        public string QueueName;
        public string ConnectionString;
    }

    public interface IServiceBusHelper
    {
        Task SendMessage(string data, string queueName, string connectionString);
    }

    public class ServiceBusHelper : IServiceBusHelper
    {
        // the client that owns the connection and can be used to create senders and receivers
        ServiceBusClient client;

        // the sender used to publish messages to the queue
        ServiceBusSender sender;

        public async Task SendMessage(string data, string queueName, string connectionString)
        {
            client = new ServiceBusClient(connectionString);

            sender = client.CreateSender(queueName);

            ServiceBusMessage message = new ServiceBusMessage(data);

            await sender.SendMessageAsync(message);

            Log.Information("Send message to service bus quere: {@data}", data);
        }
    }
}
