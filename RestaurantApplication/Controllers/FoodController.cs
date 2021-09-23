﻿using System;
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
    // Food controller is mainly user facing controller, FoodsController is for the Admin
    // This will be combined at a later stage
    public class FoodController : Controller
    {
        //private static readonly HttpClient client;
        //private JavaScriptSerializer jss = new JavaScriptSerializer();
        private readonly Client client = new Client();

        // GET: Food/List
        // List is shown when a user directly clicks on the Menu link in th navbar
        public ActionResult List()
        {
            //retrieve the list of food items

            // HttpClient Implementation
            //string url = "FoodData/ListFoods";
            //var resp = client.ExecuteGet(url);
            //IEnumerable<FoodDto> foods = resp.ReadAsAsync<IEnumerable<FoodDto>>().Result;

            //Datacontroller implementation
            FoodDataController foodDataController = new FoodDataController();
            IEnumerable<FoodDto> foods = foodDataController.ListFoods();

            return View(foods);
        }

        // Post : Food/ListNew
        // This function is executed when a client books the table with their credentials
        // Once the booking is confirmed a button to order the food will be shown in the booking status page
        // On clicking the button, the booking ID is also passed to this view as a hidden form so that the
        // payment page can be auto-filled by the system.
        [HttpPost]
        public ActionResult ListNew(int bookingId)
        {
            // retrieve the list of food items

            // HttpClient Implementation
            //string url = "FoodData/ListFoods";
            //var resp = client.ExecuteGet(url);
            //IEnumerable<FoodDto> foods = resp.ReadAsAsync<IEnumerable<FoodDto>>().Result;

            // datacontroller implementation
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
        // Not implemented in the Views yet, this is for the customer to see a detailed description of the food item
        // Might include this at a later stage
        public ActionResult Details(int id)
        {
            // HttpClient Implementation
            string url = "FoodData/FindFood/" + id;
            var resp = client.ExecuteGet(url);
            FoodDto foods = resp.ReadAsAsync<FoodDto>().Result;
            return View(foods);
        }

    }
}
