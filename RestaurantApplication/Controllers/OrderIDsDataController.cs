﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using RestaurantApplication.Models;
using RestaurantApplication.Models.ViewModel;

namespace RestaurantApplication.Controllers
{
    public class OrderIDsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OrderIDsData/GetOrderIDs
        [Authorize]
        public IEnumerable<OrderID> GetOrderIDs()
        {
            List<OrderID> orderIDs = db.OrderIDs.ToList();
            return orderIDs;
        }

        // GET: api/OrderIDsData/GetOrderID/5
        [ResponseType(typeof(OrderID))]
        [Authorize]
        public IHttpActionResult GetOrderID(int id)
        {
            OrderID orderID = db.OrderIDs.Find(id);
            if (orderID == null)
            {
                return NotFound();
            }

            return Ok(orderID);
        }


        // GET:
        [HttpGet]
        public OrderStatusFoodDetail ListOrderStatus(int? id)
        {
            // Receive the order ID details from the api controller
            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderIDsDataController.GetOrderID(Convert.ToInt32(id));
            OkNegotiatedContentResult<OrderID> contentResult = actionResult as OkNegotiatedContentResult<OrderID>;
            OrderID orderID = contentResult.Content;

            // Check the items ordered using this order ID
            int orderIDToCheck = (int)id;
            List<OrderItem> orderItems = db.OrdersItems.Where(o => o.OrderID.OrderIDNumber == orderIDToCheck).ToList();
            decimal amount = orderID.TotalAmount;

            // Populate OrderStatusFoodDetail model object using the data received above
            OrderStatusFoodDetail orderStatusFoodDetail = new OrderStatusFoodDetail
            {
                OrderIDClassDetails = orderID,
                OrderItemDetails = orderItems,
                TotalOrderAmount = amount
            };

            return orderStatusFoodDetail;
        }



        // POST: api/OrderIDsData/EditOrderID/5
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult EditOrderID(int id, OrderID orderID)
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

        // POST: api/OrderIDsData/CreateOrderID
        [ResponseType(typeof(OrderID))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateOrderID(OrderID orderID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderIDs.Add(orderID);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = orderID.OrderIDNumber }, orderID);
        }

        // POST: api/OrderIDsData/5
        [ResponseType(typeof(OrderID))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult DeleteOrderID(int id)
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