using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Julekalender.Models;

namespace Julekalender.Controllers
{
    public class DrawingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Drawing/
        public ActionResult Index()
        {
            return View(db.Drawings.ToList());
        }

        // GET: /Drawing/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drawing drawing = db.Drawings.Find(id);
            if (drawing == null)
            {
                return HttpNotFound();
            }
            return View(drawing);
        }

        // GET: /Drawing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Drawing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Time")] Drawing drawing)
        {
            if (ModelState.IsValid)
            {
                db.Drawings.Add(drawing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(drawing);
        }

        // GET: /Drawing/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drawing drawing = db.Drawings.Find(id);
            if (drawing == null)
            {
                return HttpNotFound();
            }
            return View(drawing);
        }

        // POST: /Drawing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Time")] Drawing drawing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drawing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drawing);
        }

        // GET: /Drawing/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drawing drawing = db.Drawings.Find(id);
            if (drawing == null)
            {
                return HttpNotFound();
            }
            return View(drawing);
        }

        // POST: /Drawing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Drawing drawing = db.Drawings.Find(id);
            db.Drawings.Remove(drawing);
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
