using HandleCreateUpdateClientFunction.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(HandleCreateUpdateClientFunction.Startup))]
namespace HandleCreateUpdateClientFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDocumentRepository, DocumentRepository>();
            builder.Services.AddTransient<IEmailRepository, EmailRepository>();
        }
    }
}
