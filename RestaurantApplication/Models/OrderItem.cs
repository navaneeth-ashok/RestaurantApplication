using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApplication.Models
{
    public class OrderItem
    {
        
        [Key, Column(Order = 0)]
        [ForeignKey("Food")]
        public int FoodID { get; set; }
        public virtual Food Food { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Booking")]
        public int BookingID { get; set;  }
        public virtual Booking Booking { get; set; }
        public int Quantity { get; set; }

        public decimal FoodPrice { get; set; }
        public decimal SoldPrice { get; set; }

        [ForeignKey("OrderID")]
        public string OrderIDNumber { get; set; }
        public virtual OrderID OrderID { get; set; }
    }
}