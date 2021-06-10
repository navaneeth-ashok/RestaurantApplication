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
    public class OrderItemsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OrderItemsData
        public IQueryable<OrderItem> GetOrdersItems()
        {
            return db.OrdersItems;
        }

        // GET: api/OrderItemsData/5
        [ResponseType(typeof(OrderItem))]
        public IHttpActionResult GetOrderItem(int id)
        {
            OrderItem orderItem = db.OrdersItems.Find(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // PUT: api/OrderItemsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderItem(int id, OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderItem.FoodID)
            {
                return BadRequest();
            }

            db.Entry(orderItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/OrderItemsData
        [ResponseType(typeof(OrderItem))]
        public IHttpActionResult PostOrderItem(OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrdersItems.Add(orderItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderItemExists(orderItem.FoodID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = orderItem.FoodID }, orderItem);
        }

        // DELETE: api/OrderItemsData/5
        [ResponseType(typeof(OrderItem))]
        public IHttpActionResult DeleteOrderItem(int id)
        {
            OrderItem orderItem = db.OrdersItems.Find(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            db.OrdersItems.Remove(orderItem);
            db.SaveChanges();

            return Ok(orderItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderItemExists(int id)
        {
            return db.OrdersItems.Count(e => e.FoodID == id) > 0;
        }
    }
}