﻿using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System.Text.RegularExpressions;
using Kyrsova.Messages;
using Kyrsova.Contractors;
using Store.Web.Contractors;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly INotificationService notificationService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        private readonly IOrderRepository orderRepository;

        public OrderController(IBookRepository bookRepository,
                               IOrderRepository orderRepository,
                               INotificationService notificationService,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IWebContractorService> webContractorServices,
                               IEnumerable<IPaymentService> paymentServices)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
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

            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(bookId, book.Price, count);

            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int change)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            var item = order.Items.Get(bookId);

            if (item != null && item.Count + change > 0)
            {
                item.Count += change;
            }
            else
            {
                return BadRequest("Кількість товару повинна бути більшою за нуль.");
            }

            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Order");
        }


        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId) ?? orderRepository.Create();
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
                HttpContext.Session.Set("Cart", cart);
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
            order.Items.Remove(bookId);
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

            var order = orderRepository.GetById(id);
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode, service => service.Title)
            };

            return View("DeliveryMethod", model);
        }

        [HttpPost]
        public ActionResult StartDelivery(int id, string uniqueCode)
        {
            var deliveryService = deliveryServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (deliveryService == null)
                return BadRequest("Invalid delivery service.");

            var order = orderRepository.GetById(id);
            var form = deliveryService.CreateForm(order);
            return View("DeliveryStep", form);
        }

        [HttpPost]
        public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (deliveryService == null)
                return BadRequest("Invalid delivery service.");

            var form = deliveryService.MoveNextForm(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Delivery = deliveryService.GetDelivery(form);
                orderRepository.Update(order);

                var model = new DeliveryModel
                {
                    OrderId = id,
                    Methods = paymentServices.ToDictionary(service => service.UniqueCode, service => service.Title)
                };

                return View("PaymentMethod", model);
            }

            return View("DeliveryStep", form);
        }

        [HttpPost]
        public ActionResult StartPayment(int id, string uniqueCode)
        {
            var paymentService = paymentServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (paymentService == null)
                return BadRequest("Invalid payment service.");

            var order = orderRepository.GetById(id);
            var form = paymentService.CreateForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (webContractorService != null)
                return Redirect(webContractorService.GetUri);

            return View("PaymentStep", form);
        }

        [HttpPost]
        public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
            if (paymentService == null)
                return BadRequest("Invalid payment service.");

            var form = paymentService.MoveNextForm(id, step, values);

            if (form.IsFinal)
            {
                var order = orderRepository.GetById(id);
                order.Payment = paymentService.GetPayment(form);
                orderRepository.Update(order);

                return View("Finish");
            }

            return View("PaymentStep", form);
        }

        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart();

            return View();
        }
    }
}

