using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Interfaces;
using MovieStreamingService.Application.Services;
using MovieStreamingService.Domain.Models;
using MovieStreamingService.WebApi.Controllers.Models;

namespace MovieStreamingService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly PaymentService _paymentService;

        public PaymentController(ISubscriptionService subscriptionService, IUserSubscriptionService userSubscriptionService, PaymentService paymentService)
        {
            _subscriptionService = subscriptionService;
            _userSubscriptionService = userSubscriptionService;
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("pay")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Pay(int subscriptionId, int months, string googlePayToken)
        {
            var id = User.FindFirst("userId");
            if (id == null)
                return Unauthorized("Token is required");

            var subscription = _subscriptionService.GetByIdAsync(subscriptionId).Result;
            if (subscription == null)
                return NotFound("Subscription not found");

            if (months < 1)
                return BadRequest("Invalid months count");

            if (!_paymentService.ProcessPayment(subscription, months, googlePayToken).Result)
                return BadRequest("Payment failed");

            _userSubscriptionService.AddAsync(new UserSubscription
            {
                UserId = Guid.Parse(id.Value),
                SubscriptionId = subscriptionId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(months)
            });
            return Ok();
        }
    }
}
