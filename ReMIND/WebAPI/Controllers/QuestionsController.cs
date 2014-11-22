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
using SharedPCL.Enums;

namespace WebAPI.Controllers
{
    public class QuestionsController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        // GET api/GetQuestionsDTO
        public List<QuestionDto> GetQuestionsDTO()
        {
//          return db.Questions.Select(x => new QuestionDto() { Id = x.ID, QuestionType = QuestionType.ABC, Text = x.QuestionText}).ToList();
            var result = new List<QuestionDto>();
            result.Add(new QuestionDto() { Id = 20, QuestionType = QuestionType.ABC, Text = "Sta koj kurac ?" });
            return result;
        }


        // GET api/Questions
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        // GET api/Questions/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // PUT api/Questions/5
        public IHttpActionResult PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.ID)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Questions
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = question.ID }, question);
        }

        // DELETE api/Questions/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
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