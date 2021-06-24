using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using RestaurantApplication.Models;

namespace RestaurantApplication.Controllers
{
    public class OrderItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderItems
        [Authorize]
        public ActionResult Index()
        {
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            IEnumerable<OrderItem> orderItems = orderItemsDataController.GetOrdersItems();
            return View(orderItems);
        }

        // GET: OrderItems/Details/5
        [Authorize]
        public ActionResult Details(int? foodId, int? orderId)
        {
            if (foodId == null || orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderItemsDataController.GetOrderItem(Convert.ToInt32(foodId), Convert.ToInt32(orderId));
            OkNegotiatedContentResult<OrderItem> contentResult = actionResult as OkNegotiatedContentResult<OrderItem>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            OrderItem orderItem = contentResult.Content;
            return View(orderItem);
        }

        // GET: OrderItems/Create
        // This code is not required as the Orders are created automatically by the PlaceOrder code
        //[Authorize]
        //public ActionResult Create()
        //{
        //    ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies");
        //    ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName");
        //    ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber");
        //    return View();
        //}

        // POST: OrderItems/Create
        // This code is not required as the Orders are created automatically by the PlaceOrder code
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Create([Bind(Include = "FoodID,BookingID,Quantity,FoodPrice,SoldPrice,OrderIDNumber")] OrderItem orderItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.OrdersItems.Add(orderItem);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies", orderItem.BookingID);
        //    ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName", orderItem.FoodID);
        //    ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber", orderItem.OrderIDNumber);
        //    return View(orderItem);
        //}

        // GET: OrderItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? foodId, int? orderId)
        {
            if (foodId == null || orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderItemsDataController.GetOrderItem(Convert.ToInt32(foodId), Convert.ToInt32(orderId));
            OkNegotiatedContentResult<OrderItem> contentResult = actionResult as OkNegotiatedContentResult<OrderItem>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            
            OrderItem orderItem = contentResult.Content;
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "FoodID,BookingID,Quantity,FoodPrice,SoldPrice,OrderIDNumber")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
                System.Web.Http.IHttpActionResult actionResult = orderItemsDataController.UpdateOrderItem(Convert.ToInt32(orderItem.FoodID), Convert.ToInt32(orderItem.OrderIDNumber), orderItem);
                OkNegotiatedContentResult<OrderItem> contentResult = actionResult as OkNegotiatedContentResult<OrderItem>;
                return RedirectToAction("Index");
            }

            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? foodId, int? orderId)
        {
            if (foodId == null || orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderItemsDataController.GetOrderItem(Convert.ToInt32(foodId), Convert.ToInt32(orderId));
            OkNegotiatedContentResult<OrderItem> contentResult = actionResult as OkNegotiatedContentResult<OrderItem>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            OrderItem orderItem = contentResult.Content;
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int foodId, int orderId)
        {
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            orderItemsDataController.DeleteOrderItem(foodId, orderId);
            return RedirectToAction("Index");
        }

        // This method receives the food and quantity list along with bookingID (opt)
        // Send the same over to the API to generate an OrderID and redirects the user to the status page
        // POST: OrderItems/PlaceOrder/
        [HttpPost]
        public ActionResult PlaceOrder(ICollection<string> foodID, ICollection<string> foodQuantity, int? bookingID)
        {
            OrderItemsDataController orderItemsDataController = new OrderItemsDataController();
            (int orderFlag, OrderID newOrder) = orderItemsDataController.PlaceOrder(foodID, foodQuantity, bookingID);

            if (orderFlag == 0)
            {
                return RedirectToAction("List", "Food");
            } else
            {
                return RedirectToAction("/Details/" + newOrder.OrderIDNumber, "OrderIDs");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
