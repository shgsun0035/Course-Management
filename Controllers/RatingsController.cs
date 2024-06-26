using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;
using Microsoft.AspNet.Identity;

namespace Assignment.Controllers
{
    [Authorize]
    public class RatingsController : Controller
    {
        private AssignmentContainer db = new AssignmentContainer();

        // GET: Ratings
        public ActionResult Index()
        {
            var ratingSet = db.RatingSet.Include(r => r.Course).ToList();

            var UserId = User.Identity.GetUserId();

            var UserName = User.Identity.GetUserName();

            if (User.IsInRole("admin"))
            {
                return View(ratingSet);
            }
            else 
            {
                ratingSet = db.RatingSet.Where(r => r.UserId == UserId).ToList();
                return View(ratingSet);
            }
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.RatingSet.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.CourseSet, "Id", "CourseName");
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, CourseId, RatingValue")] Rating rating)
        {
            rating.UserId = User.Identity.GetUserId();
            rating.UserName = User.Identity.GetUserName();
            ModelState.Clear();
            TryValidateModel(rating);

            if (ModelState.IsValid)
            {
                db.RatingSet.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.CourseSet, "Id", "CourseName", rating.CourseId);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.RatingSet.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
/*            ViewBag.CourseId = rating.CourseId;*/
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, UserId, CourseId, RatingValue")] Rating rating)
        {
            // get the orginal data
            var currentRating = db.RatingSet.FirstOrDefault(r => r.Id == rating.Id);
            if (currentRating == null)
            {
                return HttpNotFound();
            }
            currentRating.RatingValue = rating.RatingValue;

            if (ModelState.IsValid)
            {
                db.Entry(currentRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
/*            ViewBag.CourseId = rating.CourseId;*/
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.RatingSet.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.RatingSet.Find(id);
            db.RatingSet.Remove(rating);
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
