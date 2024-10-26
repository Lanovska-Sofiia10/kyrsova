using Microsoft.AspNetCore.Mvc;
using System;
using Kyrsova;
using Store.Web.Models;
using Store.Memory;

namespace Store.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;

        public CartController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }

        public ActionResult Add(int id, Cart cart)
        {
            var book = bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            Order order;
            if (!HttpContext.Session.TryGetCart(out cart) || cart == null)
            {
                order = orderRepository.GetById(cart?.OrderId ?? 0) ?? orderRepository.Create();
                cart = new Cart(order.Id);
            }
            else
            {
                order = orderRepository.GetById(cart.OrderId);
            }

            order.AddItem(book, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Book", new { id });
        }
    }
}


