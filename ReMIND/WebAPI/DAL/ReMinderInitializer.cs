using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class ReMinderInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ReMinderContext>
    {
        protected override void Seed(ReMinderContext context)
        {
            //FYI: DROP DB
            //sqllocaldb.exe stop v11.0
            //sqllocaldb.exe delete v11.0
            //sqllocaldb.exe start v11.0

            Event eve = new Event();
            eve.EventDescription = "TestEvent";
            eve.EventText = "Teve Text";
            eve.EventTitle = "Teve Title";
            eve.EventStartUTC = DateTime.UtcNow;
            eve.EventEndtUTC = DateTime.UtcNow + TimeSpan.FromHours(24);
            


            Subject s = new Subject();
            s.SubjectTitle = "Fizika";
            s.SubjectDescription = "Lorem Opsta Fizika Ipsum";

            eve.Subject = s;

            context.Events.Add(eve);
            context.Subjects.Add(s);
            context.SaveChanges();

            User testUser = new User();
            testUser.Email = "test@email.com";
            testUser.FullName = "Mr. Test User";
            testUser.PasswordHash = Helpers.MD5Helper.GetMd5Hash("123456aaa");

            testUser.Subjects = new List<Subject>();
            testUser.Subjects.Add(s);

            context.Users.Add(testUser);
            context.SaveChanges(); 

            var stream = System.IO.File.Open(System.Web.HttpContext.Current.Server.MapPath(@"~/TestData/pitanja_3.xlsx"), FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            var ds = excelReader.AsDataSet();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    //null records
                    if (string.IsNullOrWhiteSpace(row[0].ToString())) break;

                    Question q = new Question();
                    q.QuestionText = row[0].ToString();
                    q.QuestionExplanation = row[5].ToString();
                    q.Subject = s;

                    context.Questions.Add(q);
                    context.SaveChanges();

                    for (int i = 1; i < 5; i++)
                    {
                        QuestionAnswer qa = new QuestionAnswer();
                        qa.QuestionID = q.ID;
                        qa.AnswerText = row[i].ToString();
                        qa.AnswerValue = Convert.ToInt32(q.QuestionExplanation == qa.AnswerText);
                        qa.Question = q;
                        context.QuestionAnswers.Add(qa);
                    }

                    context.SaveChanges();
                }
                catch (Exception)
                {
                    //null error exception
                    throw;
                }

            }
            excelReader.Close();
            context.SaveChanges(); 
        }
    }
}