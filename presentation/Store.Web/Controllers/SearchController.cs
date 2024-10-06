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
        private readonly IBookRepository bookRepository;

        public SearchController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public IActionResult Index(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Book>());
            }

            var books = bookRepository.GetAllByTitle(query);

            return View(books);
        }
    }
}

