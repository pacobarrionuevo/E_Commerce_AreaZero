using E_Commerce_VS.Services;
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
            public ActionResult Create(int id) // Se recibe un parámetro ID desde la solicitud
            {
                var domain = "http://localhost:4242";

            // Obtener el Price ID usando el método Precio
            string priceId = "price_1QPLgTKyA4zEE9fSKNnk7HjR";

                // Verificar si el Price ID es válido
                if (priceId == "price_not_found")
                {
                    return BadRequest(new { error = "Invalid product ID" });
                }

                var options = new SessionCreateOptions
                {
                    UiMode = "embedded",
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            // Se utiliza el Price ID obtenido dinámicamente
                            Price = "price_1QPMePKyA4zEE9fSh1yN88nj",
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
