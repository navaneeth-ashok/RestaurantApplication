using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using RestaurantApplication.Models;

namespace RestaurantApplication.Models
{
    public class Client
    {
        public HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly string appUrl = "https://localhost:44329/";
        private readonly string mainUrl = appUrl + "api/";
        private string accessToken = "";

        public Client()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(mainUrl);
        }

        public HttpContent ExecuteGet(string url)
        {
            // Generate access Token if it's not present.
            // Enhancement, store access_token expiry and refresh it only if required.
            if(accessToken == "")
            {
                // username and password of the api_user
                // requirement -> create a user with the following email ID or 
                // paste the userid/password of a user you created
                accessToken = GetToken("api_user@api.com", "iWxh%eUh5!b_mC2");
            }
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpContent responseContent = client.GetAsync(url).Result.Content;
            return responseContent;
        }

        public HttpResponseMessage ExecutePost(string url, string jsonPayLoad, string mediaType = "application/json")
        {
            // Generate access Token if it's not present.
            // Enhancement, store access_token expiry and refresh it only if required.
            if (accessToken == "")
            {
                // username and password of the api_user
                // requirement -> create a user with the following email ID or 
                // paste the userid/password of a user you created
                accessToken = GetToken("api_user@api.com", "iWxh%eUh5!b_mC2");
            }

            // Serialize the payload
            //string jsonPayLoad = jss.Serialize(payLoad);

            HttpContent content = null;

            // Receive the serialised payload and add appropriate content media type
            if (jsonPayLoad != null)
            {
                content = new StringContent(jsonPayLoad);
                content.Headers.ContentType.MediaType = mediaType;
            }
            

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            return response;
        }

        private string GetToken(string uname, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", uname),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")
            });
            HttpContent respContent = client.PostAsync(appUrl + "Token", content).Result.Content;
            System.Diagnostics.Debug.WriteLine(respContent);

            Token newToken = respContent.ReadAsAsync<Token>().Result;
            //System.Diagnostics.Debug.WriteLine(newToken);
            //System.Diagnostics.Debug.WriteLine(newToken.access_token);

            return newToken.access_token;
            //client.PostAsync(appUrl + "Token").Result.Content;
        }
        
    }
}