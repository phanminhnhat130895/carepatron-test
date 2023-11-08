using System;
using System.Net.Http;
using System.Threading.Tasks;
using HandleCreateUpdateClientFunction.Repositories;
using HandleCreateUpdateClientFunction.Utilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Polly;

namespace HandleCreateUpdateClientFunction
{
    public class Function
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IEmailRepository _emailRepository;

        public Function(IDocumentRepository documentRepository, IEmailRepository emailRepository)
        {
            _documentRepository = documentRepository;
            _emailRepository = emailRepository;
        }

        [FunctionName("HandleCreateUpdateClientFunction")]
        public async Task Run([ServiceBusTrigger("Create-Update-Client", Connection = "")]string myQueueItem, ILogger log)
        {
            var retryPolicy = RetryPolicy.GetRetryPolicy(log);

            var taskCreateDocument = retryPolicy.ExecuteAsync(async () =>
            {
                await _documentRepository.SyncDocumentsFromExternalSource(myQueueItem);
            });

            var taskSendEmail = retryPolicy.ExecuteAsync(async () =>
            {
                await _emailRepository.Send(myQueueItem, "Hi there - welcome to my Carepatron portal.");
            });

            await Task.WhenAll(taskCreateDocument, taskSendEmail);
        }
    }
}
