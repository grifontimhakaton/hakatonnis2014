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
    public class QuestionAreaController : Controller
    {
        private ReMinderContext db = new ReMinderContext();

        // GET: /QuestionArea/
        public ActionResult Index()
        {
            return View(db.QuestionAreas.ToList());
        }

        // GET: /QuestionArea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionArea questionarea = db.QuestionAreas.Find(id);
            if (questionarea == null)
            {
                return HttpNotFound();
            }
            return View(questionarea);
        }

        // GET: /QuestionArea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /QuestionArea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,QuestionAreaTitle,QuestionAreaDescription")] QuestionArea questionarea)
        {
            if (ModelState.IsValid)
            {
                db.QuestionAreas.Add(questionarea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionarea);
        }

        // GET: /QuestionArea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionArea questionarea = db.QuestionAreas.Find(id);
            if (questionarea == null)
            {
                return HttpNotFound();
            }
            return View(questionarea);
        }

        // POST: /QuestionArea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,QuestionAreaTitle,QuestionAreaDescription")] QuestionArea questionarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionarea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionarea);
        }

        // GET: /QuestionArea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionArea questionarea = db.QuestionAreas.Find(id);
            if (questionarea == null)
            {
                return HttpNotFound();
            }
            return View(questionarea);
        }

        // POST: /QuestionArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionArea questionarea = db.QuestionAreas.Find(id);
            db.QuestionAreas.Remove(questionarea);
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
