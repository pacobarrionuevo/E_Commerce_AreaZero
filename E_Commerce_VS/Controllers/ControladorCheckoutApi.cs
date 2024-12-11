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


            // Método Precio
            [HttpGet("precio/{id}")]
            public string Precio(int id)
            {
                switch (id)
                {
                    case 1: return "price_1QPLgTKyA4zEE9fSKNnk7HjR";
                    case 2: return "price_1QPM0EKyA4zEE9fSbd6v50Pu";
                    case 3: return "price_1QPM8bKyA4zEE9fSo864AQlO";
                    case 4: return "price_1QPMEYKyA4zEE9fS3RlPbKyj";
                    case 5: return "price_1QPMFPKyA4zEE9fSePpdv7Cm";
                    case 6: return "price_1QPMH9KyA4zEE9fSsIJxVv9J";
                    case 7: return "price_1QPMHqKyA4zEE9fSkEnzcTJg";
                    case 8: return "price_1QPMIfKyA4zEE9fSlcNA6a4C";
                    case 9: return "price_1QPMJQKyA4zEE9fSFM2k0Cp4";
                    case 10: return "price_1QPMLPKyA4zEE9fSCvFKg4aZ";
                    case 11: return "price_1QPMMiKyA4zEE9fShN4JeCWt";
                    case 12: return "price_1QPMYsKyA4zEE9fS9Rrp4awA";
                    case 13: return "price_1QPMZYKyA4zEE9fSGRB0cGTK";
                    case 14: return "price_1QPMbyKyA4zEE9fSVlNg1ySm";
                    case 15: return "price_1QPMcZKyA4zEE9fSZYPvb87A";
                    case 16: return "price_1QPMczKyA4zEE9fS5PZseMSL";
                    case 17: return "price_1QPMdQKyA4zEE9fSvXY3kYV4";
                    case 18: return "price_1QPMdtKyA4zEE9fSSo4vsGzM";
                    case 19: return "price_1QPMePKyA4zEE9fSh1yN88nj";
                    case 20: return "price_1QPMesKyA4zEE9fSrBjDTeBs";
                    default: return "price_not_found";
                }
            }
   

      
    }
    }
