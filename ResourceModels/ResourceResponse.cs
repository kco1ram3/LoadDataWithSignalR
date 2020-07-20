using System.Collections.Generic;

namespace ResourceModels
{
    public class ResourceResponse
    {
        public string Token { get; set; }
        public List<Data> Datas { get; set; }
        public bool IsCompleted { get; set; }
    }
}
