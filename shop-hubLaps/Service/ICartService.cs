namespace shop_hubLaps.Service
{
    public interface ICartService
    {
        Cart GetCart(); // Lấy giỏ hàng
        void AddToCart(int productId, int quantity); // Thêm sản phẩm vào giỏ hàng
        void RemoveFromCart(int productId); // Xóa sản phẩm khỏi giỏ hàng
    }

    public class Cart
    {
    }
}
