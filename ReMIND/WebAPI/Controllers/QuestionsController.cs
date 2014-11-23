using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;
using WebAPI.DAL;
using SharedPCL.DataContracts;

namespace WebAPI.Controllers
{
    public class QuestionsController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        // GET api/GetQuestionsDTO
        public List<QuestionDTO> GetUserQuestions(int userID, int subjectID)
        {
            var result = db.Questions.Where(x => x.Subject.ID == subjectID).Select(y => new QuestionDTO()
            {
                Id = y.ID,
                QuestionExplanation = y.QuestionExplanation,
                QuestionText = y.QuestionText,
                QuestionTitle = y.QuestionTitle,
                QuestionAnswers = y.QuestionAnswers.Select(z => new QuestionAnswerDTO() { QuestionAnswerText = z.AnswerText,
                    Id = z.ID,
                    Correct = z.AnswerValue == 1
                }).ToList()
            }).ToList();

            return result;
        }

        [HttpGet]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AnswerQuestion(int answerID,int userID)
        {
            QuestionAnswer questiona = db.QuestionAnswers.Find(answerID);
            User user = db.Users.Include(x => x.QuestionAnswers).Where(y => y.ID == userID).FirstOrDefault();
            if (questiona == null || user == null)
            {
                return Ok(false);
            }

            if (user.QuestionAnswers == null)
            {
                user.QuestionAnswers = new List<QuestionAnswer>();
            }

            user.QuestionAnswers.Add(questiona);
            db.SaveChanges();

            return Ok(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.ID == id) > 0;
        }
    }
}