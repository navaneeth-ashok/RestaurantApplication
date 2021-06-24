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
using RestaurantApplication.Models;

namespace RestaurantApplication.Controllers
{
    public class BookingsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/BookingsData/GetBookings
        /// <summary>
        /// Get all the bookings
        /// </summary>
        /// <returns> List of all the bookings</returns>
        [Authorize]
        public IEnumerable<Booking> GetBookings()
        {
            List<Booking> bookings = db.Bookings.ToList();
            
            return bookings;
        }

        // GET: api/BookingsData/GetBooking/5
        /// <summary>
        /// Get the details about one booking
        /// </summary>
        /// <param name="id">booking id</param>
        /// <returns>Booking object with the specified Booking ID</returns>
        [ResponseType(typeof(Booking))]
        [Authorize]
        public IHttpActionResult GetBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // GET: api/BookingsData/ViewBooking/5
        // This is public facing API
        // This can be used by non logged-in guests to view the status of their booking
        // In this view all the personal and sensitive information about the booking is hidden
        /// <summary>
        /// Used by unauthenticated users to view the booking status
        /// </summary>
        /// <param name="id">booking ID</param>
        /// <returns>Booking DTO object with sensitive and private info removed</returns>
        [HttpGet]
        [ResponseType(typeof(Booking))]
        public IHttpActionResult ViewBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            BookingDto bookingDetail = new BookingDto
            {
                BookingID = booking.BookingID,
                Status = booking.Status,
                NumberOfOccupants = booking.NumberOfOccupants,
                BookingDateTime = booking.BookingDateTime
            };

            return Ok(bookingDetail);
        }

        // POST: api/BookingsData/EditBooking/5
        /// <summary>
        /// Edit the status of a booking
        /// </summary>
        /// <param name="id">booking ID</param>
        /// <param name="booking">Booking Model object on which the edit has to happen</param>
        /// <returns>HTTP response code</returns>
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult EditBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/BookingsData/CreateBooking
        /// <summary>
        /// Create a booking
        /// </summary>
        /// <param name="booking">Booking Object</param>
        /// <returns>booking object newly created with booking ID</returns>
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult CreateBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingID }, booking);
        }

        // POST: api/BookingsData/DeleteBooking/5
        /// <summary>
        /// Delete an existing booking
        /// </summary>
        /// <param name="id">bookign ID</param>
        /// <returns>HTTP status code if pass : Else same delete page if failed</returns>
        [ResponseType(typeof(Booking))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            try
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex) /* when (((System.Data.SqlClient.SqlException)ex.InnerException).Number== 547)*/
            {
                // Exception to handle delete if the key is referenced somewhere else
                // Fix : Ondelete cascade
                //var q = ex.InnerException as System.Data.SqlClient.SqlException;
                //var ErrorCode = q.Number;

                string message = ex.InnerException == null ? "" : ex.InnerException.Message;
                System.Diagnostics.Debug.WriteLine("Cannot delete booking, as an order is associated with it");
                System.Diagnostics.Debug.WriteLine(message);
               
            }
          

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}