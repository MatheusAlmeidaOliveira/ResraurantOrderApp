using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Structs;

namespace Api.Service.Services
{
    public class MorningOrderService : BaseOrderService
    {
        public override string Entree {get => "eggs";}
        public override string Side {get => "toast";}
        public override string Drink {get => "coffee";}
        public override string Dessert {get => "Not applicable";}

        public override List<string> CreatesTheOrderOutPut(string[] formatedOrder)
        {
            var list = ParsesTheOrder(formatedOrder);

            InsertDish(list.Count(entree => entree == DishesNumbers.EntreeNumber), Entree);
            if (ContainsError(OrderOutputList))
                return OrderOutputList;

            InsertDish(list.Count(side => side == DishesNumbers.SideNumber), Side);
            if (ContainsError(OrderOutputList))
                return OrderOutputList;

            InsertMultipleDishes(list.Count(drink => drink == DishesNumbers.DrinkNumber), Drink);
          
            InsertError(list.Count(ChecksInvalidOrders()));

            return OrderOutputList;
        }

        public override Func<string, bool> ChecksInvalidOrders()
        {
            return order => order != DishesNumbers.EntreeNumber && order != DishesNumbers.SideNumber && order != DishesNumbers.DrinkNumber;
        }
    }
}