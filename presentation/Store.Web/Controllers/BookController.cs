using Kyrsova;
using Microsoft.AspNetCore.Mvc;
using Store.Memory;

namespace Store.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IActionResult Index(int id)
        {
            var book = bookRepository.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}

