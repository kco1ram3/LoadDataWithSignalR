
namespace ResourceModels
{
    public class SearchCriteria
    {
        public string Condition { get; set; }
        public string CacheKey()
        {
            return string.Concat(Condition.ToLower());
        }
    }
}