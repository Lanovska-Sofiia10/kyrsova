﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsova
{
    public class OrderItem
    {
        public int BookId { get; }

        private int count;

        public int Count { 
            get{ return count; }
            set
            {
                ThrowIfInvalidCount(value);

                count = value;
            }
        }
        
        public decimal Price { get; }

        public OrderItem(int bookId, decimal price, int count)
        {
            ThrowIfInvalidCount(count);

            BookId = bookId;
            Count = count;
            Price = price;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

        }
    }

}
