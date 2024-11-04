using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Kyrsova.Messages;
using Kyrsova.Contractors;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly INotificationService notificationService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IOrderRepository orderRepository;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService, IEnumerable<IDeliveryService> deliveryServices)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.deliveryServices = deliveryServices;
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
                return View(Map(order));
            }

            var existingOrder = orderRepository.GetById(cart.OrderId) ?? orderRepository.Create();
            return View(Map(existingOrder));
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
            if (item != null && item.Count + change > 0)
            {
                item.Count += change;
            }
            else
            {
                return BadRequest("Count must be greater than zero.");
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

        [HttpPost]
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

        [HttpPost]
        public IActionResult Confirmate(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null || storedCode != code)
            {
                return View("Confirmation", new ConfirmationModel
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string> { { "code", storedCode == null ? "Код отсутствует, повторите отправку." : "Неправильний код підтвердження" } }
                });
            }

            HttpContext.Session.Remove(cellPhone);
            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode, service => service.Title)
            };

            return View("DeliveryMethod", model);
        }

        [HttpGet]
        public ActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var order = orderRepository.GetById(id);
            var form = deliveryService.CreateForm(order);
            return View("DeliveryStep", form);
        }


        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var form = deliveryService.MoveNext(id, step, values);

            // Перевіримо, чи є крок фінальним
            if (form.IsFinal)
            {
                // Повертаємо підтвердження успішної доставки або інший фінальний вигляд
                return RedirectToAction("DeliveryCompleted", new { orderId = id });
            }

            // В іншому випадку повертаємо форму для наступного кроку
            return View("DeliveryStep", form);
        }

    }
}
