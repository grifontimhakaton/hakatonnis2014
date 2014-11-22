using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Text;
using ModernHttpClient;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Android.Helpers
{
    public static class MethodHelper
    {
        public static TResultType Post<TResultType, TRequestType>(TRequestType value, string methodName, string apiUrl, string controller)
        {
            //var httpClient = new HttpClient
            //{
            //    BaseAddress = new Uri(apiUrl),
            //    Timeout = new TimeSpan(0, 10, 0)
            //};

            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var action = string.Format("{0}/{1}", controller, methodName);

            //var jsonFormatter = new JsonMediaTypeFormatter();
            //HttpContent content = new ObjectContent<TRequestType>(value, jsonFormatter);

            //var response = httpClient.PostAsync(action, content).Result;
            //response.EnsureSuccessStatusCode();
            //return response.Content.ReadAsAsync<TResultType>().Result;
            var client = new HttpClient(new NativeMessageHandler());

            //var contentTest = new StringContent(JsonConvert.SerializeObject(frameData), Encoding.UTF8, "application/json");
            //try
            //{
            //    HttpResponseMessage response = client.PostAsync(string.Format("{0}api/ImageProcessing/ProcessFrame?width={1}&height={2}", margoBaseUrl, width, height), contentTest).Result;
            //    response.EnsureSuccessStatusCode();

            //    HttpContent content = response.Content;
            //    var result = content.ReadAsByteArrayAsync().Result;
            //}
            //catch (Exception ex)
            //{
            //    var errorResult = ex.Message;
            //}
            //return result;
        }
    }
}