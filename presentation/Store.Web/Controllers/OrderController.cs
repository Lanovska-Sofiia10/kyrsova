﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Kyrsova;
using Store.Web.Models;
using Store.Memory;
using Microsoft.AspNetCore.Http;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.TryGetCart(out Cart cart) || cart == null)
            {
                Console.WriteLine("Cart is NOT found in session.");
                var order = orderRepository.Create();
                cart = new Cart(order.Id);
                HttpContext.Session.Set("Cart", cart);

                OrderModel model = Map(order);
                return View(model);
            }

            var existingOrder = orderRepository.GetById(cart.OrderId) ?? orderRepository.Create();
            OrderModel existingModel = Map(existingOrder);

            Console.WriteLine("Cart is retrieved from session with OrderId: " + cart.OrderId);
            return View(existingModel);
        }

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count = item.Count,
                             };

            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
            };
        }

        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var book = bookRepository.GetById(bookId);

            order.AddOrUpdateItem(book, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int change)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var item = order.GetItem(bookId);
            if (item != null)
            {
                int newCount = item.Count + change;

                // Перевіряємо нове значення на допустимість
                if (newCount > 0)
                {
                    item.Count = newCount;
                }
                else
                {
                    return BadRequest("Count must be greater than zero.");
                }
            }

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }


        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            return (order, cart);
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            
            HttpContext.Session.Set("Cart", cart);
        }

        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            
            order.RemoveItem(bookId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

    }
}



