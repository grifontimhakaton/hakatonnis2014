using ModernHttpClient;
using Newtonsoft.Json;
using SharedPCL.DataContracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace ReMinder.Helpers
{
    public static class MethodHelper
    {
        private static string baseApiUrl = "http://hakaton.azurewebsites.net/";
        public static List<QuestionDTO> GetQuestions(int userId, int subjectId)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Questions/GetUserQuestions?userID={1}&subjectID={2}", baseApiUrl, userId, subjectId)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;
                
                var result = content.ReadAsStringAsync().Result;
                var questions = JsonConvert.DeserializeObject<List<QuestionDTO>>(result);
                
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
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Subjects/GetUserSubjects?userID={1}", baseApiUrl, userId)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var subjects = JsonConvert.DeserializeObject<List<SubjectDTO>>(result);

                return subjects;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }

        public static bool AnswerQuestion(int answerID, int userID)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Questions/AnswerQuestion?userID={1}&answerID={2}", baseApiUrl, userID, answerID)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var sucess = JsonConvert.DeserializeObject<bool>(result);

                return sucess;
            }
            catch (Exception ex)
            {
                return false;
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

        public static List<EventDTO> GetEvents()
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Events/GetEvents", baseApiUrl)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var events = JsonConvert.DeserializeObject<List<EventDTO>>(result);

                return events;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }

        public static List<EventDTO> GetEvents(int subjectID)
        {
            try
            {
                var client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage response = client.GetAsync(string.Format("{0}api/Events/GetEventsFromSubject?subjectID={0}", baseApiUrl, subjectID)).Result;

                response.EnsureSuccessStatusCode();
                HttpContent content = response.Content;

                var result = content.ReadAsStringAsync().Result;
                var events = JsonConvert.DeserializeObject<List<EventDTO>>(result);

                return events;
            }
            catch (Exception ex)
            {
                var errorResult = ex.Message;
                return null;
            }
        }
    }
}