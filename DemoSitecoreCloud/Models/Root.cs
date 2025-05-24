using GraphQL.Types.Relay.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSitecoreCloud.Models
{
    public class Root
    {
        public PageInfo PageInfo { get; set; }

        public List<string> Errors { get; set; }

        public List<SitecoreNode> Nodes { get; set; }
    }
}
