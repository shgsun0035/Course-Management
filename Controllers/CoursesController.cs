using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;

namespace Assignment.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private AssignmentContainer db = new AssignmentContainer();

        // GET: Courses
        public ActionResult Index()
        {

            // get the average rating value on the index page
            var courseSet = db.CourseSet.ToList();
            foreach (var course in courseSet)
            {
                var ratingSet = db.RatingSet.Where(r => r.CourseId == course.Id).ToList();
                var totalRating = 0;
                foreach (var rating in ratingSet)
                {
                    totalRating += Int32.Parse(rating.RatingValue);
                }
                double avgRating = double.Parse(totalRating.ToString()) / double.Parse(ratingSet.Count.ToString());
                course.CourseRating = avgRating.ToString("0.0");
            }

            return View(courseSet);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // redirect to the "PageNotFound" error page
            Course course = db.CourseSet.Find(id);
            if (course == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }

            // get the average rating value on the detail page
            var ratingSet = db.RatingSet.Where(r => r.CourseId == course.Id).ToList();
            var totalRating = 0;
            foreach (var rating in ratingSet)
            {
                totalRating += Int32.Parse(rating.RatingValue);
            }
            double avgRating = double.Parse(totalRating.ToString()) / double.Parse(ratingSet.Count.ToString());
            course.CourseRating = avgRating.ToString("0.0");

            return View(course);
        }

        // GET: Courses/Create?date=YYYY-MM-DD
        [Authorize(Roles = "admin")]
        public ActionResult Create(String date)
        {
            ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName");
            if (date == null)
                return RedirectToAction("Index");
            Course e = new Course();
            DateTime convertedStartTime = DateTime.Parse(date);
            e.CourseTime = convertedStartTime;
            return View(e);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseName,CourseDescription,CourseTime,CourseEndTime,TrainerName,ClassroomId")] Course course)
        {
            course.CourseRating = "1";

            // if it is not null values of Course start time and end time
            if (course.CourseTime != null && course.CourseEndTime != null)
            {
                // valid time
                if (validateDate(course.CourseTime, (DateTime)course.CourseEndTime) == 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.CourseSet.Add(course);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    return View(course);
                }
                // invalid time - end time is ealier than start time
                else if (validateDate(course.CourseTime, (DateTime)course.CourseEndTime) == 1)
                {
                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    ViewBag.ErrorMessage = "End date cannot be ealier than Start date";
                    return View(course);
                }
                // invalid time - the selected time overlaps with the existing courses
                else
                {
                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    ViewBag.ErrorMessage = "Course time overlaps with the existing course time";
                    return View(course);
                }
            }
            // if it is null value of Course start time and end time
            else
            {
                ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                return View(course);
            }
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.CourseSet.Find(id);
            if (course == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseName,CourseDescription,CourseTime,CourseEndTime,CourseRating,TrainerName,ClassroomId")] Course course)
        {
            // if it is not null values of Course start time and end time
            if (course.CourseTime != null && course.CourseEndTime != null)
            {
                // valid time
                if (validateDate(course.CourseTime, (DateTime)course.CourseEndTime) == 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(course).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    return View(course);
                }
                // invalid time - end time is ealier than start time
                else if (validateDate(course.CourseTime, (DateTime)course.CourseEndTime) == 1)
                {
                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    ViewBag.ErrorMessage = "End date cannot be ealier than Start date";
                    return View(course);
                }
                // invalid time - the selected time overlaps with the existing courses
                else
                {
                    ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                    ViewBag.ErrorMessage = "Course time overlaps with the existing course time";
                    return View(course);
                }
            }
            // if it is null value of Course start time and end time
            else
            {
                ViewBag.ClassroomId = new SelectList(db.ClassroomSet, "Id", "ClassroomName", course.ClassroomId);
                return View(course);
            }
            
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.CourseSet.Find(id);

            // redirect to the "PageNotFound" error page
            if (course == null)
            {
                return RedirectToAction("PageNotFound", "Error");
            }
            
            // get the average rating value on the delete page
            var ratingSet = db.RatingSet.Where(r => r.CourseId == course.Id).ToList();
            var totalRating = 0;
            foreach (var rating in ratingSet)
            {
                totalRating += Int32.Parse(rating.RatingValue);
            }
            double avgRating = double.Parse(totalRating.ToString()) / double.Parse(ratingSet.Count.ToString());
            course.CourseRating = avgRating.ToString("0.0");

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.CourseSet.Find(id);
            db.CourseSet.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Chart(string startTime, string endTime)
        {
            if (startTime.Equals("") || endTime.Equals("")) {
                TempData["ErrorMessage"] = "Input Time cannot be empty";
                return RedirectToAction("Index");
            }
            DateTime convertedStartTime = DateTime.Parse(startTime);
            DateTime convertedEndTime = DateTime.Parse(endTime);

            // valid time of chart start time and end time
            if (validateDate(convertedStartTime, convertedEndTime) != 1)
            {
                var chartItemSet = new List<Chart>();
                var courseSet = db.CourseSet.ToList();
                var bookingSet = db.BookingSet.ToList();

                foreach (var course in courseSet)
                {
                    if (course.CourseTime >= convertedStartTime && course.CourseEndTime <= convertedEndTime)
                    {
                        Chart chartItem = new Chart();
                        chartItem.CourseName = course.CourseName;
                        var bookingCount = 0;
                        foreach (var booking in bookingSet)
                        {
                            if (booking.CourseId == course.Id)
                            {
                                bookingCount += 1;
                            }
                        }
                        chartItem.BookingAmount = bookingCount;
                        chartItemSet.Add(chartItem);
                    }

                }
                ViewBag.StartTime = startTime;
                ViewBag.EndTime = endTime;

                return View(chartItemSet);
            }
            else 
            {
                // Pass error message to Index view
                TempData["ErrorMessage"] = "End date cannot be ealier than Start date";
                return RedirectToAction("Index");

            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Method for checking overlap and invaid start time and end time
        private int validateDate(DateTime startTime, DateTime endTime) 
        {
            var courseSet = db.CourseSet.AsNoTracking().ToList();
            foreach (var course in courseSet)
            {
                if (startTime > endTime)
                {
                    return 1;
                }
                
                if (startTime > course.CourseTime && startTime < course.CourseEndTime)
                {
                    return 2;
                }
                
                if (endTime > course.CourseTime && endTime < course.CourseEndTime)
                {
                    return 2;
                }
                
                if (startTime <= course.CourseTime && endTime >= course.CourseEndTime)
                {
                    return 2;
                }
            }

            return 0;
 
        }
    }
}
