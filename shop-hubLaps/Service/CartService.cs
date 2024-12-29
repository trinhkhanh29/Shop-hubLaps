using Microsoft.EntityFrameworkCore;
using shop_hubLaps.Models;
using System.Collections.Generic;
using System.Linq;

public interface ICartService
{
    void UpdateCart(int productId, int quantity);  // Update cart items (either add new or update existing)
    List<ChiTietDonHang> GetCartItems(string userId);  // Get all cart items for the given user
}

public class CartService : ICartService
{
    private readonly DataModel _context;

    public CartService(DataModel context)
    {
        _context = context;
    }

    // Update cart: Either add new items or update existing ones
    public void UpdateCart(int productId, int quantity)
    {
        // Get user ID from the logged-in user (replace with actual logic if needed)
        var userId = "currentUserId";  // This should be replaced by actual logged-in user ID logic, e.g., User.Identity.Name in a web app

        // Fetch existing cart or create a new one if none exists
        var donHang = _context.DonHangs
            .Include(d => d.ChiTietDonHangs)
            .FirstOrDefault(d => d.makh == userId && d.tinhtrang == "CART");

        if (donHang == null)
        {
            // Create a new order (donHang) if it doesn't exist
            donHang = new DonHang
            {
                makh = userId,
                tinhtrang = "CART",
                ChiTietDonHangs = new List<ChiTietDonHang>()
            };
            _context.DonHangs.Add(donHang);
        }

        // Check if the item already exists in the cart
        var existingItem = donHang.ChiTietDonHangs.FirstOrDefault(c => c.malaptop == productId);

        if (existingItem != null)
        {
            // Update the quantity of the existing item
            existingItem.soluong = quantity;
        }
        else
        {
            // Add new item to the cart
            var newItem = new ChiTietDonHang
            {
                malaptop = productId,
                soluong = quantity,
                dongia = 1000 // You can replace this with actual logic to get the price of the laptop
            };
            donHang.ChiTietDonHangs.Add(newItem);
        }

        // Save changes to the database
        _context.SaveChanges();
    }

    // Get all cart items for a given user
    public List<ChiTietDonHang> GetCartItems(string userId)
    {
        var donHang = _context.DonHangs
            .Include(d => d.ChiTietDonHangs)
            .FirstOrDefault(d => d.makh == userId && d.tinhtrang == "CART");

        return donHang?.ChiTietDonHangs.ToList() ?? new List<ChiTietDonHang>();  // Return empty list if no cart found
    }
}
