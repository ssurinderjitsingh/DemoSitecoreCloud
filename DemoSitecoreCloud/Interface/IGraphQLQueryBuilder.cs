
using GraphQL;

namespace DemoSitecoreCloud.Interface
{
    public interface IGraphQLQueryBuilder
    {
        GraphQLRequest GetItemGraphQLQuery(string itemId);

        GraphQLRequest GetPaginatedGraphQLQuery(string itemId, int first, string afterCursor);

        GraphQLRequest CreateGraphQLQuery(string itemName, Dictionary<string, string> nodes, string templateId, string parentId);

        GraphQLRequest UpdateItemGraphQLQuery(string itemId, Dictionary<string, string> fields, string templateId, string parentId);

        GraphQLRequest DeleteItemGraphQLQuery(string itemId);

        GraphQLRequest GetMediaPresignedUrlGraphQLuery(string itemPath);
    }
}
