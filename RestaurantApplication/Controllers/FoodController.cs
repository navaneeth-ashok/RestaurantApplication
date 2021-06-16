using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using RestaurantApplication.Models;
using RestaurantApplication.Models.ViewModel;
using System.Web.Script.Serialization;
namespace RestaurantApplication.Controllers
{
    public class FoodController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static FoodController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44329/api/");
        }
        // GET: Food
        // Older code, will throw it out soon
        public ActionResult List()
        {
            //retrieve the list of food items
            //string url = "FoodData/ListFoods";
            //HttpResponseMessage response = client.GetAsync(url).Result;

            //IEnumerable<FoodDto> foods = response.Content.ReadAsAsync<IEnumerable<FoodDto>>().Result;
            FoodDataController foodDataController = new FoodDataController();
            IEnumerable<FoodDto> foods = foodDataController.ListFoods();
            return View(foods);
        }

        [HttpPost]
        public ActionResult ListNew(int bookingId)
        {
            // retrieve the list of food items
            //string url = "FoodData/ListFoods";
            //HttpResponseMessage response = client.GetAsync(url).Result;

            //IEnumerable<FoodDto> foods = response.Content.ReadAsAsync<IEnumerable<FoodDto>>().Result;

            FoodDataController foodDataController = new FoodDataController();
            IEnumerable<FoodDto> foods = foodDataController.ListFoods();

            BookingIDFoodMenu foodsMenu = new BookingIDFoodMenu
            {
                Food = foods,
                BookingID = (int)bookingId
            };

            return View(foodsMenu);
        }

        // GET: Food/Details/5
        public ActionResult Details(int id)
        {
            string url = "FoodData/FindFood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            FoodDto foods = response.Content.ReadAsAsync<FoodDto>().Result;
            return View(foods);
        }

        // GET: Food/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Food/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Food/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Food/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Food/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Food/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
