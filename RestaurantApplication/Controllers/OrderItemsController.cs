using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantApplication.Models;

namespace RestaurantApplication.Controllers
{
    public class OrderItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderItems
        public ActionResult Index()
        {
            var ordersItems = db.OrdersItems.Include(o => o.Booking).Include(o => o.Food).Include(o => o.OrderID);
            return View(ordersItems.ToList());
        }

        // GET: OrderItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrdersItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        public ActionResult Create()
        {
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies");
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName");
            ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodID,BookingID,Quantity,FoodPrice,SoldPrice,OrderIDNumber")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.OrdersItems.Add(orderItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies", orderItem.BookingID);
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName", orderItem.FoodID);
            ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber", orderItem.OrderIDNumber);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrdersItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies", orderItem.BookingID);
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName", orderItem.FoodID);
            ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber", orderItem.OrderIDNumber);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodID,BookingID,Quantity,FoodPrice,SoldPrice,OrderIDNumber")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingID = new SelectList(db.Bookings, "BookingID", "Allergies", orderItem.BookingID);
            ViewBag.FoodID = new SelectList(db.Foods, "FoodID", "FoodName", orderItem.FoodID);
            ViewBag.OrderIDNumber = new SelectList(db.OrderIDs, "OrderIDNumber", "OrderIDNumber", orderItem.OrderIDNumber);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrdersItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrdersItems.Find(id);
            db.OrdersItems.Remove(orderItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // OrderItems/PlaceOrder/
        [HttpPost]
        public void PlaceOrder(ICollection<string> foodID, ICollection<string> foodQuantity)
        {
            List<string> foodList = foodID.ToList();
            List<string> quantityList = foodQuantity.ToList();
            System.Diagnostics.Debug.WriteLine("FoodID" + foodList[0]);
            System.Diagnostics.Debug.WriteLine("FoodQuantity" + quantityList[0]);
            System.Diagnostics.Debug.WriteLine("FoodID" + foodList[1]);
            System.Diagnostics.Debug.WriteLine("FoodQuantity" + quantityList[1]);
            System.Diagnostics.Debug.WriteLine(DateTime.Now);

            OrderID newOrder = new OrderID
            {
                OrderIDTime = DateTime.Now
            };
            db.OrderIDs.Add(newOrder);
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine("OrderID" + newOrder.OrderIDNumber);
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
