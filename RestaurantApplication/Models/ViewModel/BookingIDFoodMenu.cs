using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantApplication.Models;

namespace RestaurantApplication.Models.ViewModel
{
    public class BookingIDFoodMenu
    {
        public IEnumerable<FoodDto> Food { get; set; }
        public int BookingID { get; set; }
    }
}