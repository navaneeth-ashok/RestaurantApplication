using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantApplication.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int TableNumber { get; set; }
        public int NumberOfOccupants { get; set; }
        public string Allergies { get; set; }

        public DateTime BookingDateTime { get; set; }

        // future enhancement of user table
        public string UserID { get; set; }
        public string PhoneNumber { get; set; }
        public string EMailID { get; set; }
        public BookingStatus Status { get; set; }
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