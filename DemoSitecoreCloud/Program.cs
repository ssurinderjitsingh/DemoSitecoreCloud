using DemoSitecoreCloud;
using DemoSitecoreCloud.Interface;
using DemoSitecoreCloud.Models;
using GraphQL.Types.Relay.DataObjects;
using System.Collections.Generic;

public class Program
{
    private static IConnection connection;
    public static async Task Main(string[] args)
    {
        connection = new SitecoreConnection(
            endpoint: "https://hostname-sitecorecloud.io",
            clientId: "client-id",
            clientSecret: "client-secret",
            tokenUrl: "https://identity-url/connect/token",
            audience: "https://your-audience"
        );

        var builder = new GraphQLQueryBuilder();
        var executor = new GraphQLExecutor(connection);
        var uploader = new MediaUploader(builder, executor);

        var countries = new List<Country>
            {
                new Country { DisplayName = "Canada", Name = "Canada", CountryCode = "CA" },
                new Country { DisplayName = "United Kingdom", Name = "United Kingdom", CountryCode = "UK" }
            };

        Console.WriteLine("Countries loaded:");
        foreach (var country in countries)
        {
            Console.WriteLine($"{country.DisplayName} ({country.CountryCode})");
        }

        // Example Upload usage
       // string result = await uploader.UploadAsync("/Project/Media/Banner", "C:\\temp\\banner.jpg");
        //Console.WriteLine(result);
    }

    public async Task<string> CreateSitecoreItem(string sitecoreId)
    {
        var builder = new GraphQLQueryBuilder();
        var executor = new GraphQLExecutor(connection);

        var fields = new Dictionary<string, string>();
        var itemName = string.Empty;
        var templateId = "template-guid";
        var parentId = "parent-guid";

        var graphQlQuery = builder.CreateGraphQLQuery(itemName, fields, templateId, parentId);
        var result = await executor.ExecuteMutationQuery(graphQlQuery, true);

        return result;
    }

    public async Task<string> UpdateSitecoreItem(string sitecoreId)
    {
        var builder = new GraphQLQueryBuilder();
        var executor = new GraphQLExecutor(connection);

        var fields = new Dictionary<string, string>();
        var itemName = string.Empty;
        var templateId = "template-guid";
        var parentId = "parent-guid";

        var graphQlQuery = builder.UpdateItemGraphQLQuery(itemName, fields, templateId, parentId);
        var result = await executor.ExecuteMutationQuery(graphQlQuery, false);

        return result;
    }

    public async Task<Root> GetItemsAsync(string itemId)
    {
        var builder = new GraphQLQueryBuilder();
        var executor = new GraphQLExecutor(connection);

        var graphQlQuery = builder.GetItemGraphQLQuery(itemId);
        var result = await executor.ExecuteQueryAsync(graphQlQuery);

        return result;

    }

    public async Task<List<SitecoreNode>> GetPaginatedItemsAsync(string parentId)
    {
        var builder = new GraphQLQueryBuilder();
        var executor = new GraphQLExecutor(connection);

        var nodes = new List<SitecoreNode>();
        var first = 100;
        string? afterCursor = null;
        bool hasNextPage = false;

        do
        {
            var graphQlQuery = builder.GetPaginatedGraphQLQuery(parentId, first, afterCursor);
            var result = await executor.ExecuteQueryAsync(graphQlQuery);

            if (result != null)
            {
                nodes.AddRange(result.Nodes);

                hasNextPage = result.PageInfo?.HasNextPage ?? false;
                afterCursor = result.PageInfo?.EndCursor;
            }
            else
            {
                hasNextPage = false; // stop on failure
            }

        } while (hasNextPage);

        return nodes;
    }
}


