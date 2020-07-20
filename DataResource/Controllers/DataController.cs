using ResourceModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;

namespace DataResource.Controllers
{
    public class DataController : ApiController
    {
        [HttpPost]
        public void Search(ResourceRequest resourceRequest)
        {
            Random rnd = new Random();

            int supplier = 6; //rnd.Next(1, 6);
            for (int index = 1; index <= supplier; index++)
            {
                int items = rnd.Next(1, 100);
                ResourceResponse response = new ResourceResponse()
                {
                    Token = resourceRequest.Token,
                    Datas = new List<Data>(),
                    IsCompleted = index == supplier
                };
                for (int item = 1; item <= items; item++)
                {
                    response.Datas.Add(new Data() {
                        Key = Guid.NewGuid().ToString(),
                        Price = float.Parse(rnd.Next(100, 9999).ToString()) + (float.Parse(rnd.Next(100, 9999).ToString()) / float.Parse(rnd.Next(100, 9999).ToString()))
                    });
                }

                int delay = rnd.Next(3000, 5000);
                Thread.Sleep(delay);

                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Content-type", "application/json");
                request.Parameters.Clear();
                request.AddParameter("application/json", request.JsonSerializer.Serialize(response), ParameterType.RequestBody);

                var client = new RestClient(resourceRequest.CallbackApi);
                //client.Execute(request);
                client.ExecuteAsync(request, null);
            }
        }
    }
}