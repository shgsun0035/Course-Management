using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Assignment.Models;
using Assignment.Utils;
using Microsoft.AspNet.Identity;

namespace Assignment.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private AssignmentContainer db = new AssignmentContainer();
        private ApplicationDbContext UsersContext = new ApplicationDbContext();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookingSet = db.BookingSet.ToList();

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("admin"))
            {
                return View(bookingSet);
            }
            else
            {
                bookingSet = db.BookingSet.Where(b => b.UserId == userId).ToList();
                return View(bookingSet);
            }
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create

        public ActionResult Create()
        {
            // Standardize the dropdown menu content format
            var courseSelectList = db.CourseSet.Select(c => new
            {
                CouseId = c.Id,
                CourseDetail = c.CourseName + " " + c.CourseTime.ToString()
            }).ToList();

            ViewBag.CourseId = new SelectList(courseSelectList, "CouseId", "CourseDetail");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId")] Booking booking)
        {
            booking.UserId = User.Identity.GetUserId();
            booking.UserName = User.Identity.GetUserName();
            ModelState.Clear();
            TryValidateModel(booking);

            if (ModelState.IsValid)
            {
                db.BookingSet.Add(booking);
                db.SaveChanges();

                var bookingCourse = db.CourseSet.Where(c => c.Id == booking.CourseId).ToList();
                string bookingCourseName = bookingCourse[0].CourseName;
                string bookingCourseTime = bookingCourse[0].CourseTime.ToString();
                string bookingCourseEndTime = bookingCourse[0].CourseEndTime.ToString();
                string bookingCourseTrainerName = bookingCourse[0].TrainerName;
                string bookingCourseClassroomName = bookingCourse[0].Classroom.ClassroomName;
                string bookingCourseClassroomDescription = bookingCourse[0].Classroom.ClassroomDescription;

                // Standardize the content format of the automatic email of "Booking Successfully"
                string emailBody = "Dear " + booking.UserName + ", <br><br>Your booking: <br>Course Name: "
                    + bookingCourseName + "; <br>Course Start Time: " + bookingCourseTime + "; <br>Course End Time: " + bookingCourseEndTime
                    + "; <br>Course Trainer: " + bookingCourseTrainerName + "; <br>Course Classroom: " + bookingCourseClassroomName + " "
                    + bookingCourseClassroomDescription + "; <br><br>It has been booked successfully!";

                EmailSender emailSender = new EmailSender();
                try
                {
                    emailSender.Send(booking.UserName, "Booking Successfully", emailBody, "", "").ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }

                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.CourseSet, "Id", "CourseName", booking.CourseId);

            return View(booking);
        }

        // GET: Bookings/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            var courseSelectList = db.CourseSet.Select(c => new
            {
                CouseId = c.Id,
                CourseDetail = c.CourseName + " " + c.CourseTime.ToString()
            }).ToList();

            ViewBag.CourseId = new SelectList(courseSelectList, "CouseId", "CourseDetail", booking.CourseId);

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId")] Booking booking)
        {
            booking.UserId = User.Identity.GetUserId();
            booking.UserName = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.CourseSet, "Id", "CourseName", booking.CourseId);
            return View(booking);
        }

        // GET: Bookings/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.BookingSet.Find(id);
            db.BookingSet.Remove(booking);
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

        public ActionResult Email()
        {
            ViewBag.ItemList = CheckBoxList();
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Email(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            ViewBag.ItemList = CheckBoxList();

            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            string serverPath = Server.MapPath("~/Uploads/");

            // Handle the email attachment
            string fileExtension = "";
            string filePath = "";
            string attachmentPath = "";
            if (postedFile != null) {
                // file full name = file name + file extension
                fileExtension = Path.GetExtension(postedFile.FileName);
                filePath = myUniqueFileName + fileExtension;
                // setup the full save path
                attachmentPath = serverPath + filePath;
                postedFile.SaveAs(attachmentPath);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents, attachmentPath, fileExtension).ConfigureAwait(false);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    return View();
                }
            }

            return View();
        }

        // the checkbox for selecting the users for bulky email sending on the email page
        private List<UserEmail> CheckBoxList() 
        {
            var UserDb = UsersContext.Users.ToList();

            List<UserEmail> UserCheckBoxList = new List<UserEmail>();
            foreach (var user in UserDb)
            {
                string id = user.Id;
                string name = user.UserName;
                bool isSelected = false;
                UserCheckBoxList.Add(new UserEmail { Id = id, UserName = name, IsSelected = isSelected });
            }

            return UserCheckBoxList;
        }
    }
}
