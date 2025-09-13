using Azure.Core.Extensions;
using Azure.Data.Tables;
using Microsoft.Extensions.Azure;
using Socializer.Database.NoSql.Extensions;

namespace Socializer.Database.NoSql.Extensions;

public static class AzureClientFactoryBuilderExtensions
{
    public static IAzureClientBuilder<TableServiceClient, TableClientOptions> AddTableServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi = true)
    {
        if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
        {
            return builder.AddTableServiceClient(serviceUri);
        }
        else
        {
            return TableClientBuilderExtensions.AddTableServiceClient(builder, serviceUriOrConnectionString);
        }
    }
}