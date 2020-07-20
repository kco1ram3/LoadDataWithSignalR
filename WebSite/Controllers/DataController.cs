using ResourceModels;
using RestSharp;
using System;
using System.Configuration;
using System.Web.Http;
using WebSite.Hubs;
using System.Runtime.Caching;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebSite.Controllers
{
    public class DataController : ApiControllerWithHub<DataHub>
    {
        [HttpPost]
        public SearchResponse Search(SearchRequest searchRequest)
        {
            string cacheKey = searchRequest.SearchCriteria.CacheKey();
            MemoryCache memoryCache = MemoryCache.Default;
            var cache = memoryCache.Get(cacheKey);
            if (cache == null)
            {
                ResourceRequest resourceRequest = new ResourceRequest()
                {
                    Token = cacheKey,
                    CallbackApi = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/Data/Response"
                };
                ResourceResponse resourceResponse = new ResourceResponse()
                {
                    Token = cacheKey,
                    Datas = new List<Data>(),
                    IsCompleted = false
                };
                memoryCache.Add(cacheKey, resourceResponse, DateTimeOffset.UtcNow.AddHours(1));
                Hub.Groups.Add(searchRequest.ConnectionId, cacheKey);

                
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Content-type", "application/json");
                request.Parameters.Clear();
                request.AddParameter("application/json", request.JsonSerializer.Serialize(resourceRequest), ParameterType.RequestBody);

                var client = new RestClient($"{ConfigurationManager.AppSettings["DataResourceApiUrl"]}/Data/Search");
                //client.Execute(request);
                client.ExecuteAsync(request, null);
            }
            else
            {
                ResourceResponse resourceResponse = (ResourceResponse)cache;
                if (!resourceResponse.IsCompleted)
                {
                    Hub.Groups.Add(searchRequest.ConnectionId, cacheKey);
                }
                return ClientResponse(resourceResponse, searchRequest.SearchFilter);
            }
            return null;
        }

        [HttpPost]
        public void Response(ResourceResponse resourceResponse)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            var cache = (ResourceResponse)memoryCache.Get(resourceResponse.Token);
            cache.Datas.AddRange(resourceResponse.Datas);
            cache.IsCompleted = resourceResponse.IsCompleted;
            memoryCache[resourceResponse.Token] = cache;
            Hub.Clients.Group(resourceResponse.Token).update(ClientResponse(cache));
        }

        private SearchResponse ClientResponse(ResourceResponse resourceResponse, SearchFilter searchFilter = null)
        {
            SearchResponse searchResponse = new SearchResponse()
            {
                Token = resourceResponse.Token,
                IsCompleted = resourceResponse.IsCompleted, 
                TotalItems = resourceResponse.Datas.Count
            };

            int pageNo = searchFilter?.PageNo ?? 1;
            int pageSize = searchFilter?.PageSize ?? 10;
            searchResponse.Datas = resourceResponse.Datas;
            searchResponse.Datas.Sort((x, y) => x.Price.CompareTo(y.Price));
            if (resourceResponse.Datas.Count > pageSize)
            {
                int start = (pageNo * pageSize) - pageSize;
                if (start + pageSize > resourceResponse.Datas.Count)
                {
                    searchResponse.Datas = searchResponse.Datas.GetRange(start, resourceResponse.Datas.Count % pageSize);
                }
                else
                {
                    searchResponse.Datas = searchResponse.Datas.GetRange(start, pageSize);
                }
            }
            
            return searchResponse;
        }
    }
}