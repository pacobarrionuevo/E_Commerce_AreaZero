using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce_VS.Controllers
{
    //Controla que el estado de la sesion
    [Route("session-status")]
    [ApiController]
    public class SessionStatusController : Controller
    {
        [HttpGet]
        public ActionResult SessionStatus([FromQuery] string session_id)
        {
            var sessionService = new SessionService();
            Session session = sessionService.Get(session_id);

            return Json(new { status = session.Status, customer_email = session.CustomerDetails.Email });
        }
    }
}
