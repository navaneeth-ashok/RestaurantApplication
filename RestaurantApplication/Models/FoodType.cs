using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApplication.Models
{
    public class FoodType
    {
        [Key]
        public int TypeID { get; set; }
        public string TypeName { get; set; }

        public string TypeDisplayName { get; set; }
    }
}