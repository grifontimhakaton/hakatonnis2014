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
    public class SubjectsController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        // GET api/Subjects
        public List<SubjectDTO> GetSubjects()
        {
            return db.Subjects.Select(x => new SubjectDTO() { SubjectID = x.ID, SubjectName = x.SubjectTitle }).ToList();            
        }

        // GET api/Subjects
        public List<SubjectDTO> GetUserSubjects(int userID)
        {
            return db.Subjects.Select(x => new SubjectDTO() { SubjectID = x.ID, SubjectName = x.SubjectTitle, UserSelected = x.Users.Where(y => y.ID == userID).Count() > 0 }).ToList();            
        }

        [HttpGet]
        [ResponseType(typeof(bool))]
        public IHttpActionResult SelectSubject(int subjectID,int userID)
        {
            Subject subject = db.Subjects.Find(subjectID);
            User user = db.Users.Include(x => x.Subjects).Where(y => y.ID == userID).FirstOrDefault();
            if (subject == null || user == null)
            {
                return Ok(false);
            }

            if (user.Subjects == null)
            {
                user.Subjects = new List<Subject>();
            }

            user.Subjects.Add(subject);
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

        private bool SubjectExists(int id)
        {
            return db.Subjects.Count(e => e.ID == id) > 0;
        }
    }
}