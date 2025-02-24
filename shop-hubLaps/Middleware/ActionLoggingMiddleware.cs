using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace shop_hubLaps.Middleware
{
    public class ActionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ActionLoggingMiddleware> _logger;

        public ActionLoggingMiddleware(RequestDelegate next, ILogger<ActionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";

            // Trích xuất UserId từ claims nếu người dùng đã xác thực
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : "Anonymous";

            var path = context.Request.Path;
            var method = context.Request.Method;
            var queryString = context.Request.QueryString.ToString();
            var timestamp = DateTime.UtcNow;

            // Trích xuất role của người dùng
            var roles = context.User.Identity?.IsAuthenticated == true
                ? string.Join(", ", context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value))
                : "None";

            // Log thông tin yêu cầu trước khi xử lý
            _logger.LogInformation("Request started - Roles: {Roles}, User: {User}, UserID: {UserId}, Path: {Path}, Method: {Method}, QueryString: {QueryString}, Timestamp: {Timestamp}",
                roles, user, userId, path, method, queryString, timestamp);

            if (method == HttpMethods.Post)
            {
                context.Request.EnableBuffering();
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                _logger.LogInformation("Request Body: {RequestBody}", requestBody);
            }

            await _next(context);

            _logger.LogInformation("Request completed - User: {User}, Path: {Path}, Method: {Method}, QueryString: {QueryString}, Timestamp: {Timestamp}",
                user, path, method, queryString, timestamp);
        }
    }
}
