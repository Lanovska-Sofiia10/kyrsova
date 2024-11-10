using Kyrsova;
using Kyrsova.Contractors;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.LiqPay
{
    public class LiqPayPaymentService : IPaymentService, IWebContractorService
    {
        public string UniqueCode => "LiqPay";
        
        public string GetUri => "/LiqPay/";

        public string Title => "Оплата банківською карткою";

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id, 1, false, new Field[0]);
        }

        public OrderPayment GetPayment(Form form)
        {
            return new OrderPayment(UniqueCode, "Оплата карткою", new Dictionary<string, string>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            return new Form(UniqueCode, orderId, 2, true, new Field[0]);
        }
    }
}
