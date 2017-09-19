using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SignSystemAPI.Client
{
    public static class ApiClient
    {
        public static HttpClient GetClient()
        {
            HttpClient client= new HttpClient();
            client.BaseAddress=new Uri(Constants.Constants.SignSystemUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
