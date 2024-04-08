﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutfitO.Models;
using OutfitO.Repository;
using OutfitO.ViewModels;
using Stripe;
using System;
using System.Security.Claims;

namespace OutfitO.Controllers
{
    public class CheckOutController : Controller
    {
        ICartRepository _cartRepository;
        IPaymentRepository _paymentRepository;
        UserManager<User> _userManager;
        public CheckOutController(ICartRepository cartRepository, IPaymentRepository paymentRepository)
        {
            _cartRepository = cartRepository;
            _paymentRepository = paymentRepository;
        }

        public IActionResult Index()
        {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Cart"] = _cartRepository.GetForUser(Userid);
            ViewData["User"] = Userid;
            var TPromoPrice = HttpContext.Session.GetString("TPromoPrice");
            // Retrieve the price from TempData and parse it back to decimal
            if (TPromoPrice != null && decimal.TryParse(TPromoPrice.ToString(), out decimal price))
            {
                ViewData["Price"] = price;
            }
            else
            {
                ViewData["Price"] = 0; // or any default value you prefer
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ProcessPayment(PaymentViewModel model, string stripeToken)
        {
            try
            {
                // Configure Stripe with your secret key
                StripeConfiguration.ApiKey = "sk_test_51Oxs1dA5xWK61WYU1vOXqBpm4XmvZOmPAlHShIgyusJIej7fMytuXlWsfiApFV2AMwmTseQiyoPJ2mcAaWsSUEF9008TiLucZv";

                // Create a charge using the token generated by the Stripe.js
                var options = new ChargeCreateOptions
                {
                    Amount = (long)(model.Amount * 100), // Amount in cents
                    Currency = "usd",
                    Description = "Payment for " + model.Name,
                    Source = stripeToken,
                };
                var service = new ChargeService();
                var charge = service.Create(options);

                // If payment is successful, save payment information to the database
                if (charge.Status == "succeeded")
                {
                    var payment = new Payment
                    {
                        PaymentId = charge.Id,
                        Name=model.Name,
                        Email=model.Email,
                        Address=model.Address,
                        City=model.City,
                        Zip=model.Zip,
                        Country=model.Country,
                        State=model.State,
                        Phone=model.Phone,
                        UserId= model.UserId,
                        TotalPrice = model.Amount,
                    };

                    _paymentRepository.Insert(payment);
                    _paymentRepository.Save();

					
					HttpContext.Session.SetInt32("Count", 0);
					// Redirect to success view with payment information
					return RedirectToAction("Success", new { paymentId = payment.PaymentId });
                }
                else
                {
                    // If payment fails, return to failure view
                    return RedirectToAction("Failed", new { errorMessage = "Payment failed." });
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during payment processing, return to failure view
                return RedirectToAction("Failed", new { errorMessage = ex.Message });
            }
        }
        [Authorize]
        public IActionResult Success(string paymentId)
        {
            var payment =_paymentRepository.Get(paymentId);
            TempData["Payment"] = payment.Id.ToString();
            TempData["TPrice"] = payment.TotalPrice.ToString();
            return RedirectToAction("Add", "Order");
            //return View(payment);
        }
        [Authorize]
        public IActionResult Failed(string errorMessage)
        {
            try
            {
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error occurred while rendering Failed view: " + ex.Message);
                throw; // Rethrow the exception
            }
        }

    }
}
