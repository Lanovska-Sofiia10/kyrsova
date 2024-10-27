using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Kyrsova;
using Store.Web.Models;
using Store.Memory;

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

        [HttpPost]
        public IActionResult AddItem(int id)
        {
            Order order;
            if (!HttpContext.Session.TryGetCart(out Cart cart) || cart == null)
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
                HttpContext.Session.Set("Cart", cart);
            }
            else
            {
                order = orderRepository.GetById(cart.OrderId);
            }

            var book = bookRepository.GetById(id);
            if (book != null)
            {
                order.AddItem(book, 1);
                orderRepository.Update(order);

                cart.TotalCount = order.TotalCount;
                cart.TotalPrice = order.TotalPrice;
                HttpContext.Session.Set("Cart", cart);
            }

            // Повертаємося на сторінку книги
            return RedirectToAction("Index", "Book", new { id });
        }

    }
}



