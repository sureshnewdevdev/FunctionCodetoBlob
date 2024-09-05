using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp3
{
    public class Function1
    {
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=newaccountforprp;AccountKey=mOUYvug1ysQX7igkPoRebeTDtHLn2o/9z2H1HlUX63/QTe8EV8yBxSwjuPK6vzjRv9RN8PLNgrdg+AStd//hoQ==;EndpointSuffix=core.windows.net";

        [Function(nameof(Function1))]
        public async Task Run(
            [BlobTrigger("source/{name}", Connection = "AzureWebJobsStorage")] Stream sourceBlobStream,
            string name,
            FunctionContext context)
        {
            //System.Diagnostics.Process.GetProcesses
            var logger = context.GetLogger<Function1>();
            logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name}");

            // Step 1: Initialize BlobServiceClient using the connection string
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Step 2: Get a reference to the target container
            var targetContainer = blobServiceClient.GetBlobContainerClient("target");

            // Ensure the target container exists
            await targetContainer.CreateIfNotExistsAsync();

            // Step 3: Get a reference to the target blob
            var targetBlob = targetContainer.GetBlobClient(name);

            // Step 4: Upload the blob from the source to the target
            await targetBlob.UploadAsync(sourceBlobStream, overwrite: true);

            logger.LogInformation($"Blob {name} has been copied to the target container.");
        }

        public async Task Run2(
            [BlobTrigger("source/{name}", Connection = "AzureWebJobsStorage")] Stream sourceBlobStream,
            string name,
            FunctionContext context)
        {
           
        }
    }
}
