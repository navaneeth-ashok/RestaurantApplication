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
    public class FoodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: Foods
        public ActionResult Index()
        {
            FoodDataController foodDataController = new FoodDataController();
            IEnumerable<Food> foods = foodDataController.ShowAllFoods();
            return View(foods);
        }

        // GET: Foods/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDataController foodDataController = new FoodDataController();
            System.Web.Http.IHttpActionResult actionResult = foodDataController.Details(Convert.ToInt32(id));
            OkNegotiatedContentResult<Food> contentResult = actionResult as OkNegotiatedContentResult<Food>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Food food = contentResult.Content;
            return View(food);
        }

        // GET: Foods/Create
        // This method is for generating the form for creating a new food item.
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "FoodID,FoodName,FoodDescription,FoodPrice,OfferPrice,OrderCount,FoodReviewStar,FoodTypeID")] Food food)
        {
            // verify the model state is valid or not before requesting the API to add the food
            if (ModelState.IsValid)
            {
                FoodDataController foodDataController = new FoodDataController();
                System.Web.Http.IHttpActionResult actionResult = foodDataController.AddFood(food);
                return RedirectToAction("Index");
            }

            return View(food);
        }

        // GET: Foods/Edit/5
        // For the admin to edit the food details, 
        // This is just for the view to generate the fields with pre-filled information in input values
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDataController foodDataController = new FoodDataController();
            System.Web.Http.IHttpActionResult actionResult = foodDataController.Details(Convert.ToInt32(id));
            OkNegotiatedContentResult<Food> contentResult = actionResult as OkNegotiatedContentResult<Food>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Food food = contentResult.Content;
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "FoodID,FoodName,FoodDescription,FoodPrice,OfferPrice,OrderCount,FoodReviewStar,FoodTypeID")] Food food)
        {
            // Preliminary inspection of ModelState validation before calling the API
            if (ModelState.IsValid)
            {
                FoodDataController foodDataController = new FoodDataController();
                System.Web.Http.IHttpActionResult actionResult = foodDataController.UpdateFood(food.FoodID, food);
                return RedirectToAction("Index");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ModelState Invalid");
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        // This is to delete the food from the system.
        // Fetched the food ID and asks for confirmation
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDataController foodDataController = new FoodDataController();
            System.Web.Http.IHttpActionResult actionResult = foodDataController.Details(Convert.ToInt32(id));
            OkNegotiatedContentResult<Food> contentResult = actionResult as OkNegotiatedContentResult<Food>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            Food food = contentResult.Content;
            return View(food);
        }

        // POST: Foods/Delete/5
        // POST reques tto the API to delete the food with the id received as a parameter
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodDataController foodDataController = new FoodDataController();
            System.Web.Http.IHttpActionResult actionResult = foodDataController.DeleteFood(Convert.ToInt32(id));
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
