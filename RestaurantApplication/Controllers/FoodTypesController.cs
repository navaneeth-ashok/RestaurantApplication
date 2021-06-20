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
    public class FoodTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FoodTypes
        public ActionResult Index()
        {
            FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
            IEnumerable<FoodType> foodTypes = foodTypesDataController.GetFoodTypes();
            return View(foodTypes);
        }

        // GET: FoodTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
            System.Web.Http.IHttpActionResult actionResult = foodTypesDataController.GetFoodType(Convert.ToInt32(id));
            OkNegotiatedContentResult<FoodType> contentResult = actionResult as OkNegotiatedContentResult<FoodType>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            FoodType foodType = contentResult.Content;
            return View(foodType);
        }

        // GET: FoodTypes/Create
        // For generating the view for creating the FoodTypes
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,TypeDisplayName")] FoodType foodType)
        {
            // pre-validation in the action method for the input foodType before sending to the API
            if (ModelState.IsValid)
            {
                FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
                foodTypesDataController.CreateFoodType(foodType);
                return RedirectToAction("Index");
            }

            return View(foodType);
        }

        // GET: FoodTypes/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
            System.Web.Http.IHttpActionResult actionResult = foodTypesDataController.GetFoodType(Convert.ToInt32(id));
            OkNegotiatedContentResult<FoodType> contentResult = actionResult as OkNegotiatedContentResult<FoodType>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            FoodType foodType = contentResult.Content;
            return View(foodType);
        }

        // POST: FoodTypes/Edit/5
        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,TypeDisplayName")] FoodType foodType)
        {
            if (ModelState.IsValid)
            {
                FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
                System.Web.Http.IHttpActionResult actionResult = foodTypesDataController.EditFoodType(foodType.TypeID, foodType);
                return RedirectToAction("Index");
            }
            return View(foodType);
        }

        // GET: FoodTypes/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
            System.Web.Http.IHttpActionResult actionResult = foodTypesDataController.GetFoodType(Convert.ToInt32(id));
            OkNegotiatedContentResult<FoodType> contentResult = actionResult as OkNegotiatedContentResult<FoodType>;
            if (contentResult == null)
            {
                return HttpNotFound();
            }
            FoodType foodType = contentResult.Content;
            return View(foodType);
        }

        // POST: FoodTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodTypesDataController foodTypesDataController = new FoodTypesDataController();
            System.Web.Http.IHttpActionResult actionResult = foodTypesDataController.DeleteFoodType(Convert.ToInt32(id));
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
