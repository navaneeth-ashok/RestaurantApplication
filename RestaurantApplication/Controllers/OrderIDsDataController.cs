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
    public class OrderIDsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OrderIDsData
        public IQueryable<OrderID> GetOrderIDs()
        {
            return db.OrderIDs;
        }

        // GET: api/OrderIDsData/5
        [ResponseType(typeof(OrderID))]
        public IHttpActionResult GetOrderID(string id)
        {
            OrderID orderID = db.OrderIDs.Find(id);
            if (orderID == null)
            {
                return NotFound();
            }

            return Ok(orderID);
        }

        // PUT: api/OrderIDsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderID(int id, OrderID orderID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderID.OrderIDNumber)
            {
                return BadRequest();
            }

            db.Entry(orderID).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderIDExists(id))
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

        // POST: api/OrderIDsData
        [ResponseType(typeof(OrderID))]
        public IHttpActionResult PostOrderID(OrderID orderID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderIDs.Add(orderID);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderIDExists(orderID.OrderIDNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = orderID.OrderIDNumber }, orderID);
        }

        // DELETE: api/OrderIDsData/5
        [ResponseType(typeof(OrderID))]
        public IHttpActionResult DeleteOrderID(string id)
        {
            OrderID orderID = db.OrderIDs.Find(id);
            if (orderID == null)
            {
                return NotFound();
            }

            db.OrderIDs.Remove(orderID);
            db.SaveChanges();

            return Ok(orderID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderIDExists(int id)
        {
            return db.OrderIDs.Count(e => e.OrderIDNumber == id) > 0;
        }
    }
}