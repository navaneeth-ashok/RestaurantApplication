using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace RestaurantApplication.Models
{
    public class Client
    {
        private readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private string mainUrl = "https://localhost:44329/api/";

        public Client()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/");
        }

        public HttpContent Execute(string url)
        {
            HttpContent responseContent = client.GetAsync(url).Result.Content;
            return responseContent;
        }
        
    }
}