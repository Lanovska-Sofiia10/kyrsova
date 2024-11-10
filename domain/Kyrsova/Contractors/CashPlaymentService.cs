using System;
using System.Collections.Generic;

namespace Kyrsova.Contractors
{
    public class CashPlaymentService : IPaymentService
    {
        public string UniqueCode => "Card";

        public string Title => "Оплата при отриманні";

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id, 1, false, new Field[0]);
        }

        public OrderPayment GetPayment(Form form)
        {
            if (form.UniqueCode != UniqueCode || !form.IsFinal)
                throw new InvalidOperationException("Invalid payment form. ");

            return new OrderPayment(UniqueCode, "Оплата при отриманні", new Dictionary<string, string>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash step.");

            return new Form(UniqueCode, orderId, 2, true, new  Field[0]);
        }
    }
}
