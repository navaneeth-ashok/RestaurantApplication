﻿using System;
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
        [Authorize]
        public ActionResult Index()
        {
            var ordersItems = db.OrdersItems.Include(o => o.Booking).Include(o => o.Food).Include(o => o.OrderID);
            return View(ordersItems.ToList());
        }

        // GET: OrderItems/Details/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrdersItems.Find(id);
            db.OrdersItems.Remove(orderItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // OrderItems/PlaceOrder/
        [HttpPost]
        public ActionResult PlaceOrder(ICollection<string> foodID, ICollection<string> foodQuantity, int? bookingID)
        {
            List<string> foodList = foodID.ToList();
            List<string> quantityList = foodQuantity.ToList();
            //System.Diagnostics.Debug.WriteLine("FoodID" + foodList[0]);
            //System.Diagnostics.Debug.WriteLine("FoodQuantity" + quantityList[0]);
            //System.Diagnostics.Debug.WriteLine("FoodID" + foodList[1]);
            //System.Diagnostics.Debug.WriteLine("FoodQuantity" + quantityList[1]);
            //System.Diagnostics.Debug.WriteLine(DateTime.Now);


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

            // Booking id default Setting
            //int bookingIDNew = 1;
            //if(bookingID != null)
            //{
            //    bookingIDNew = (int)bookingID;
            //}

            // Server Validation to prevent orders with no item from being placed
            var orderFlag = 0;

            decimal amount = 0;
            // iterate over the foodList and quantityList, fetch the food details
            foreach (var fd in foodList.Zip(quantityList, Tuple.Create))
            {
                System.Diagnostics.Debug.WriteLine(fd.Item1 + " " + fd.Item2);
                if(Convert.ToInt32(fd.Item2) > 0)
                {
                    Food food = db.Foods.Find(Convert.ToInt32(fd.Item1));
                    OrderItem newOrderItem = new OrderItem
                    {
                        FoodID = food.FoodID,
                        BookingID = bookingID, // lets have bookingID as 1 if booking is not mentioned #WIP: Fetch the BookingID
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

            if(orderFlag == 0)
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
