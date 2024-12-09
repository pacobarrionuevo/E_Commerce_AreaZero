using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Collections.Generic;

namespace E_Commerce_VS.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        [HttpPost]
        public ActionResult Create()
        {
            var domain = "http://localhost:4242";
            var options = new SessionCreateOptions
            {
                UiMode = "embedded",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                        Price = "price_1QPMesKyA4zEE9fSrBjDTeBs",
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                ReturnUrl = domain + "/return.html?session_id={CHECKOUT_SESSION_ID}",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { clientSecret = session.ClientSecret });
        }
    }
}
