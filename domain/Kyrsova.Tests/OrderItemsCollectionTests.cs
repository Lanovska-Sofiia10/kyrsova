using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kyrsova.Tests
{
    public class OrderItemsCollectionTests
    {
        [Fact]
        public void GetItem_WithExistingItem_ReturnsItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Get(100);
            });
        }

        [Fact]
        public void GetItem_WithNoExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Get(100);
            });
        }

        [Fact]
        public void Add_WithExistingItem_ThrowInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            var book = new Book(1, null, null, null, null, 0m);

            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(1, 10m, 10);
            });
        }

        [Fact]
        public void Add_WithNewItem_SetsCount()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            var book = new Book(4, null, null, null, null, 0m);

            order.Items.Add(4, 30m, 10);

            Assert.Equal(10, order.Items.Get(4).Count);
        }

        [Fact]
        public void RemoveItem_WithExistingItem_RemovesItem()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            order.Items.Remove(1);

            Assert.Collection(order.Items,
                              item => Assert.Equal(2, item.BookId));
                      

        }

        [Fact]
        public void RemoveItem_WithNoExistingItem_ThrowsInvalidOperationException()
        {
            var order = new Order(1, new[]
            {
                new OrderItem(1,10m, 3),
                new OrderItem(2,100m, 5),
            });

            Assert.Throws<InvalidOperationException>(() => {
                order.Items.Remove(100);
            });

        }
    }
}
