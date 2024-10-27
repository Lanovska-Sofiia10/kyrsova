using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsova
{
    public class OrderItem
    {
        public int BookId { get; }
        public int Count { get; set; } // Змінити на змінну
        public decimal Price { get; }

        public OrderItem(int bookId, int count, decimal price)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

            BookId = bookId;
            Count = count;
            Price = price;
        }
    }

}
