using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using Store.Memory;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly INotificationService notificationService;
        private readonly IOrderRepository orderRepository;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!HttpContext.Session.TryGetCart(out Cart cart) || cart == null)
            {
                var order = orderRepository.Create();
                cart = new Cart(order.Id);
                HttpContext.Session.Set("Cart", cart);
                OrderModel model = Map(order);
                return View(model);
            }

            var existingOrder = orderRepository.GetById(cart.OrderId) ?? orderRepository.Create();
            OrderModel existingModel = Map(existingOrder);
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

        [HttpPost]
        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(bookId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }



        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))

        [HttpPost]
        public IActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Номер телефону не відповідає";
                return View("Index", model);
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

        [HttpPost]
        public IActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Номер телефону не відповідає";
                return View("Index", model);
            }

            int code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);

      
            Console.WriteLine($"Імітація відправки коду {code} на номер: {cellPhone}");

            return View("Confirmation", new ConfirmationModel
            {
                OrderId = id,
                CellPhone = cellPhone
            });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (string.IsNullOrWhiteSpace(cellPhone))
                return false;

            cellPhone = cellPhone.Replace(" ", "").Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+380\d{9}$");
        }


            int code = 1111;
            HttpContext.Session.SetInt32(cellPhone, code);

      
            Console.WriteLine($"Імітація відправки коду {code} на номер: {cellPhone}");

            return View("Confirmation", new ConfirmationModel
            {
                OrderId = id,
                CellPhone = cellPhone
            });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (string.IsNullOrWhiteSpace(cellPhone))
                return false;

            cellPhone = cellPhone.Replace(" ", "").Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+380\d{9}$");
        }


        [HttpPost]
        public IActionResult StartDelivery(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);

            if (storedCode == null)
            {
                return View("Confirmation", new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string> { { "code", "Код отсутствует, повторите отправку." } }
                });
            }

            if (storedCode != code)
            {
                return View("Confirmation", new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string> { { "code", "Неправильний код підтвердження" } }
                });
            }

            return RedirectToAction("DeliveryStarted", new { id = id });
        }
    }
}
