using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;


namespace DemoSitecoreCloud
{
    public class QueryBuilder
    {
        public QueryBuilder() { }

        public static GraphQLRequest CreateGraphQLQuery(string itemName, Dictionary<string, string> nodes, string templateId, string parentId)
        {

            var fieldsBuilder = new StringBuilder();
            foreach (var node in nodes)
            {
                fieldsBuilder.Append($"{{name: \"{ node.Key}\", value: \"{node.Value}\" }}" );
            }

            var query = $@"
                        mutation {{
                            createItem(
                                     input: {{ 
                                             name: ""{itemName}""
                                             templateId: ""{templateId}""
                                             parent: ""{parentId}""
                                             language: ""en""
                                              fields[
                                                 {fieldsBuilder.ToString().Trim()}
                                            ]
                                         }}
                                     ){{
                                        item {{
                                            itemId
                                            name
                                            path    
                                            fields(ownFields:true, excludeStandardfields: true){{
                                                  nodes {{ 
                                                           name 
                                                           value
                                                       }}
                                                 }}
                                            }}
                                        }}
                                 }}";
            return new GraphQLRequest
            {
                Query = query,
            };
        }

        public static GraphQLRequest GetItemGraphQLQuery(string itemId)
        {
            var query = $@"
                        query {{
                            item(where: {{ itemId: ""{itemId}"" }}) {{
                                        item {{
                                            itemId
                                            name
                                            path    
                                            fields(ownFields:true, excludeStandardfields: true){{
                                                  nodes {{ 
                                                           name 
                                                           value
                                                       }}
                                                 }}
                                            }}
                                        }}
                                 }}";
            return new GraphQLRequest
            {
                Query = query,
            };
        }

    }
}
