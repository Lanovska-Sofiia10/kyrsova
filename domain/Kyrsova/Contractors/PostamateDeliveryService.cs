﻿using System;
using System.Collections.Generic;

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

        public string UniqueCode => "DeliveryService";
        public string Title => "Доставка";

        public string GetCityNameFromForm(IDictionary<string, string> formFields)
        {
            if (formFields.ContainsKey("city"))
            {
                var cityId = formFields["city"];
                if (cities.ContainsKey(cityId))
                {
                    return cities[cityId];
                }
                else
                {
                    throw new ArgumentException("Невідомий cityId");
                }
            }
            else
            {
                throw new ArgumentException("Поле 'city' не знайдено у формі");
            }
        }

        public OrderDelivery GetDelivery(Form form)
        {
            if (form.UniqueCode != UniqueCode || !form.IsFinal)
               throw new InvalidOperationException("Invalid form.");
            
            var cityId = form.Fields
                             .Single(Field => Field.Name == "city")
                             .Value;
            var cityName = cities[cityId];

            var postamateId = form.Fields
                             .Single(Field => Field.Name == "city")
                             .Value;

            var novaPoshtaBrancheId = form.Fields
                                           .SingleOrDefault(field => field.Name == "novaPoshtaBranch")?.Value;
            var ukrPoshtaBrancheId = form.Fields
                                          .SingleOrDefault(field => field.Name == "ukrPoshtaBranch")?.Value;

            var postamateName = postamateId != null && postamates[cityId].ContainsKey(postamateId)
                                ? postamates[cityId][postamateId]
                                : null;

            var novaPoshtaBrancheName = novaPoshtaBrancheId != null && novaPoshtaBranches[cityId].ContainsKey(novaPoshtaBrancheId)
                                        ? novaPoshtaBranches[cityId][novaPoshtaBrancheId]
                                        : null;

            var ukrPoshtaBrancheName = ukrPoshtaBrancheId != null && ukrPoshtaBranches[cityId].ContainsKey(ukrPoshtaBrancheId)
                                       ? ukrPoshtaBranches[cityId][ukrPoshtaBrancheId]
                                       : null;

            var parameters = new Dictionary<string, string>
     {
        { nameof(cityId), cityId },
        { nameof(cityName), cityName },
        { nameof(postamateId), postamateId },
        { nameof(postamateName), postamateName },
        { nameof(novaPoshtaBrancheId), novaPoshtaBrancheId },
        { nameof(novaPoshtaBrancheName), novaPoshtaBrancheName },
        { nameof(ukrPoshtaBrancheId), ukrPoshtaBrancheId },
        { nameof(ukrPoshtaBrancheName), ukrPoshtaBrancheName },
     };

            var description = $"Місто: {cityName}\n" +
                              $"Поштомат: {postamateName}\n" +
                              $"Відділення Нової Пошти: {novaPoshtaBrancheName}\n" +
                              $"Відділення Укрпошти: {ukrPoshtaBrancheName}\n";

            return new OrderDelivery(UniqueCode, description, 150m, parameters);
        }



        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
               new SelectionField("Тип доставки", "deliveryType", "1", deliveryTypes),
               new SelectionField("Місто", "city", "1", cities) // Тут додається поле "city"
            });
        }


        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (!values.ContainsKey("deliveryType") || !values.ContainsKey("city"))
                    throw new InvalidOperationException("Required fields 'deliveryType' or 'city' not found in values.");

                string deliveryType = values["deliveryType"];
                string city = values["city"];

                IReadOnlyDictionary<string, string> options = deliveryType switch
                {
                    "1" => postamates.ContainsKey(city) ? postamates[city] : throw new InvalidOperationException("Місто не має поштоматів."),
                    "2" => novaPoshtaBranches.ContainsKey(city) ? novaPoshtaBranches[city] : throw new InvalidOperationException("Місто не має відділень Нової Пошти."),
                    "3" => ukrPoshtaBranches.ContainsKey(city) ? ukrPoshtaBranches[city] : throw new InvalidOperationException("Місто не має відділень Укрпошти."),
                    _ => throw new InvalidOperationException("Невідомий тип доставки.")
                };

                return new Form(UniqueCode, orderId, 2, false, new Field[]
                {
            new HiddenField("Тип доставки", "deliveryType", deliveryType),
            new HiddenField("Місто", "city", city),
            new SelectionField("Вибір відділення", "branch", "1", options)
                });
            }
            if (step == 2)
            {
                return new Form(UniqueCode, orderId, 0, true, new Field[]
                {
            new HiddenField("Місто", "city", values.ContainsKey("city") ? values["city"] : "")
                });
            }

            throw new InvalidOperationException("Неправильний крок.");
        }

    }
}