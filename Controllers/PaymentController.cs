using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ECommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PaymentController(IConfiguration Config)
        {
            _config = Config;
        }
        [HttpPost("create-payment-intent")]
        public ActionResult CratePaymentIntent([FromBody]PaymentReq Req  )
        {
            StripeConfiguration.ApiKey = _config.GetValue<string>("Stripe:SecretKey");
            Console.WriteLine(_config.GetValue<string>("Stripe:SecurityKey"));

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(Req.Amount*100),
                Currency="usd",
                PaymentMethodTypes=new List<string> { "card"}

            };
            var service = new PaymentIntentService();
            var intent = service.Create(options);
            return Ok(new { ClientSecret = intent.ClientSecret });
        }

        public class PaymentReq
        {
            public decimal Amount { get; set; }
        }
    }
}
