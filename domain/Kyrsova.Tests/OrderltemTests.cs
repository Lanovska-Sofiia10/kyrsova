﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsova.Tests
{
    public class OrderltemTests
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                new OrderItem(1, 0m, count);
            });
        }

        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, 0m, count);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var orderItem = new OrderItem(1, 3m, 2);
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2, orderItem.Count);
            Assert.Equal(3m, orderItem.Price);
           
        }

        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOfRangeException()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = -1;
            });
        }

        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOfRangeException()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                orderItem.Count = 0;
            });
        }

        [Fact]
        public void Count_WithPositiveValue_SetsValus()
        {
            var orderItem = new OrderItem(0, 0m, 5);

            orderItem.Count = 10;    

            Assert.Equal(10, orderItem.Count);
        }
    }
}
