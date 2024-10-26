using Kyrsova;

namespace Store.Web.Models
{
    public class Cart
    {
        public int OrderId { get; set; }  // Зроблено set доступним для параметрless конструктора

        public int TotalCount { get; set; }

        public decimal TotalPrice { get; set; }

        public Cart()
        {
            OrderId = 0;
            TotalCount = 0;
            TotalPrice = 0m;
        }

        public Cart(int orderId)
        {
            OrderId = orderId;
            TotalCount = 0;
            TotalPrice = 0m;
        }
    }
}
