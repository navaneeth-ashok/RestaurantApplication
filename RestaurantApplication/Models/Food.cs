using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApplication.Models
{
    public class Food
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal FoodPrice { get; set; }

        public decimal OfferPrice { get; set; }

        public ICollection<string> IngredientsList { get; set; }

        public int OrderCount { get; set; }

        public double FoodReviewStar { get; set; }

        [ForeignKey("FoodType")]
        public int? FoodTypeID { get; set; }
        public virtual FoodType FoodType { get; set; }
    }

    public class FoodDto
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal OfferPrice { get; set; }
        public double FoodReviewStar { get; set; }
        [ForeignKey("FoodType")]
        public int? FoodTypeID { get; set; }
        public virtual FoodType FoodType { get; set; }
    }
}