using Kyrsova;

namespace Store.Web.Models
{
    public class Cart
    {
        public int OrderId { get; }

        public int TotalCount { get; }

        public decimal TotalPrice { get; }

        public Cart()
        {
            OrderId = 0;
            TotalCount = 0;
            TotalPrice = 0m;
        }

        public Cart(int orderId, int totalCount, decimal totalPrice)
        {
            OrderId = orderId;
            TotalCount = totalCount;
            TotalPrice = totalPrice;
        }
    }
}
