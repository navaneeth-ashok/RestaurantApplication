using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using RestaurantApplication.Models;
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
        public ActionResult List()
        {
            // retrieve the list of food items
            string url = "FoodData/ListFoods";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<FoodDto> foods = response.Content.ReadAsAsync<IEnumerable<FoodDto>>().Result;
            return View(foods);
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Food/Create
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Food/Edit/5
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Food/Delete/5
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
