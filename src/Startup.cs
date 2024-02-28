using System;
using System.Reflection;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Exercise1.Cloud;
using Exercise1.Commands.FetchData;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Exercise1.Startup))]

namespace Exercise1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            var configuration = builder.GetContext().Configuration;
            var connectionString = configuration["AzureStorageConnectionString"];
            try
            {
                var tableServiceClient = new TableServiceClient(connectionString);
                var blobServiceClient = new BlobServiceClient(connectionString);
                tableServiceClient.GetProperties();
                blobServiceClient.GetProperties();
                builder.Services.AddSingleton(tableServiceClient);
                builder.Services.AddSingleton(blobServiceClient);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to Azure Storage. " +
                    $"Ensure that you have a running Azurite instance. " +
                    $"Execute 'azurite' command in a terminal to run the azure storage emulator. " +
                    $"Error: {ex.Message}");
            }
            builder.Services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();
            builder.Services.AddScoped<IPublicApisClient, PublicApisClient>();
            builder.Services.AddScoped<ICloudClient, CloudClient>();
        }
    }
}

