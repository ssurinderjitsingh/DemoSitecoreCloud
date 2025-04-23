using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace DemoSitecoreCloud
{
    public class QueryExecuter
    {
        public QueryExecuter()
        {

        }
        public async Task<string> ExecuteCreateQuery(GraphQLRequest graphQLQuery)
        {
            var graphQLClient = new GraphQLHttpClient("BaseUrl", new NewtonsoftJsonSerializer());

            var connection = new SitecoreXmConnection(new HttpClient());
            var authToken = await connection.GetAuthTokenAsync();

            if (authToken != null)
            {
                graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

            }

            var response = await graphQLClient.SendQueryAsync<dynamic>(graphQLQuery);
            if (response.Data != null)
            {
                var children = response.Data.item;
                if (children != null)
                {
                    var edges = children.children.edges;
                    var pageInfo = children.children.pageInfo;
                }
            }
            return string.Empty;
        }
    }
}
