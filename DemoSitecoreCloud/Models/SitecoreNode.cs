using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSitecoreCloud.Models
{
    public class SitecoreNode
    {
        public int Id { get; set; }

        public string Name { get; set; }    

        public string Path { get; set; }

        public string DisplayName { get; set; }

        public string ParentId { get; set; }

        public List<SitecoreField> Fields { get; set; }
        
    }
}
