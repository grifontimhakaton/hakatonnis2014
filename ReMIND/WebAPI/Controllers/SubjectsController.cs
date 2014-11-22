﻿using System;
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
    public class SubjectsController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        // GET api/Subjects
        public List<Subject> GetSubjects()
        {
            return db.Subjects.ToList();
        }

        // GET api/Subjects
        public List<Subject> GetUserSubjects(int userID)
        {
            //db.Users.Where(x => x.ID == userID).Select(y => y.Subjects).ToList();
            //return db.Subjects.ToList();
            var test = db.Subjects.Select(x => new SubjectDTO() { SubjectID = x.ID, SubjectName = x.SubjectTitle, UserSelected = x.Users.Where(y => y.ID == x.ID).Count() == 1 });


            return null;
        }

        // GET api/Subjects/5
        [ResponseType(typeof(Subject))]
        public IHttpActionResult GetSubject(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // PUT api/Subjects/5
        public IHttpActionResult PutSubject(int id, Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.ID)
            {
                return BadRequest();
            }

            db.Entry(subject).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
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

        // POST api/Subjects
        [ResponseType(typeof(Subject))]
        public IHttpActionResult PostSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subjects.Add(subject);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subject.ID }, subject);
        }

        // DELETE api/Subjects/5
        [ResponseType(typeof(Subject))]
        public IHttpActionResult DeleteSubject(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return NotFound();
            }

            db.Subjects.Remove(subject);
            db.SaveChanges();

            return Ok(subject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectExists(int id)
        {
            return db.Subjects.Count(e => e.ID == id) > 0;
        }
    }
}