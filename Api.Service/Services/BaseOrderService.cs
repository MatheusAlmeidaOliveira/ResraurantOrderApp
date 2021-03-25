using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Service.Services
{
    public abstract class BaseOrderService
    {
        public abstract string Entree { get; }
        public abstract string Side { get; }
        public abstract string Drink { get; }
        public abstract string Dessert { get; }
        public string Error { get => "error"; }
        public List<string> OrderOutputList = new List<string>();

        public abstract List<string> CreatesTheOrderOutPut(string[] formatedOrder);

        public abstract Func<string, bool> ChecksInvalidOrders();

        public bool ContainsError(List<string> orderOutputList)
        {
            return orderOutputList.Contains(Error);
        }

        public List<string> ParsesTheOrder(string[] formatedOrder)
        {
            const byte periodOfTheDay = 0;
            var list = formatedOrder.ToList();
            list.RemoveAt(periodOfTheDay);
            return list;
        }

        public void InsertDish(int amountOfDishes, string meal)
        {
            if (amountOfDishes == 1)
                OrderOutputList.Add(meal);
            else if (amountOfDishes > 1)
            {
                OrderOutputList.Add(meal);
                OrderOutputList.Add(Error);
            }
        }

        public void InsertMultipleDishes(int amountOfDishes, string meal)
        {
            if (amountOfDishes == 1)
                OrderOutputList.Add(meal);
            else if (amountOfDishes > 1)
                OrderOutputList.Add($"{meal}(x{amountOfDishes})");
        }

        public void InsertError(int errors)
        {
            if (errors >= 1)
                OrderOutputList.Add(Error);
        }
    }
}