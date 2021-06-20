using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using RestaurantApplication.Models;

namespace RestaurantApplication.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [HttpGet]
        // GET: Bookings
        // This is for the admin to retrieve and view all the current bookings in the system
        public ActionResult Index()
        {
            BookingsDataController bookingsDataController = new BookingsDataController();
            IEnumerable<Booking> bookings =  bookingsDataController.GetBookings();
            //return View(db.Bookings.ToList());
            return View(bookings);
        }

        [Authorize]
        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingsDataController bookingsDataController = new BookingsDataController();
            System.Web.Http.IHttpActionResult actionResult = bookingsDataController.GetBooking(Convert.ToInt32(id));
            OkNegotiatedContentResult<Booking> contentResult = actionResult as OkNegotiatedContentResult<Booking>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Booking booking = contentResult.Content;

            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // public facing action to show the booking details with  sensitive information removed
        public ActionResult ViewBooking(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingsDataController bookingsDataController = new BookingsDataController();
            System.Web.Http.IHttpActionResult actionResult = bookingsDataController.ViewBooking(Convert.ToInt32(id));
            OkNegotiatedContentResult<BookingDto> contentResult = actionResult as OkNegotiatedContentResult<BookingDto>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            BookingDto bookingDetail = contentResult.Content;
            return View(bookingDetail);
        }

        // GET: Bookings/Create
        // Action to generate the view to create the booking
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // Action for users to create a booking at the table
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,TableNumber,NumberOfOccupants,Allergies,BookingDateTime,UserID,EmailID,PhoneNumber,BookingName")] Booking booking)
        {
            // verify the model state before sending the booking object to controller
            if (ModelState.IsValid)
            {
                BookingsDataController bookingsDataController = new BookingsDataController();
                System.Web.Http.IHttpActionResult actionResult = bookingsDataController.CreateBooking(booking);
                return RedirectToAction("ViewBooking/" + booking.BookingID);
            }
            // if model state is not valid, redirect the user to the booking page again.
            // enhancement : Add an error message to say the booking creation failed.
            return View(booking);
        }

        [Authorize]
        // GET: Bookings/Edit/5
        // Method to edit the booking details
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingsDataController bookingsDataController = new BookingsDataController();
            System.Web.Http.IHttpActionResult actionResult = bookingsDataController.GetBooking(Convert.ToInt32(id));
            OkNegotiatedContentResult<Booking> contentResult = actionResult as OkNegotiatedContentResult<Booking>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Booking booking = contentResult.Content;
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // Method to receive the edited booking details
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,TableNumber,NumberOfOccupants,Allergies,BookingDateTime,UserID,Status,EmailID,PhoneNumber,BookingName")] Booking booking)
        {
            // Preliminary inspection of ModelState validation before calling the API
            if (ModelState.IsValid)
            {
                BookingsDataController bookingsDataController = new BookingsDataController();
                System.Web.Http.IHttpActionResult actionResult = bookingsDataController.EditBooking(booking.BookingID, booking);
                return RedirectToAction("Index");
            } 
            else
            {
                System.Diagnostics.Debug.WriteLine("ModelState Invalid");
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingsDataController bookingsDataController = new BookingsDataController();
            System.Web.Http.IHttpActionResult actionResult = bookingsDataController.GetBooking(Convert.ToInt32(id));
            OkNegotiatedContentResult<Booking> contentResult = actionResult as OkNegotiatedContentResult<Booking>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Booking booking = contentResult.Content;
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingsDataController bookingsDataController = new BookingsDataController();
            System.Web.Http.IHttpActionResult actionResult = bookingsDataController.DeleteBooking(Convert.ToInt32(id));
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
