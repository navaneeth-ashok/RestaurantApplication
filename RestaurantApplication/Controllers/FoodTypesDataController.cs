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
    public class FoodTypesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FoodTypesData/GetFoodTypes
        public IEnumerable<FoodType> GetFoodTypes()
        {
            return db.FoodTypes;
        }

        // GET: api/FoodTypesData/GetFoodType/5
        [ResponseType(typeof(FoodType))]
        public IHttpActionResult GetFoodType(int id)
        {
            FoodType foodType = db.FoodTypes.Find(id);
            if (foodType == null)
            {
                return NotFound();
            }

            return Ok(foodType);
        }

        // POST: api/FoodTypesData/EditFoodType/5
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult EditFoodType(int id, FoodType foodType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != foodType.TypeID)
            {
                return BadRequest();
            }

            db.Entry(foodType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodTypeExists(id))
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

        // POST: api/FoodTypesData/CreateFoodType
        [ResponseType(typeof(FoodType))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateFoodType(FoodType foodType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FoodTypes.Add(foodType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = foodType.TypeID }, foodType);
        }

        // POST: api/FoodTypesData/DeleteFoodType/5
        [ResponseType(typeof(FoodType))]
        [Authorize]
        public IHttpActionResult DeleteFoodType(int id)
        {
            FoodType foodType = db.FoodTypes.Find(id);
            if (foodType == null)
            {
                return NotFound();
            }

            db.FoodTypes.Remove(foodType);
            db.SaveChanges();

            return Ok(foodType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FoodTypeExists(int id)
        {
            return db.FoodTypes.Count(e => e.TypeID == id) > 0;
        }
    }
}