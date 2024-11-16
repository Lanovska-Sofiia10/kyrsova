using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Kyrsova.Contractors
{
    public class DeliveryService : IDeliveryService
    {
        private static readonly IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Київ"}, {"2", "Луцьк"}, {"3", "Львів"}, {"4", "Одеса"}, {"5", "Дніпро"},
            {"6", "Харків"}, {"7", "Запоріжжя"}, {"8", "Вінниця"}, {"9", "Чернігів"}, {"10", "Полтава"},
            {"11", "Черкаси"}, {"12", "Хмельницький"}, {"13", "Чернівці"}, {"14", "Ужгород"}, {"15", "Рівне"},
            {"16", "Івано-Франківськ"}, {"17", "Тернопіль"}, {"18", "Миколаїв"}, {"19", "Херсон"}, {"20", "Суми"},
            {"21", "Житомир"}, {"22", "Кропивницький"}, {"23", "Сєвєродонецьк"}
        };

        private static readonly IReadOnlyDictionary<string, string> deliveryTypes = new Dictionary<string, string>
        {
            {"1", "Поштомат Нової Пошти"},
            {"2", "Відділення Нової Пошти"},
            {"3", "Відділення Укрпошти"}
        };

        // Список поштоматів Нової Пошти
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            { "1", new Dictionary<string, string> { {"1", "№5485"}, {"2", "№3024"}, {"3", "№3468"}, {"4", "№25330"}, {"5", "№3090"} } },
            { "2", new Dictionary<string, string> { {"1", "№35669" }, {"2", "№31619" }, {"3", "№26352" }, {"4", "№6267" }, {"5", "№36441" } } },
            { "3", new Dictionary<string, string> { {"1", "№31386" }, {"2", "№3284" }, {"3", "№26606" }, {"4", "№5531" }, {"5", "№5509" } } },
            { "4", new Dictionary<string, string> { {"1", "№36386" }, {"2", "№26265" }, {"3", "№46010" }, {"4", "№34184" }, {"5", "№31559" } } },
            { "5", new Dictionary<string, string> { {"1", "№46437" }, {"2", "№34251" }, {"3", "№5422" }, {"4", "№35853" }, {"5", "№4896" } } },
            { "6", new Dictionary<string, string> { {"1", "№20678" }, {"2", "№3161" }, {"3", "№46964" }, {"4", "№36355" }, {"5", "№36433" } } },
            { "7", new Dictionary<string, string> { {"1", "№32080" }, {"2", "№5564" }, {"3", "№46502" }, {"4", "№3427" }, {"5", "№31819" } } },
            { "8", new Dictionary<string, string> { {"1", "№31607" }, {"2", "№40516" }, {"3", "№35489" }, {"4", "№45808" }, {"5", "№45659" } } },
            { "9", new Dictionary<string, string> { {"1", "№5617" }, {"2", "№46577" }, {"3", "№46993" }, {"4", "№5592" }, {"5", "№46583" } } },
            { "10", new Dictionary<string, string> { {"1", "№35666" }, {"2", "№36469" }, {"3", "№3401" }, {"4", "№3151" }, {"5", "№20155" } } },
            { "11", new Dictionary<string, string> { {"1", "№26274" }, {"2", "№32872" }, {"3", "№46561" }, {"4", "№32583" }, {"5", "№37391" } } },
            { "12", new Dictionary<string, string> { {"1", "№41083" }, {"2", "№41775" }, {"3", "№36641" }, {"4", "№35416" }, {"5", "№34594" } } },
            { "13", new Dictionary<string, string> { {"1", "№5274" }, {"2", "№35518" }, {"3", "№36601" }, {"4", "№36729" }, {"5", "№35711" } } },
            { "14", new Dictionary<string, string> { {"1", "№20640" }, {"2", "№32406" }, {"3", "№31481" }, {"4", "№45117" }, {"5", "№46517" } } },
            { "15", new Dictionary<string, string> { {"1", "№5614" }, {"2", "№40469" }, {"3", "№36717" }, {"4", "№36188" }, {"5", "№35432" } } },
            { "16", new Dictionary<string, string> { {"1", "№36827" }, {"2", "№36417" }, {"3", "№35375" }, {"4", "№36414" }, {"5", "№40176" } } },
            { "17", new Dictionary<string, string> { {"1", "№46205" }, {"2", "№35473" }, {"3", "№23337" }, {"4", "№46648" }, {"5", "№45746" } } },
            { "18", new Dictionary<string, string> { {"1", "№26948" }, {"2", "№29767" }, {"3", "№34606" }, {"4", "№20987" }, {"5", "№46032" } } },
            { "19", new Dictionary<string, string> { {"1", "№45312" }, {"2", "№5658" }, {"3", "№5677" } } },
            { "20", new Dictionary<string, string> { {"1", "№36013" }, {"2", "№45696" }, {"3", "№45953" }, {"4", "№35808" }, {"5", "№36016" } } },
            { "21", new Dictionary<string, string> { {"1", "№45632" }, {"2", "№45637" }, {"3", "№20741" }, {"4", "№45635" }, {"5", "№41702" } } },
            { "22", new Dictionary<string, string> { {"1", "№36220" }, {"2", "№46688" }, {"3", "№46917" }, {"4", "№46689" }, {"5", "№46445" } } },
            { "23", new Dictionary<string, string> { {"1", "№6476" }, {"2", "№5669" }, {"3", "№6232" }, {"4", "№5683" } } },
        };

        // Список відділень Нової Пошти
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> novaPoshtaBranches = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            { "1", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "2", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "3", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "4", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "5", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "6", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "7", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "8", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "10", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "11", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "12", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "13", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "14", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "15", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "16", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "17", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "18", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "19", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "20", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "21", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "22", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "23", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
        };

        // Список відділень Укрпошти
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> ukrPoshtaBranches = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            { "1", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "2", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "3", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "4", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "5", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "6", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "7", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "8", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "10", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "11", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "12", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "13", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "14", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "15", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "16", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "17", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "18", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "19", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "20", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "21", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "22", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
            { "23", new Dictionary<string, string> { {"1", "Відділення №1"}, {"2", "Відділення №2"}, {"3", "Відділення №3"}, { "4", "Відділення №4" }, { "5", "Відділення №5" } } },
        };

        public string Name => "DeliveryService";
        public string Title => "Доставка";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                .AddParameter("orderId", order.Id.ToString())
                .AddField(new SelectionField("Місто", "city", cities))
                .AddField(new SelectionField("Тип доставки", "deliveryType", deliveryTypes));
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                string city = values["city"];
                string deliveryType = values["deliveryType"];

                if (deliveryType == "1") // Поштомат Нової Пошти
                {
                    return Form.CreateNext(Name, 2, values)
                        .AddField(new SelectionField("Поштомат", "postamate", postamates[city]));
                }
                else if (deliveryType == "2") // Відділення Нової Пошти
                {
                    return Form.CreateNext(Name, 2, values)
                        .AddField(new SelectionField("Відділення Нової Пошти", "novaPoshtaBranch", novaPoshtaBranches[city]));
                }
                else if (deliveryType == "3") // Відділення Укрпошти
                {
                    return Form.CreateNext(Name, 2, values)
                        .AddField(new SelectionField("Відділення Укрпошти", "ukrPoshtaBranch", ukrPoshtaBranches[city]));
                }
                else
                {
                    throw new InvalidOperationException("Невірний тип доставки");
                }
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, 3, values);
            }
            else
            {
                throw new InvalidOperationException("Невірний крок форми");
            }
        }

        public OrderDelivery GetDelivery(Form form)
        {
            // Перевірка на правильність форми
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Невірна форма");

            var cityId = form.Parameters["city"];
            string cityName = cities.ContainsKey(cityId) ? cities[cityId] : "Невідоме місто";

            string postamateId = form.Parameters.ContainsKey("postamate") ? form.Parameters["postamate"] : null;
            string postamateName = (postamateId != null && postamates.ContainsKey(cityId) && postamates[cityId].ContainsKey(postamateId))
                ? postamates[cityId][postamateId]
                : "Не вибрано";

            string novaPoshtaBrancheId = form.Parameters.ContainsKey("novaPoshtaBranch") ? form.Parameters["novaPoshtaBranch"] : null;
            string novaPoshtaBrancheName = (novaPoshtaBrancheId != null && novaPoshtaBranches.ContainsKey(cityId) && novaPoshtaBranches[cityId].ContainsKey(novaPoshtaBrancheId))
                ? novaPoshtaBranches[cityId][novaPoshtaBrancheId]
                : "Не вибрано";

            string ukrPoshtaBrancheId = form.Parameters.ContainsKey("ukrPoshtaBranch") ? form.Parameters["ukrPoshtaBranch"] : null;
            string ukrPoshtaBrancheName = (ukrPoshtaBrancheId != null && ukrPoshtaBranches.ContainsKey(cityId) && ukrPoshtaBranches[cityId].ContainsKey(ukrPoshtaBrancheId))
                ? ukrPoshtaBranches[cityId][ukrPoshtaBrancheId]
                : "Не вибрано";

            var parameters = new Dictionary<string, string>
    {
        { nameof(cityId), cityId },
        { nameof(cityName), cityName },
        { nameof(postamateId), postamateId ?? "N/A" },
        { nameof(postamateName), postamateName },
        { nameof(novaPoshtaBrancheId), novaPoshtaBrancheId ?? "N/A" },
        { nameof(novaPoshtaBrancheName), novaPoshtaBrancheName },
        { nameof(ukrPoshtaBrancheId), ukrPoshtaBrancheId ?? "N/A" },
        { nameof(ukrPoshtaBrancheName), ukrPoshtaBrancheName },
    };

            // Опис замовлення
            var description = $"Місто: {cityName}\n" +
                              $"Поштомат: {postamateName}\n" +
                              $"Відділення Нової Пошти: {novaPoshtaBrancheName}\n" +
                              $"Відділення Укрпошти: {ukrPoshtaBrancheName}\n";

            // Повернення замовлення з відповідними параметрами
            return new OrderDelivery(Name, description, 150m, parameters);
        }

    }
}
