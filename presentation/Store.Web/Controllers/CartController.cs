using Microsoft.AspNetCore.Mvc;
using System;
using Kyrsova;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IBookRepository bookRepository;

        public CartController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public ActionResult Add(int id, Cart cart)
        {
            var book = bookRepository.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!HttpContext.Session.TryGetCart(out cart) || cart == null)
            {
                cart = new Cart();
            }

            if (cart.Items.ContainsKey(id))
            {
                cart.Items[id]++;  // Збільшуємо кількість товару
            }
            else
            {
                cart.Items[id] = 1;  // Якщо товар не в корзині, додаємо його
            }

            cart.Amount += book.Price;  // Збільшуємо загальну суму

            // Зберігаємо корзину в сесії
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Book", new { id });
        }

    }
}

