using System;
using System.Collections.Generic;

namespace Kyrsova.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static readonly IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Київ"},
            {"2", "Луцьк"},
            {"3", "Львів"},
            {"4", "Одеса"},
            {"5", "Дніпро"},
            {"6", "Харків"},
            {"7", "Запоріжжя"},
            {"8", "Вінниця"},
            {"9", "Чернігів"},
            {"10", "Полтава"},
            {"11", "Черкаси"},
            {"12", "Хмельницький"},
            {"13", "Чернівці"},
            {"14", "Ужгород"},
            {"15", "Рівне"},
            {"16", "Івано-Франківськ"},
            {"17", "Тернопіль"},
            {"18", "Миколаїв"},
            {"19", "Херсон"},
            {"20", "Суми"},
            {"21", "Житомир"},
            {"22", "Кропивницький"},
            {"23", "Сєвєродонецьк"}
        };

        // Список поштоматів у містах
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            { "1", new Dictionary<string, string> { {"1", "№5485"}, {"2", "№3024"}, {"3", "№3468"}, {"4", "№25330"}, {"5", "№3090"} } }, // Київ
            { "2", new Dictionary<string, string> { {"1", "№6267"}, {"2", "№26352"}, {"3", "№35420"}, {"4", "№35422"}, {"5", "№5977"} } }, // Луцьк
            { "3", new Dictionary<string, string> { {"1", "№1001"}, {"2", "№1002"}, {"3", "№1003"} } }, // Львів
            { "4", new Dictionary<string, string> { {"1", "№2001"}, {"2", "№2002"}, {"3", "№2003"} } }, // Одеса
            { "5", new Dictionary<string, string> { {"1", "№3001"}, {"2", "№3002"}, {"3", "№3003"} } }, // Дніпро
            { "6", new Dictionary<string, string> { {"1", "№4001"}, {"2", "№4002"}, {"3", "№4003"} } }, // Харків
            { "7", new Dictionary<string, string> { {"1", "№5001"}, {"2", "№5002"}, {"3", "№5003"} } }, // Запоріжжя
            { "8", new Dictionary<string, string> { {"1", "№6001"}, {"2", "№6002"}, {"3", "№6003"} } }, // Вінниця
            { "9", new Dictionary<string, string> { {"1", "№7001"}, {"2", "№7002"}, {"3", "№7003"} } }, // Чернігів
            { "10", new Dictionary<string, string> { {"1", "№8001"}, {"2", "№8002"}, {"3", "№8003"} } }, // Полтава
            { "11", new Dictionary<string, string> { {"1", "№9001"}, {"2", "№9002"}, {"3", "№9003"} } }, // Черкаси
            { "12", new Dictionary<string, string> { {"1", "№10001"}, {"2", "№10002"}, {"3", "№10003"} } }, // Хмельницький
            { "13", new Dictionary<string, string> { {"1", "№11001"}, {"2", "№11002"}, {"3", "№11003"} } }, // Чернівці
            { "14", new Dictionary<string, string> { {"1", "№12001"}, {"2", "№12002"}, {"3", "№12003"} } }, // Ужгород
            { "15", new Dictionary<string, string> { {"1", "№13001"}, {"2", "№13002"}, {"3", "№13003"} } }, // Рівне
            { "16", new Dictionary<string, string> { {"1", "№14001"}, {"2", "№14002"}, {"3", "№14003"} } }, // Івано-Франківськ
            { "17", new Dictionary<string, string> { {"1", "№15001"}, {"2", "№15002"}, {"3", "№15003"} } }, // Тернопіль
            { "18", new Dictionary<string, string> { {"1", "№16001"}, {"2", "№16002"}, {"3", "№16003"} } }, // Миколаїв
            { "19", new Dictionary<string, string> { {"1", "№17001"}, {"2", "№17002"}, {"3", "№17003"} } }, // Херсон
            { "20", new Dictionary<string, string> { {"1", "№18001"}, {"2", "№18002"}, {"3", "№18003"} } }, // Суми
            { "21", new Dictionary<string, string> { {"1", "№19001"}, {"2", "№19002"}, {"3", "№19003"} } }, // Житомир
            { "22", new Dictionary<string, string> { {"1", "№20001"}, {"2", "№20002"}, {"3", "№20003"} } }, // Кропивницький
            { "23", new Dictionary<string, string> { {"1", "№21001"}, {"2", "№21002"}, {"3", "№21003"} } }, // Сєвєродонецьк
        };

        public string UniqueCode => "Postamate";
        public string Title => "Доставка через поштомати Нової Пошти";

        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("Місто", "city", "1", cities),
           });
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                string city = values["city"];
                if (postamates.ContainsKey(city))
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Місто", "city", city),
                        new SelectionField("Поштомат", "postamate", "1", postamates[city])
                    });
                }
                throw new InvalidOperationException("Invalid city selected.");
            }

            if (step == 2)
            {
                return new Form(UniqueCode, orderId, 0, true, Array.Empty<Field>());
            }

            throw new InvalidOperationException("Invalid step.");
        }
    }
}
