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

                FoodName = f.FoodName,
                FoodDescription = f.FoodDescription,
                OfferPrice = f.OfferPrice,
                FoodReviewStar = f.FoodReviewStar
            }));
            return FoodDtos;
        }

        // GET: api/FoodData/FindFood/5
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

        // PUT: api/FoodData/UpdateFood/5
        [ResponseType(typeof(void))]
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

        // POST: api/FoodData/DeleteAnimal/5
        [ResponseType(typeof(Food))]
        [HttpPost]
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