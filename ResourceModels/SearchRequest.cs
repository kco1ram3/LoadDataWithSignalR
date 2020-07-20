
namespace ResourceModels
{
    public class SearchRequest
    {
        public string ConnectionId { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
        public SearchFilter SearchFilter { get; set; }
    }
}