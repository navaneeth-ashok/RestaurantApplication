using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        // This controller is to serve a one stop view to all the restaurant management actions
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}