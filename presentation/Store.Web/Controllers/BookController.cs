using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Memory;
using Store.Web.App;

namespace Store.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService bookService;

        public BookController(BookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index(int id)
        {
            var model = bookService.GetById(id);
            return View(model);
        }

        public IActionResult Catalog()
        {
            var books = bookService.GetAllBooks();
            Console.WriteLine($"Кількість книг у моделі: {books.Count}");
            return View("~/Views/Order/Catalog.cshtml", books); // Передача моделі у View
        }

    }

}

