using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YakShop.Common.Models;

namespace YakShop.Api.Infrastructure
{
    public static class YakShopContextExtensions
    {
        public static HerdDataList GetHerdView(this YakShopContext context, int days)
        {
            var all = context.Herds.Select(c => new Herd() {Name = c.Name, Id = c.Id, Age = c.Age }).ToList();
            var setting = context.Settings.FirstOrDefault();

            var deadHerds = new List<Herd>();
            foreach(var herd in all)
            {

                herd.AgeLastShaved = CalculateLastShavedAge(herd.Age, days);
                herd.Age = (decimal)(herd.Age * 100 + days)/100;
                if (herd.Age >= 10)
                    deadHerds.Add(herd);
            }
            
            //Remove dead herds after crossing max age
            HashSet<int> herdIds = new HashSet<int>(deadHerds.Select(x => x.Id));
            var herds = all.Where(x => !herdIds.Contains(x.Id))
                         .ToList();

            return new HerdDataList() { Herd = herds };
        }

        public static StockData GetStockView(this YakShopContext context, int days)
        {
            var all = context.Herds.Select(c => new Herd() { Name = c.Name, Id = c.Id, Age = c.Age }).ToList();
            var setting = context.Settings.FirstOrDefault();

            var stockData = new StockData();
            var deadHerds = new List<Herd>();
            foreach (var herd in all)
            {
                stockData.Skin += CalculateSkinStock(herd.Age, days);
                stockData.Milk += CalculateMilk(herd.Age, days);
            }

            ///Get current set of orders
            var totalOrders = context.Orders.GroupBy(i => 1)
                        .Select(g => new CartItem
                        {
                            Milk = g.Sum(o => o.FullfilledMilk),
                            Skin = g.Sum(o => o.FullfilledSkin)
                        }).FirstOrDefault();

            //If we already have orders subtract fullfilled orders from total stock
            if (totalOrders != null)
            {
                stockData.Milk = totalOrders.Milk == null ? stockData.Milk : (decimal)(stockData.Milk - totalOrders.Milk);
                stockData.Skin = totalOrders.Skin == null ? stockData.Skin : (int)(stockData.Skin - totalOrders.Skin);
            }
            return stockData;
        }

        public static StockData GetSalesView(this YakShopContext context, int days)
        {
            ///Get current set of orders
            var totalOrders = context.Orders.GroupBy(i => 1)
                        .Select(g => new StockData
                        {
                            Milk = (decimal)g.Sum(o => o.Milk),
                            Skin = (int)g.Sum(o => o.Skin)
                        }).FirstOrDefault();


            return totalOrders;
        }

        public static CartItem CreateOrder(this YakShopContext context, int days, CartData cartData)
        {
            var all = context.Herds.Select(c => new Herd() { Name = c.Name, Id = c.Id, Age = c.Age }).ToList();
            var setting = context.Settings.FirstOrDefault();

            var stockData = new StockData();
            var deadHerds = new List<Herd>();
            foreach (var herd in all)
            {
                stockData.Skin += CalculateSkinStock(herd.Age, days);
                stockData.Milk += CalculateMilk(herd.Age, days);
            }

            ///Get current set of orders
            var totalOrders = context.Orders.GroupBy(i => 1)
                        .Select(g => new CartItem
                        {
                            Milk = g.Sum(o => o.Milk),
                            Skin = g.Sum(o => o.Skin)
                        }).FirstOrDefault();

            if (totalOrders != null)
            {
                stockData.Milk = totalOrders.Milk == null ? stockData.Milk : (decimal)(stockData.Milk - totalOrders.Milk);
                stockData.Skin = totalOrders.Skin == null ? stockData.Skin : (int)(stockData.Skin - totalOrders.Skin);
            }
            var orderFullfill = new CartItem();

            orderFullfill.Milk = stockData.Milk >= cartData.Order.Milk ? cartData.Order.Milk : null;
            orderFullfill.Skin = stockData.Skin >= cartData.Order.Skin ? cartData.Order.Skin : null;

            context.Add(new Order() { Customer = cartData .Customer, Milk = cartData.Order.Milk, Skin = cartData.Order.Skin, FullfilledMilk = orderFullfill.Milk, FullfilledSkin = orderFullfill.Skin });
            context.SaveChanges();

            return orderFullfill;
        }

        private static decimal CalculateLastShavedAge(decimal startAge, int days)
        {
            int startAgeDays = (int)startAge * 100;
            int age = startAgeDays;

            //A LabYak cannot be shaved before it becomes 1 year old
            if (startAgeDays <= 100)
                return 0;

            var nextShavingAge = (int)(8 + (age * 0.01M));
            if (days < nextShavingAge)
                return startAge;
            var nextDay = days - nextShavingAge;
            if (nextDay == 0)
                return startAge;

            while (true)
            {
                var temp = (int)(age + nextShavingAge) + 1;

                nextShavingAge = (int)(8 + (temp * 0.01M));

                if (nextDay <= nextShavingAge)
                {
                    if (temp <= (startAgeDays + days))
                        return (decimal)(temp) / 100;
                    else
                        return (decimal)(age) / 100;
                }

                nextDay = nextDay - nextShavingAge;
                age = temp;
            }
        }

        private static int CalculateSkinStock(decimal startAge, int days)
        {
            int maxAge = 1000;
            int startAgeDays = (int)startAge * 100;
            int age = startAgeDays;

            ///A LabYak cannot be shaved before it becomes 1 year old
            if (startAgeDays <= 100)
                return 0;

            var nextShavingAge = (int)(8 + (age * 0.01M));

            if (days < nextShavingAge)
                return 1;
            var nextDay = days - nextShavingAge;
            if (nextDay == 0)
                return 1;

            int skinCount = 1;
            while (true)
            {
                var temp = (int)(age + nextShavingAge) + 1;

                nextShavingAge = (int)(8 + (temp * 0.01M));

                if (nextDay <= nextShavingAge)
                {
                    if (temp <= (startAgeDays + days))
                        return skinCount + 1;
                    else
                        return skinCount;
                }

                nextDay = nextDay - nextShavingAge;
                age = temp;

                if(age >= maxAge)
                    return skinCount;

                skinCount++;
            }
        }

        private static decimal CalculateMilk(decimal startAge, int days)
        {
            decimal maxAge = 1000;
            decimal startAgeDays = startAge * 100;
            decimal age = startAgeDays + days;
            if (age > maxAge)
                age = maxAge;

            var startValue = (decimal)(50 - (startAgeDays * 0.03M));
            var endValue = (decimal)(50 - (age * 0.03M));
            var sum = ((age - startAgeDays) * (startValue + endValue)) / 2;

            return sum;
        }
    }
}
