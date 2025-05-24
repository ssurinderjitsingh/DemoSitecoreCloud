using DemoSitecoreCloud.Interface;
using DemoSitecoreCloud.Models;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Types.Relay.DataObjects;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DemoSitecoreCloud
{
    public class GraphQLExecutor : IGraphQLExecutor
    {
        private readonly IConnection _connection;
        private readonly GraphQLHttpClient _graphQLClient;

        public GraphQLExecutor(IConnection connection)
        {
            _connection = connection;
            _graphQLClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_connection.EndPoint)
            }, new NewtonsoftJsonSerializer());
        }

        /// <summary>
        /// Executes mutation query to create item in sitecore
        /// </summary>
        /// <param name="graphQLQuery"></param>
        /// <returns></returns>
        public async Task<string> ExecuteMutationQuery(GraphQLRequest graphQLQuery)
        {
            var token = await _connection.GetAccessTokenAsync();
            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dynamicObject = await _graphQLClient.SendMutationAsync<dynamic>(graphQLQuery);
            if (dynamicObject.Data != null)
            {
                var data = dynamicObject.Data;
                if (data != null)
                {
                    var createItem = data.createItem;
                }
            }
            return "Item Created";
        }

        /// <summary>
        /// Executes media ql query to media item in sitecore
        /// </summary>
        /// <param name="graphQLQuery"></param>
        /// <returns></returns>
        public async Task<string> ExecuteMediaGraphQLQuery(GraphQLRequest graphQLQuery)
        {
            var preSignedUploadUrl = string.Empty;

            var token = await _connection.GetAccessTokenAsync();
            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dynamicObject = await _graphQLClient.SendMutationAsync<dynamic>(graphQLQuery);
            if (dynamicObject.Data != null)
            {
                var data = dynamicObject.Data;
                if (data != null)
                {
                    preSignedUploadUrl = data.uploadMedia;
                }
            }
            return preSignedUploadUrl;
        }

        /// <summary>
        /// Executes graph ql query to create item in sitecore
        /// </summary>
        /// <param name="graphQLQuery"></param>
        /// <returns></returns>
        public async Task<Root> ExecuteQueryAsync(GraphQLRequest graphQLQuery)
        {
            var root = new Root();
            var token = await _connection.GetAccessTokenAsync();
            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _graphQLClient.SendQueryAsync<dynamic>(graphQLQuery);
            if (response.Data?.item != null)
            {
                var children = response.Data.item;
                var edges = children.children?.edges;
                var pageInfoData = children.children?.pageInfo;

                if (edges != null)
                {
                    foreach (var edge in edges)
                    {
                        var node = edge.node;
                        var sitecoreNode = new SitecoreNode
                        {
                            Name = node.name,
                            DisplayName = node.displayName,
                            ParentId = node.id,
                            Path = node.path,
                            Fields = new List<SitecoreField>()
                        };

                        if (node?.fields?.edges != null)
                        {
                            foreach (var fieldEdge in node.fields.edges)
                            {
                                var fieldNode = fieldEdge.node;
                                sitecoreNode.Fields.Add(new SitecoreField
                                {
                                    Name = fieldNode.name,
                                    Value = fieldNode.value
                                });
                            }
                        }
                        root.Nodes.Add(sitecoreNode);
                    }
                }

                if (pageInfoData != null)
                {
                    root.PageInfo = new PageInfo
                    {
                        EndCursor = pageInfoData.endCursor,
                        HasNextPage = pageInfoData.hasNextPage
                    };
                }
            }
            else
            {
                root.Errors = response.Errors?.Select(e => e.Message).ToList() ?? new List<string>();
            }

            return root;
        }
    }
}
