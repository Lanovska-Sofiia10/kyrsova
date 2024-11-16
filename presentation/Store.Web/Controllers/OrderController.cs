using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System.Text.RegularExpressions;
using Kyrsova.Contractors;
using Store.Web.Contractors;
using Store.Web.App;
using System.Net;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService orderService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;

        public OrderController(OrderService orderService,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IWebContractorService> webContractorServices,
                               IEnumerable<IPaymentService> paymentServices)
        {
            this.orderService = orderService;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (orderService.TryGetModel(out OrderModel model))
                return View(model);

            return View("Empty");
        }

        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1)
        {
            orderService.AppBook(bookId, count);
            return RedirectToAction("Index", "Book", new { id = bookId });
        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            var model = orderService.UpdateBook(bookId, count);
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult RemoveItem(int bookId)
        {
            var model = orderService.RemoveBook(bookId);
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult SendConfirmation(string cellPhone)
        {
            var model = orderService.SendConfirmation(cellPhone);
            return View("Confirmation", model);
        }

        [HttpPost]
        public IActionResult Confirmate(string cellPhone, int confirmationCode)
        {
            var model = orderService.ConfirmCellPhone(cellPhone, confirmationCode);

            if (model.Errors.Count > 0)
                return View("Confirmation", model);

            var deliveryMethods = deliveryServices.ToDictionary(service => service.Name, service => service.Title);
            return View("DeliveryMethod", deliveryMethods);
        }

        [HttpPost]
        public ActionResult StartDelivery(string serviceName)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = deliveryService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == serviceName);
            if (webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(NextDelivery));
            var parameters = form.Parameters.ToDictionary(field => field.Key, field => (object)field.Value);
            var redirectUri = webContractorService.StartSession(parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query = null,
            };
            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;
            return builder.Uri;
        }

        [HttpPost]
        public IActionResult NextDelivery(string serviceName, int step, Dictionary<string, string> values)
        {
            // Отримання сервісу доставки
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);

            // Перевірка на коректність форми
            var form = deliveryService.NextForm(step, values);
            if (form == null)
            {
                // Обробка помилки, якщо форма не знайдена
                return View("Error", "Неможливо отримати форму доставки.");
            }

            // Перевірка, чи є кінцевий крок
            if (!form.IsFinal)
            {
                return View("DeliveryStep", form);
            }

            // Отримання доставки через GetDelivery
            var delivery = deliveryService.GetDelivery(form);
            if (delivery == null)
            {
                // Обробка помилки, якщо не вдалося отримати доставку
                return View("Error", "Неможливо отримати інформацію про доставку.");
            }

            // Встановлення доставки в замовлення
            orderService.SetDelivery(delivery);

            // Отримання методів оплати
            var paymentMethods = paymentServices.ToDictionary(service => service.Name, service => service.Title);
            return View("PaymentMethod", paymentMethods);
        }


        [HttpPost]
        public ActionResult StartPayment(string serviceName)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == serviceName);
            if (webContractorService == null)
                return View("PaymentStep", form);

            var returnUri = GetReturnUri(nameof(NextPayment));
            var parameters = form.Parameters.ToDictionary(field => field.Key, field => (object)field.Value);
            var redirectUri = webContractorService.StartSession(parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        [HttpPost]
        public ActionResult NextPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var form = paymentService.NextForm(step, values);

            if (!form.IsFinal)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("Finish", model);
        }
    }
}