using System.Security.Claims;

namespace ECommerceBackend.MiddleWare
{
    public class UserIdMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserIdMiddlware> _logger;
        public UserIdMiddlware(RequestDelegate next, ILogger<UserIdMiddlware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                _logger.LogInformation($"User ID: {userId}");
                context.Items["UserId"] = userId;
            }
            else
            {
                _logger.LogWarning("User ID not found in claims.");
            }
            await _next(context);
        }

    }
}