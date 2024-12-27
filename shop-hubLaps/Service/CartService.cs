//using Microsoft.AspNetCore.Cors.Infrastructure;

//namespace shop_hubLaps.Service
//{
//    public class CartService : ICartService
//    {
//        private List<CartItem> _cartItems = new List<CartItem>(); // Giả sử đây là danh sách giỏ hàng lưu trong bộ nhớ

//        public Cart GetCart()
//        {
//            return new Cart { Items = _cartItems };
//        }

//        public void AddToCart(int productId, int quantity)
//        {
//            var item = new CartItem { ProductId = productId, Quantity = quantity };
//            _cartItems.Add(item);
//        }

//        public void RemoveFromCart(int productId)
//        {
//            var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);
//            if (item != null)
//            {
//                _cartItems.Remove(item);
//            }
//        }
//    }

//}