using Kyrsova;
using Kyrsova.Messages;
using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IBookRepository bookRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }

            model = null;
            return false;
        }

        internal bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }

            order = null;
            return false;
        }

        internal OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            return bookRepository.GetAllByIds(bookIds);
        }

        public OrderModel AppBook(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add");

            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();
            
            AddOrUpdateBook(order, bookId, count);
            UpdateSession(order);
            

            return Map(order);
        }

        internal void AddOrUpdateBook(Order order, int bookId, int count)
        {
            var book = bookRepository.GetById(bookId);
            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(book.Id, book.Price, count);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set("Cart", cart);
        }

        public OrderModel UpdateBook(int bookId, int count)
        {
            var order = GetOrder();
            order.Items.Get(bookId).Count = count;
            orderRepository.Update(order);
            UpdateSession(order);
            return Map(order);
        }

        public OrderModel RemoveBook(int bookId)
        {
            var order = GetOrder();
            order.Items.Remove(bookId);
            orderRepository.Update(order);
            UpdateSession(order);
            return Map(order);
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
                return order;
            throw new InvalidOperationException("Empty session.");
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111;   // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
            }
            else
            {
                model.Errors["cellPhone"] = "Номер телефону не відповідає формату +380987654321";
            }

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "UА");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);

                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            var model = new OrderModel();

            // Перевірка, чи є в сесії код підтвердження для даного номеру телефону
            var storedCode = Session.GetInt32(cellPhone);
            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Щось сталося. Спробуйте ще раз отримати код.";
                return model;
            }

            // Перевірка на збіг кодів підтвердження
            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неправильний код. Перевірте та спробуйте знову.";
                return model;
            }

            try
            {
                // Отримання замовлення
                var order = GetOrder();
                order.CellPhone = cellPhone;

                // Оновлення замовлення в репозиторії
                orderRepository.Update(order);

                // Видалення коду з сесії
                Session.Remove(cellPhone);

                return Map(order);  // Повернення оновленої моделі замовлення
            }
            catch (Exception ex)
            {
                // Логування помилки (за потреби)
                model.Errors["general"] = "Сталася помилка при підтвердженні телефону. Спробуйте ще раз.";
                return model;
            }
        }


        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);
            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);
            Session.RemoveCart();
            return Map(order);
        }
    }
}
