using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantApplication.Models;
using RestaurantApplication.Models.ViewModel;

namespace RestaurantApplication.Controllers
{
    public class OrderIDsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderIDs
        public ActionResult Index()
        {
            return View(db.OrderIDs.ToList());
        }

        // GET: OrderIDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderID orderID = db.OrderIDs.Find(id);
            int orderIDToCheck = (int)id;
            List<OrderItem> orderItems = db.OrdersItems.Where(o => o.OrderID.OrderIDNumber == orderIDToCheck).ToList();
            // debug code
            // Amount price is calculated on Order Now, this code should be removed
            decimal amount = orderID.TotalAmount;
            //foreach( var item in orderItems)
            //{
            //    System.Diagnostics.Debug.WriteLine(item.Food.FoodName + " " + item.SoldPrice);
            //    amount += item.Quantity * item.SoldPrice;
            //}

            OrderStatusFoodDetail orderStatusFoodDetail = new OrderStatusFoodDetail
            {
                OrderIDClassDetails = orderID,
                OrderItemDetails = orderItems,
                TotalOrderAmount = amount
            };

            // adding TotalAmount to the 

            if (orderID == null)
            {
                return HttpNotFound();
            }
            return View(orderStatusFoodDetail);
        }

        // GET: OrderIDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderIDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderIDNumber,OrderIDTime")] OrderID orderID)
        {
            if (ModelState.IsValid)
            {
                db.OrderIDs.Add(orderID);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderID);
        }

        // GET: OrderIDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderID orderID = db.OrderIDs.Find(id);
            if (orderID == null)
            {
                return HttpNotFound();
            }
            return View(orderID);
        }

        // POST: OrderIDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderIDNumber,OrderIDTime,Status")] OrderID orderID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderID).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderID);
        }

        // GET: OrderIDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderID orderID = db.OrderIDs.Find(id);
            if (orderID == null)
            {
                return HttpNotFound();
            }
            return View(orderID);
        }

        // POST: OrderIDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderID orderID = db.OrderIDs.Find(id);
            db.OrderIDs.Remove(orderID);
            db.SaveChanges();
            return RedirectToAction("Index");
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
