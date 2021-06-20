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

        [HttpGet]
        [Authorize]
        // Get: /api/FoodData/ShowAllFoods
        // This is for admin to view all the food details
        public IEnumerable<Food> ShowAllFoods()
        {
            List<Food> Foods = db.Foods.ToList();

            return Foods;
        }

        // PUT: api/FoodData/UpdateFood/5
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPut]
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