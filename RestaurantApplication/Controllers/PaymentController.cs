using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Razorpay.Api;
using RestaurantApplication.Models;
using RestaurantApplication.Models.ViewModel;

namespace RestaurantApplication.Controllers
{
    public class PaymentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Payment/Index
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int OrderId)
        {
            //System.Diagnostics.Debug.WriteLine(OrderModel.OrderIDClassDetails.OrderIDNumber);
            //// receive the orderItem having the orderID as the ongoing order
            OrderItem orderItem = db.OrdersItems.Where(o => o.OrderID.OrderIDNumber == OrderId).ToList()[0];

            //System.Diagnostics.Debug.WriteLine(orderItem[0].OrderID.OrderIDNumber);
            //// receive user details of the order if it exists from this order ID
            //// fetching from BookingDB the booking details
            Booking bookingDetails = db.Bookings.Find(orderItem.BookingID);

            string customerName = "";
            string customerEmail = "";
            string customerPhone = "";
            if(bookingDetails != null)
            {
                customerName = bookingDetails.BookingName;
                customerEmail = bookingDetails.EMailID;
                customerPhone = bookingDetails.PhoneNumber;
            }

            PaymentDetail paymentDetail = new PaymentDetail
            {
                name = customerName,
                email = customerEmail,
                contactNumber = customerPhone,
                address = null,
                amount = db.OrderIDs.Find(OrderId).TotalAmount,
            };
            return View(paymentDetail);
            //return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(Models.PaymentDetail _requestData)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Generate random receipt number for order
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_dTBD7wqaumXJ5j", "AOm2UZvORrtH5e8YMT8243ds");
            Dictionary<string, object> options = new Dictionary<string, object>();
            System.Diagnostics.Debug.WriteLine(_requestData.amount * 100);
            System.Diagnostics.Debug.WriteLine(_requestData.amount);
            options.Add("amount", _requestData.amount * 100);  // Amount will in paise
            options.Add("receipt", transactionId);
            options.Add("currency", "USD");
            options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
            //options.Add("notes", "Order from Firehouse Pizzeria");
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();

            // Create order model for return on view
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_dTBD7wqaumXJ5j",
                amount = _requestData.amount * 100,
                currency = "INR",
                name = _requestData.name,
                email = _requestData.email,
                contactNumber = _requestData.contactNumber,
                address = _requestData.address,
                description = "Testing description"
            };

            // Return on PaymentPage with Order data
            return View("PaymentPage", orderModel);
        }

        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public decimal amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string contactNumber { get; set; }
            public string address { get; set; }
            public string description { get; set; }
        }


        [HttpPost]
        public ActionResult Complete()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_dTBD7wqaumXJ5j", "AOm2UZvORrtH5e8YMT8243ds");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }
}