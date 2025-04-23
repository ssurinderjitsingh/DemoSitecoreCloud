// See https://aka.ms/new-console-template for more information
using DemoSitecoreCloud;
using DemoSitecoreCloud.Models;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var countries = new List<Country>
{
    new Country
    {
        DisplayName = "Canada",
        Name = "Canada",
        CountryCode = "CA"
    },
    new Country
    {
        DisplayName = "United Kingdom",
        Name = "United Kingdom",
        CountryCode = "UK"
    }
};




foreach (var country in countries)
{
    QueryBuilder.CreateGraphQLQuery(country.Name, nodes, templateId, parentId);
}

Console.WriteLine(countries);

//var countries = new List<Country>().Add(new Country
//{
//    DisplayName = "Canada",
//    Name = "Canada",
//    CountryCode = "CA"
//});