using ModernHttpClient;
using Newtonsoft.Json;
using SharedPCL.DataContracts;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Android.Helpers
{
    public static class MethodHelper
    {
        private static string baseApiUrl = "http://hakaton.azurewebsites.net/";
        public static List<QuestionDto> GetQuestions(int userId)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                //HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Questions/GetQuestionsDTO?userId={1}", baseApiUrl, userId)).Result;
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Questions/GetQuestionsDTO", baseApiUrl)).Result;
              
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

        public static List<SubjectDTO> GetUserSubjects(int userId)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Subjects/GetUserSubjects?userID={0}", baseApiUrl, userId)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var questions = JsonConvert.DeserializeObject<List<SubjectDTO>>(result);

                return questions;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }

        public static UserDTO LoginOrRegister(string username, string password)
        {
            return LoginOrRegister(username, string.Empty, password);
        }

        public static UserDTO LoginOrRegister(string username, string email, string password)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response;

                if (string.IsNullOrWhiteSpace(email))
                {
                    response = client.GetAsync(string.Format("{0}api/Users/LoginUser?email={1}&passwordMD5={2}", baseApiUrl, username, password)).Result;
                }
                else
                {
                    response = client.GetAsync(string.Format("{0}api/Users/CreateUser?email={1}&passwordMD5={2}&fullName={3}", baseApiUrl, username, email, password)).Result;
                }

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var loginOrRegisterResponse = JsonConvert.DeserializeObject<UserDTO>(result);

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