using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Exercise1.Commands.FetchData;
using Microsoft.Extensions.Logging;

namespace Exercise1.Cloud
{
    public class CloudClient : ICloudClient
    {
        private readonly TableServiceClient _tableService;
        private readonly BlobServiceClient _blobService;
        private readonly ILogger _logger;
        private const string tableName = "logs";
        private const string containerName = "payloads";

        public CloudClient(TableServiceClient tableService,
            BlobServiceClient blobService,
            ILogger<CloudClient> logger)
        {
            _tableService = tableService;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task Upsert(PublicApisResponse apiResponse)
        {
            var blobContainer = _blobService.GetBlobContainerClient(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.UploadBlobAsync(apiResponse.Id.ToString(),
                new BinaryData(JsonSerializer.Serialize(apiResponse)));

            _logger.LogInformation($"A blob ({apiResponse.Id}) has been uploaded.");
        }

        public async Task Log(Log log)
        {
            var tableClient = _tableService.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            var entity = LogEntity.FromLog(log, "1");

            await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
            _logger.LogInformation($"A table log has been uploaded. " +
                $"{nameof(entity.PartitionKey)}:{entity.PartitionKey}," +
                $"{nameof(entity.RowKey)}:{entity.RowKey}.");
        }

        public async Task<(string payload, bool found)> Fetch(Guid id)
        {
            var blobContainer = _blobService.GetBlobContainerClient(containerName);
            await blobContainer.CreateIfNotExistsAsync();
            var blobClient = blobContainer.GetBlobClient(id.ToString());
            if (await blobClient.ExistsAsync())
                return ((await blobClient.DownloadContentAsync()).Value.Content.ToString(), found: true);
            return (string.Empty, found: false);
        }

        public async Task<IEnumerable<Log>> FetchLogs(DateTimeOffset from, DateTimeOffset to)
        {
            var tableClient = _tableService.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();

            string fromRowKey = LogEntity.GenerateRowKey(from);
            string toRowKey = LogEntity.GenerateRowKey(to);

            var logEntities = tableClient.QueryAsync<LogEntity>(entity =>
                entity.PartitionKey == "1" &&
                entity.RowKey.CompareTo(fromRowKey) >= 0 &&
                entity.RowKey.CompareTo(toRowKey) <= 0);

            var logs = new List<Log>();
            await foreach (var item in logEntities)
            {
                logs.Add(item.ToLog());
            }
            return logs;
        }
    }
}

