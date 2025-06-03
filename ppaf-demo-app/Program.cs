using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace AzureCosmosDBPPAFDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration from appsettings.json (ensure this file exists and is properly configured)
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Read Cosmos DB settings from configuration
            var cosmosConfig = config.GetSection("CosmosDb");
            string? endpoint = cosmosConfig["Endpoint"];
            string? key = cosmosConfig["Key"];
            string? databaseId = cosmosConfig["DatabaseId"];
            string? containerId = cosmosConfig["ContainerId"];

            // Validate that all required Cosmos DB settings are present
            if (string.IsNullOrWhiteSpace(endpoint) ||
                string.IsNullOrWhiteSpace(key) ||
                string.IsNullOrWhiteSpace(databaseId) ||
                string.IsNullOrWhiteSpace(containerId))
            {
                Console.WriteLine("CosmosDb configuration is missing in appsettings.json.");
                return;
            }

            // Use null-forgiving operator (!) after null checks to safely assign to non-nullable variables
            string safeEndpoint = endpoint!;
            string safeKey = key!;
            string safeDatabaseId = databaseId!;
            string safeContainerId = containerId!;

            // CosmosClientOptions allows you to customize client behavior
            var clientOptions = new CosmosClientOptions
            {
                ApplicationName = "ppaf-demo",
                ConnectionMode = ConnectionMode.Direct,
                // Update the ApplicationPreferredRegions list below to match your preferred Azure regions..
                ApplicationPreferredRegions = new List<string>
                    {
                        "West US 2",    // Example region; replace or add regions as needed
                        "Central US"
                    },
            };

            // Initialize Cosmos DB client and container reference
            var client = new CosmosClient(endpoint, key, clientOptions);
            var container = client.GetContainer(databaseId, containerId);

            // Set up cancellation support for graceful shutdown (Ctrl+C)
            using var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("\nCtrl+C detected. Exiting...");
                e.Cancel = true;
                cts.Cancel();
            };

            Console.WriteLine("Writing to Cosmos DB every second. Press Ctrl+C to exit.");

            // Main loop: write a new item to Cosmos DB every second until cancelled
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    // Create a new item with a unique id and a sample message
                    var item = new { id = Guid.NewGuid().ToString(), message = "Hello Cosmos!" };
                    var response = await container.CreateItemAsync(item, new PartitionKey(item.id));

                    // Log the status and regions contacted for diagnostics
                    var timestamp = DateTime.UtcNow.ToString("o");
                    var rawStatus = (int)response.StatusCode;
                    var statusCode = (rawStatus == 201) ? 200 : rawStatus;

                    var regions = string.Join(", ",
                        response.Diagnostics.GetContactedRegions().Select(r => r.regionName));

                    Console.WriteLine($"[{timestamp}] Status: {statusCode} | RegionsContacted: {regions}");
                }
                catch (Exception ex)
                {
                    // Log any errors encountered during item creation
                    Console.WriteLine($"[{DateTime.UtcNow:o}] ERROR: {ex.Message}");
                }

                try
                {
                    // Wait for 1 second before next write, support cancellation
                    await Task.Delay(1000, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"Exited!!!");
                }
            }
        }
    }
}
