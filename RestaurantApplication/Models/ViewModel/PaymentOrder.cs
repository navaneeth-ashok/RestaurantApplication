using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantApplication.Models.ViewModel
{
    public class PaymentOrder
    {
        public OrderStatusFoodDetail orderItems;
        public PaymentDetail paymentItems;
    }
}