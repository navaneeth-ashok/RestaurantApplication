using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantApplication.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int TableNumber { get; set; }
        [Required(ErrorMessage = "Number of guests is required.")]
        public int NumberOfOccupants { get; set; }
        public string Allergies { get; set; }

        public DateTime BookingDateTime { get; set; }

        // future enhancement of user table
        public string UserID { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email ID is required.")]
        public string EMailID { get; set; }
        public BookingStatus Status { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string BookingName { get; set; }
    }

    public class BookingDto
    {
        public int BookingID { get; set; }
        public int NumberOfOccupants { get; set; }
        public DateTime BookingDateTime { get; set; }
        public BookingStatus Status { get; set; }

    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}