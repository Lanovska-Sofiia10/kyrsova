using Kyrsova;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Memory
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> orders = new List<Order>();

        public Order Create()
        {
            int nextId = orders.Count + 1;
            var order = new Order(nextId, new OrderItem[0]);

            orders.Add(order);

            return order;
        }

        public Order GetById(int id)
        {
            return orders.SingleOrDefault(order => order.Id == id);
        }

        public void Update(Order order)
        {
            var existingOrder = GetById(order.Id);
            if (existingOrder != null)
            {
                orders.Remove(existingOrder);
                orders.Add(order);
            }
        }
    }
}
