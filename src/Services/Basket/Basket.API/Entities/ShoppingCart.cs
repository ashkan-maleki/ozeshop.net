namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string? UserName { get; set; }
        public List<ShoppingCartItem>? Items { get; set; }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public ShoppingCart()
        {
            
        }

        public decimal? TotalPrice =>
            Items?
                .Sum(i => i.Price * i.Quantity)
            ?? 0;
    }
}
