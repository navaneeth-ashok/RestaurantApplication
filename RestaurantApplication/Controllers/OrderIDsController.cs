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
using RestaurantApplication.Models.ViewModel;

namespace RestaurantApplication.Controllers
{
    public class OrderIDsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderIDs
        [Authorize]
        public ActionResult Index()
        {
            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            IEnumerable<OrderID> orderIDs = orderIDsDataController.GetOrderIDs();
            return View(orderIDs);
        }

        // GET: OrderIDs/Details/5
        // This is customer facing method with custom view
        // The customers are redirected to this page once the order is set.
        // This view will contain, order details, order status and total amount
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            OrderStatusFoodDetail orderStatusFoodDetail = orderIDsDataController.ListOrderStatus(Convert.ToInt32(id));
            return View(orderStatusFoodDetail);
        }

        // GET: OrderIDs/Create
        // A function to generate the View for the Form to create new Order
        // Not required, as the orders are created automatically
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderIDs/Create
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderIDNumber,OrderIDTime")] OrderID orderID)
        {
            if (ModelState.IsValid)
            {
                OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
                System.Web.Http.IHttpActionResult actionResult = orderIDsDataController.CreateOrderID(orderID);
                return RedirectToAction("Index");
            }
            return View(orderID);
        }

        // GET: OrderIDs/Edit/5
        // To populate the form fields for the Edit View
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderIDsDataController.GetOrderID(Convert.ToInt32(id));
            OkNegotiatedContentResult<OrderID> contentResult = actionResult as OkNegotiatedContentResult<OrderID>;
            
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            OrderID orderID = contentResult.Content;
            return View(orderID);
        }

        // POST: OrderIDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "OrderIDNumber,OrderIDTime,Status,TotalAmount")] OrderID orderID)
        {
            if (ModelState.IsValid)
            {
                OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
                System.Web.Http.IHttpActionResult actionResult = orderIDsDataController.EditOrderID(orderID.OrderIDNumber, orderID);
                return RedirectToAction("Index");
            }
            return View(orderID);
        }

        // GET: OrderIDs/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Receive the order ID details from the api controller
            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            System.Web.Http.IHttpActionResult actionResult = orderIDsDataController.GetOrderID(Convert.ToInt32(id));
            OkNegotiatedContentResult<OrderID> contentResult = actionResult as OkNegotiatedContentResult<OrderID>;
            
            if (contentResult == null)
            {
                return HttpNotFound();
            }

            OrderID orderID = contentResult.Content;
            return View(orderID);
        }

        // POST: OrderIDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderIDsDataController orderIDsDataController = new OrderIDsDataController();
            orderIDsDataController.DeleteOrderID(id);
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
