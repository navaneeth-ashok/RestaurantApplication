using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantApplication.Models.ViewModel
{
    public class OrderStatusFoodDetail
    {
        public OrderID OrderIDClassDetails { get; set; }
        public List<OrderItem> OrderItemDetails { get; set; }
        public decimal TotalOrderAmount { get; set; }
    }
}