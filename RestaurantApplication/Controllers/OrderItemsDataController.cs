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

        /// <summary>
        /// Get all the possible orderItems
        /// </summary>
        /// <returns>List of orderItems</returns>
        // GET: api/OrderItemsData
        [Authorize]
        [HttpGet]
        [Authorize]
        public IEnumerable<OrderItem> GetOrdersItems()
        {
            List<OrderItem> orderItems = db.OrdersItems.Include(o => o.Booking).Include(o => o.Food).Include(o => o.OrderID).ToList();
            return db.OrdersItems;
        }

        // GET: api/OrderItemsData/GetOrderItem/5
        /// <summary>
        /// Get all the order Items matching the orderID and foodID
        /// </summary>
        /// <param name="foodId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [ResponseType(typeof(OrderItem))]
        [HttpGet]
        [Authorize]
        public IHttpActionResult GetOrderItem(int foodId, int orderId)
        {
            OrderItem orderItem = db.OrdersItems.Find(foodId, orderId);
            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        /// <summary>
        /// For editing an existing order : pass orderID, foodID and the orderITem object
        /// </summary>
        /// <param name="foodId">foodID</param>
        /// <param name="orderId">orderID</param>
        /// <param name="orderItem">orderItem object</param>
        /// <returns>HTTP  Status code</returns>
        // Post: api/OrderItemsData/5
        [ResponseType(typeof(void))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult UpdateOrderItem(int foodId, int orderId, OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (foodId != orderItem.FoodID || orderId != orderItem.OrderIDNumber)
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
                if (!OrderItemExists(orderItem.FoodID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // Recalculate the order amount, because the orders could've changed by now
            RecalculateOrderAmount(orderId);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create a new OrderItem : Not required automatic method takes care of this functionality
        /// </summary>
        /// <param name="orderItem"></param>
        /// <returns></returns>
        // POST: api/OrderItemsData
        [ResponseType(typeof(OrderItem))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult CreateOrderItem(OrderItem orderItem)
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

        /// <summary>
        /// Delete OrderItem ( pass the orderID and foodID )
        /// </summary>
        /// <param name="foodId">foodID</param>
        /// <param name="orderId">OrderID</param>
        /// <returns>HTTP Status Code</returns>
        // Post: api/OrderItemsData/DeleteOrderItem/5
        [ResponseType(typeof(OrderItem))]
        [Authorize]
        [HttpPost]
        public IHttpActionResult DeleteOrderItem(int foodId, int orderId)
        {
            OrderItem orderItem = db.OrdersItems.Find(foodId, orderId);
            if (orderItem == null)
            {
                return NotFound();
            }

            db.OrdersItems.Remove(orderItem);
            db.SaveChanges();

            return Ok(orderItem);
        }

        // A function to read the OrderItems table, and recalculate the item quantity to
        // update OrderIDs table with the updated Total Amount
        // GET: api/OrderItemsData/RecalculateOrderAmount/5
        [HttpGet]
        [Authorize]
        public void RecalculateOrderAmount(int orderID)
        {
            IQueryable<OrderItem> orderItems = db.OrdersItems.Where(x => x.OrderID.OrderIDNumber == orderID);
            decimal amount = 0;
            foreach (var od in orderItems)
            {
                amount += od.SoldPrice * od.Quantity;
            }

            OrderID orderIDObject = db.OrderIDs.Find(orderID);
            orderIDObject.TotalAmount = amount;
            db.SaveChanges();

        }

        [HttpPost]
        [Authorize]
        // POST: api/OrderItemsData/RecalculateAllOrderAmounts
        // FixMe : The code is buggy, will require adding MARS to the DBContext reader to avoid multiple reader issue
        public void RecalculateAllOrderAmounts()
        {
            IEnumerable<OrderItem> orderItems = GetOrdersItems();
            foreach( var orders in orderItems)
            {
                RecalculateOrderAmount(orders.OrderIDNumber);
            }
        }

        [HttpGet]
        // POST: api/OrderItemsData/CheckOrderUpdate/5
        // This is for the order status page to detect if any order related changes has been done
        // If done, request the user to refresh the page.
        public bool CheckOrderUpdate(int orderID, decimal amount)
        {
            OrderID orderIDObject = db.OrderIDs.Find(orderID);
            if (orderIDObject.TotalAmount == amount)
            {
                return false;
            } else
            {
                return true;
            }
        }

        // Method to receive the order from the action,
        // process the order -> generate newOrder
        // splits the lists into food : quantity
        // create newOrderItems objects and write to db
        // newOrderItems contains foodId, quantity, Price etc
        // find the total amount quantity
        // POST : api/OrderItemsData/PlaceOrder/
        [HttpPost]
        public (int, OrderID) PlaceOrder(ICollection<string> foodID, ICollection<string> foodQuantity, int? bookingID)
        {
            List<string> foodList = foodID.ToList();
            List<string> quantityList = foodQuantity.ToList();

            // on receiving order from the customer, create a new orderID object
            // with a new ID and datatime
            OrderID newOrder = new OrderID
            {
                OrderIDTime = DateTime.Now,
                Status = OrderStatus.Placed
            };
            db.OrderIDs.Add(newOrder);
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine("OrderID" + newOrder.OrderIDNumber);

            // once the orderID is generated,
            // create the order with this orderID
            // fetch food details -> price, soldprice / offerprice etc

            // Server Validation to prevent orders with no item from being placed
            var orderFlag = 0;

            decimal amount = 0;
            // iterate over the foodList and quantityList, fetch the food details
            foreach (var fd in foodList.Zip(quantityList, Tuple.Create))
            {
                System.Diagnostics.Debug.WriteLine(fd.Item1 + " " + fd.Item2);
                if (Convert.ToInt32(fd.Item2) > 0)
                {
                    Food food = db.Foods.Find(Convert.ToInt32(fd.Item1));
                    OrderItem newOrderItem = new OrderItem
                    {
                        FoodID = food.FoodID,
                        BookingID = bookingID,
                        Quantity = Convert.ToInt32(fd.Item2),
                        FoodPrice = food.FoodPrice,
                        SoldPrice = food.OfferPrice,
                        OrderIDNumber = newOrder.OrderIDNumber
                    };
                    db.OrdersItems.Add(newOrderItem);
                    db.SaveChanges();
                    // for order verification
                    orderFlag++;

                    // amount calculation
                    amount += newOrderItem.Quantity * newOrderItem.SoldPrice;
                }

            }


            db.OrderIDs.Attach(newOrder);
            newOrder.TotalAmount = amount;
            db.SaveChanges();

            return (orderFlag, newOrder);
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