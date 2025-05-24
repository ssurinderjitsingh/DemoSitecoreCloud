using DemoSitecoreCloud.Models;
using GraphQL;

namespace DemoSitecoreCloud.Interface
{
    public interface IGraphQLExecutor
    {
        Task<string> ExecuteCreateQuery(GraphQLRequest graphQLQuery);

        Task<string> ExecuteMediaGraphQLQuery(GraphQLRequest graphQLQuery);

        Task<Root> ExecuteMutationQuery(GraphQLRequest graphQLQuery);
    }
}
