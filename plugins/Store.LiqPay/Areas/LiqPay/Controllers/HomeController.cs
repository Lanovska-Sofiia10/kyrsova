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

        public IActionResult Callback()
        {
            return View();
        }
    }
}
