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
    public class EventsController : ApiController
    {
        private ReMinderContext db = new ReMinderContext();

        // GET api/Events
        public List<EventDTO> GetEvents()
        {
            return db.Events.Include(x => x.Subject).Select(z => new EventDTO(){                
                EventDescription = z.EventDescription,
                EventEndtUTC = z.EventEndtUTC,
                EventStartUTC = z.EventEndtUTC,
                EventText = z.EventText,
                EventTitle = z.EventTitle,
                Subject = z.Subject.SubjectTitle,
                ID = z.ID
            }).ToList();
        }

        public List<EventDTO> GetEventsFromSubject(int subjectID)
        {
            return db.Events.Include(x => x.Subject).Where(y => y.Subject.ID == subjectID).Select(z => new EventDTO()
            {
                EventDescription = z.EventDescription,
                EventEndtUTC = z.EventEndtUTC,
                EventStartUTC = z.EventEndtUTC,
                EventText = z.EventText,
                EventTitle = z.EventTitle,
                Subject = z.Subject.SubjectTitle,
                ID = z.ID
            }).ToList();
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.ID == id) > 0;
        }
    }
}