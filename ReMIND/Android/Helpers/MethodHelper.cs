using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using ModernHttpClient;
using SharedPCL.DataContracts;

namespace Android.Helpers
{
    public static class MethodHelper
    {
        private static string baseApiUrl = "http://192.168.0.102:8081/";
        public static List<QuestionDto> GetQuestions(int userId)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Question/GetQuestions?userId={1}", baseApiUrl, userId)).Result;
              
                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;
                
                var result = content.ReadAsStringAsync().Result;
                var questions = JsonConvert.DeserializeObject<List<QuestionDto>>(result);
                
                return questions;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }

        public static LoginOrRegisterResponseDto LoginOrRegister(string username, string password)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/User/LoginOrRegister?username={1}&password={2}", baseApiUrl, username, password)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var loginOrRegisterResponse = JsonConvert.DeserializeObject<LoginOrRegisterResponseDto>(result);

                return loginOrRegisterResponse;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }
    }
}