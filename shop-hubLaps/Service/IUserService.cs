using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Areas.Identity.Data;
using shop_hubLaps.Models;

namespace shop_hubLaps.Service
{
    public interface IUserService
    {
        Task<SampleUser> GetUserByMakhAsync(string makh);
    }

    public class UserService : IUserService
    {
        private readonly DataModel _context;

        public UserService(DataModel context)
        {
            _context = context;
        }

        public async Task<SampleUser> GetUserByMakhAsync(string makh)
        {
            return await _context.SampleUsers.FirstOrDefaultAsync(u => u.Id == makh); // So sánh với thuộc tính Id
        }
    }
}
