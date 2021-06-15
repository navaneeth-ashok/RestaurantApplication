using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApplication.Models
{
    public class OrderID
    {
        [Key]
        public int OrderIDNumber { get; set; }
        public DateTime OrderIDTime { get; set; }
        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

    }

    public enum OrderStatus
    {
        Placed,
        Confirmed,
        Cooking,
        Prepared,
        OnDelivery,
        Delivered,
        Served,
        Paid,
        CancelledByUser,
        CancelledByRestaurant,
        CancelledByDelivery,
        Completed
    }
}