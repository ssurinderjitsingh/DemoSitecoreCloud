using DemoSitecoreCloud.Models;
using GraphQL;

namespace DemoSitecoreCloud.Interface
{
    public interface IGraphQLExecutor
    {
        Task<Root> ExecuteQueryAsync(GraphQLRequest graphQLQuery);

        Task<string> ExecuteMediaGraphQLQuery(GraphQLRequest graphQLQuery);

        Task<string> ExecuteMutationQuery(GraphQLRequest graphQLQuery);
    }
}
    