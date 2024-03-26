using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;

namespace OutfitO.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            // Populate list of products
            List<productTest> productTests = new List<productTest>
            {
                new productTest
                {
                    Id = 1,
                    Title = "Shirt",
                    Description = "This is a Shirt",
                    Img = "images/image1.png",
                    Price = 20.00,
                    Stock = 10,
                    Quantity = 2
                },
                new productTest
                {
                    Id = 2,
                    Title = "Pants",
                    Description = "This is a Pants",
                    Img = "images/image2.png",
                    Price = 30.00,
                    Stock = 20,
                    Quantity = 1
                }
            };
            return View(productTests);
        }

        public IActionResult OrderConfirmation()
        {
            // Check if TempData contains a valid session ID
            if (TempData["sessionId"] != null && !string.IsNullOrEmpty(TempData["sessionId"].ToString()))
            {
                // Retrieve the session ID from TempData
                var sessionId = TempData["sessionId"].ToString();

                // Create an instance of SessionService
                var service = new SessionService();

                try
                {
                    // Retrieve the session using the provided session ID
                    var session = service.Get(sessionId);

                    // Check if the session exists and if payment status is "paid"
                    if (session != null && session.PaymentStatus == "paid")
                    {
                        var transaction = session.PaymentIntentId.ToString(); 
                        // Return the Success view without passing any model
                        return View("Success");
                    }
                }
                catch (StripeException ex)
                {
                    // Handle any Stripe exceptions here
                    // Log or display an error message
                    Console.WriteLine("StripeException: " + ex.Message);
                }
            }

            // If session ID is invalid or payment status is not "paid", redirect to the Login view
            return View("Login");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            List<productTest> productTests = new List<productTest>
            {
                new productTest
                {
                    Id = 1,
                    Title = "Shirt",
                    Description = "This is a Shirt",
                    Img = "images/image1.png",
                    Price = 20.00,
                    Stock = 10,
                    Quantity = 2
                },
                new productTest
                {
                    Id = 2,
                    Title = "Pants",
                    Description = "This is a Pants",
                    Img = "images/image2.png",
                    Price = 30.00,
                    Stock = 20,
                    Quantity = 1
                }
            };

            var domain = "http://localhost:45071/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Login",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in productTests)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Title,
                            Description = item.Description,
                            Images = new List<string> { item.Img }
                        },
                        UnitAmountDecimal = (long)(item.Price * item.Quantity),
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Store session ID in TempData
            TempData["sessionId"] = session.Id;

            return Redirect(session.Url);
        }
    }
}
