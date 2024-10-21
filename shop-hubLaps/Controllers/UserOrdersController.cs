using Microsoft.AspNetCore.Mvc;
using shop_hubLaps.Service;
using System.Threading.Tasks;

namespace shop_hubLaps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserOrdersController : ControllerBase
    {
        private readonly UserOrderService _userOrderService;
        private readonly IUserService _userService;

        public UserOrdersController(UserOrderService userOrderService, IUserService userService)
        {
            _userOrderService = userOrderService;
            _userService = userService;
        }

        [HttpGet("{makh}")]
        public async Task<IActionResult> GetUserOrders(string makh)
        {
            var user = await _userService.GetUserByMakhAsync(makh);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var orders = await _userOrderService.GetOrdersByUserMakhAsync(makh); // Thêm phương thức này trong UserOrderService
            return Ok(new { user, orders });
        }
    }
}
