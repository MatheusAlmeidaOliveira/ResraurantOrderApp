using System;
using System.Collections.Generic;
using System.Linq;
using Api.Domain.Structs;

namespace Api.Service.Services
{
    public class NightOrderService : BaseOrderService
    {
        public override string Entree {get => "steak";}
        public override string Side {get => "potato";}
        public override string Drink {get => "wine";}
        public override string Dessert {get => "cake";}

        public override List<string> CreatesTheOrderOutPut(string[] formatedOrder)
        {
            var list = ParsesTheOrder(formatedOrder);

            InsertDish(list.Count(entree => entree == DishesNumbers.EntreeNumber), Entree);
            if (ContainsError(OrderOutputList))
                return OrderOutputList;

            InsertMultipleDishes(list.Count(side => side == DishesNumbers.SideNumber), Side);

            InsertDish(list.Count(drink => drink == DishesNumbers.DrinkNumber), Drink);
            if (ContainsError(OrderOutputList))
                return OrderOutputList;
            
            InsertDish(list.Count(dessert => dessert == DishesNumbers.DessertNumber), Dessert);
            if (ContainsError(OrderOutputList))
                return OrderOutputList;
            
            InsertError(list.Count(ChecksInvalidOrders()));

            return OrderOutputList;
        }

        public override Func<string, bool> ChecksInvalidOrders()
        {
            return order => order != DishesNumbers.EntreeNumber && order != DishesNumbers.SideNumber && order != DishesNumbers.DrinkNumber && order != DishesNumbers.DessertNumber;
        }
    }
}