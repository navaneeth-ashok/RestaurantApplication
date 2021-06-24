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
    public class FoodDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FoodData/ListFoods
        /// <summary>
        /// Get the list of food items present in the menu
        /// </summary>
        /// <returns>FoodDto objects containing only relevant items</returns>
        [HttpGet]
        public IEnumerable<FoodDto> ListFoods()
        {
            List<Food> Foods = db.Foods.ToList();
            List<FoodDto> FoodDtos = new List<FoodDto>();

            Foods.ForEach(f => FoodDtos.Add(new FoodDto() {

                FoodID = f.FoodID,
                FoodName = f.FoodName,
                FoodDescription = f.FoodDescription,
                OfferPrice = f.OfferPrice,
                FoodReviewStar = f.FoodReviewStar,
                FoodTypeID = f.FoodTypeID,
                FoodType = f.FoodType
            }));
            return FoodDtos;
        }

        // GET: api/FoodData/FindFood/5
        // This method is used to generate the FoodDto for customer, masking few sensitive details
        /// <summary>
        /// To display the details about a particular food item
        /// </summary>
        /// <param name="id">Food ID</param>
        /// <returns>FoodDto object</returns>
        [ResponseType(typeof(Food))]
        [HttpGet]
        public IHttpActionResult FindFood(int id)
        {
            Food food = db.Foods.Find(id);
            FoodDto foodDto = new FoodDto()
            {
                FoodName = food.FoodName,
                FoodDescription = food.FoodDescription,
                OfferPrice = food.OfferPrice,
                FoodReviewStar = food.FoodReviewStar
            };
            if (food == null)
            {
                return NotFound();
            }

            return Ok(foodDto);
        }

        // GET: api/FoodData/Details/5
        // This method is used to generate the food object for admin,
        // contains all possible food db data
        /// <summary>
        /// Fetch and show all the details about a particular food item
        /// </summary>
        /// <param name="id">food id</param>
        /// <returns>Food object</returns>
        [Authorize]
        [ResponseType(typeof(Food))]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Food food = db.Foods.Find(id);
           
            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        // Get: /api/FoodData/ShowAllFoods
        /// <summary>
        /// This is for admin to view all the food details
        /// </summary>
        /// <returns>List of Food items</returns>
        [HttpGet]
        [Authorize]
        public IEnumerable<Food> ShowAllFoods()
        {
            List<Food> Foods = db.Foods.ToList();

            return Foods;
        }

        /// <summary>
        /// Update the food item details by the admin
        /// </summary>
        /// <param name="id">food id</param>
        /// <param name="food">food object to be edited</param>
        /// <returns>Save the changes to DB and returns HTTP status code</returns>
        // POST: api/FoodData/UpdateFood/5
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateFood(int id, Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != food.FoodID)
            {
                return BadRequest();
            }

            db.Entry(food).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
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

        /// <summary>
        /// Add a new food item to the DB
        /// </summary>
        /// <param name="food">Food Object</param>
        /// <returns>returns the newly created object with the ID</returns>
        // POST: api/FoodData/AddFood
        [ResponseType(typeof(Food))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddFood(Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Foods.Add(food);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = food.FoodID }, food);
        }

        /// <summary>
        /// Delete the food item
        /// </summary>
        /// <param name="id">id of the food entry to be deleted</param>
        /// <returns>HTTP status code</returns>
        // POST: api/FoodData/DeleteFood/5
        [ResponseType(typeof(Food))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteFood(int id)
        {
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return NotFound();
            }

            db.Foods.Remove(food);
            db.SaveChanges();

            return Ok(food);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FoodExists(int id)
        {
            return db.Foods.Count(e => e.FoodID == id) > 0;
        }
    }
}