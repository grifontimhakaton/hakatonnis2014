using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;
using WebAPI.DAL;

namespace WebAPI.Controllers
{
    [Authorize]
    public class QuestionAnswerController : Controller
    {
        private ReMinderContext db = new ReMinderContext();

        // GET: /QuestionAnswer/
        public ActionResult Index()
        {
            var questionanswers = db.QuestionAnswers.Include(q => q.Question);
            return View(questionanswers.ToList());
        }

        // GET: /QuestionAnswer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            return View(questionanswer);
        }

        // GET: /QuestionAnswer/Create
        public ActionResult Create()
        {
            ViewBag.QuestionID = new SelectList(db.Questions, "ID", "QuestionText");
            return View();
        }

        // POST: /QuestionAnswer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,QuestionID,AnswerText,AnswerValue")] QuestionAnswer questionanswer)
        {
            if (ModelState.IsValid)
            {
                db.QuestionAnswers.Add(questionanswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionID = new SelectList(db.Questions, "ID", "QuestionText", questionanswer.QuestionID);
            return View(questionanswer);
        }

        // GET: /QuestionAnswer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionID = new SelectList(db.Questions, "ID", "QuestionText", questionanswer.QuestionID);
            return View(questionanswer);
        }

        // POST: /QuestionAnswer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,QuestionID,AnswerText,AnswerValue")] QuestionAnswer questionanswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionanswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionID = new SelectList(db.Questions, "ID", "QuestionText", questionanswer.QuestionID);
            return View(questionanswer);
        }

        // GET: /QuestionAnswer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            return View(questionanswer);
        }

        // POST: /QuestionAnswer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            db.QuestionAnswers.Remove(questionanswer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
