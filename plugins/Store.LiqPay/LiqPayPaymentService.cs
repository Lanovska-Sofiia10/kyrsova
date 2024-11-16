using Kyrsova;
using Kyrsova.Contractors;
using Microsoft.AspNetCore.Http;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;

namespace Store.LiqPay
{
    public class LiqPayPaymentService : IPaymentService, IWebContractorService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LiqPayPaymentService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private HttpRequest Request => httpContextAccessor.HttpContext.Request;

        public string Name => "LiqPay";
        public string Title => "Оплата банківською карткою";

        // Реалізуємо UniqueCode для інтерфейсу IWebContractorService
        public string UniqueCode => "LiqPay";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParameter("orderId", order.Id.ToString());
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid LiqPay payment step.");
            return Form.CreateLast(Name, step + 1, values);
        }

        public OrderPayment GetPayment(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid payment form.");
            return new OrderPayment(Name, "Оплата карткою", form.Parameters);
        }

        public Uri StartSession(IReadOnlyDictionary<string, object> parameters, Uri returnUri)
        {
            // Конвертуємо IReadOnlyDictionary<string, object> в IEnumerable<KeyValuePair<string, string?>>
            var stringParameters = parameters
                .Where(p => p.Value != null) // Фільтруємо null-значення
                .ToDictionary(
                    p => p.Key,
                    p => p.Value.ToString() // Перетворюємо значення в string
                );

            // Створюємо QueryString
            var queryString = QueryString.Create(stringParameters);
            queryString += QueryString.Create("returnUri", returnUri.ToString());

            // Будуємо URL
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = "LiqPay/",
                Query = queryString.ToString(),
            };

            if (Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }

    }
}
