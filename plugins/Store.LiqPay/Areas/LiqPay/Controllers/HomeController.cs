using Microsoft.AspNetCore.Mvc;

namespace Store.LiqPay.Areas.LiqPay.Controllers
{
    [Area("LiqPay")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessPayment(string cardNumber, string expiryDate, string cvv)
        {
            // Перевірка вхідних даних
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Replace(" ", "").Length != 16)
            {
                ModelState.AddModelError(string.Empty, "Номер картки має складатися з 16 цифр.");
                ViewBag.CardNumber = cardNumber; // Повертаємо номер картки
                ViewBag.ExpiryDate = expiryDate;
                ViewBag.CVV = cvv;
                return View("Index");
            }

            if (string.IsNullOrWhiteSpace(expiryDate) || !expiryDate.Contains("/") || expiryDate.Length != 5)
            {
                ModelState.AddModelError(string.Empty, "Термін дії має бути у форматі MM/YY.");
                ViewBag.CardNumber = cardNumber;
                ViewBag.ExpiryDate = expiryDate; // Повертаємо термін дії
                ViewBag.CVV = cvv;
                return View("Index");
            }

            if (string.IsNullOrWhiteSpace(cvv) || cvv.Length != 3)
            {
                ModelState.AddModelError(string.Empty, "CVV має складатися з 3 цифр.");
                ViewBag.CardNumber = cardNumber;
                ViewBag.ExpiryDate = expiryDate;
                ViewBag.CVV = cvv; // Повертаємо CVV
                return View("Index");
            }

            // Якщо дані вірні
            ViewBag.Message = $"Платіж успішно оброблено для картки {cardNumber.Substring(0, 4)} **** **** ****";
            return View("Callback");
        }


        public IActionResult Callback()
        {
            return View();
        }
    }
}
