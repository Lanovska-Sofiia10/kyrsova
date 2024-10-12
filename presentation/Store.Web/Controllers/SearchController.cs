using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kyrsova;
using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookService bookService;

        public SearchController(BookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Book>());
            }

            var books = bookService.GetAllByQuery(query);

            return View("Index", books);
        }
    }
}

