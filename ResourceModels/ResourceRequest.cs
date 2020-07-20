using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceModels
{
    public class ResourceRequest
    {
        public string Token { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
        public string CallbackApi { get; set; }
    }
}
