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

        /// <summary>
        /// Get the list of food types present
        /// </summary>
        /// <returns>list of food types</returns>
        // GET: api/FoodTypesData/GetFoodTypes
        public IEnumerable<FoodType> GetFoodTypes()
        {
            return db.FoodTypes;
        }

        /// <summary>
        /// Get a foodType
        /// </summary>
        /// <param name="id">ID of the food type</param>
        /// <returns>FoodType object</returns>
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

        /// <summary>
        /// Edit a food type
        /// </summary>
        /// <param name="id">id of the type to be edited</param>
        /// <param name="foodType">foodtype object with the edits</param>
        /// <returns>HTTP status</returns>
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

        /// <summary>
        /// Create a new food type
        /// </summary>
        /// <param name="foodType">foodtype object</param>
        /// <returns>newly created food object with ID</returns>
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

        /// <summary>
        /// Delete the food Type 
        /// </summary>
        /// <param name="id">ID of the item to be deleted</param>
        /// <returns>HTTP status code with foodobject</returns>
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