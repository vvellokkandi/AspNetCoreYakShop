using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YakShop.Mvc.Controllers
{
    public class ComponentController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> HerdsView(int days)
        {
           return ViewComponent(typeof(YakShop.Mvc.Pages.Components.Herds.HerdsViewComponent), new { elapsedDays = 0, fetch = true });
        }

        [HttpGet]
        public async Task<IActionResult> StockView(int days)
        {
            return ViewComponent(typeof(YakShop.Mvc.Pages.Components.StockSales.StockSalesViewComponent), new { isStock = true, elapsedDays = days, fetch = true });
        }

        [HttpGet]
        public async Task<IActionResult> SalesView(int days)
        {
            return ViewComponent(typeof(YakShop.Mvc.Pages.Components.StockSales.StockSalesViewComponent), new { isStock = false, elapsedDays = days, fetch = true });
        }

        [HttpGet]
        public async Task<IActionResult> OrdersView(int days)
        {
            return ViewComponent(typeof(YakShop.Mvc.Pages.Components.Orders.OrdersViewComponent));
        }

        [HttpGet]
        public async Task<IActionResult> CartView()
        {
            return ViewComponent(typeof(YakShop.Mvc.Pages.Components.Cart.CartViewComponent), new YakShop.Mvc.Pages.Components.Cart.CartModel());
        }

        [HttpPost]
        public async Task<IActionResult> CartViewValidate([FromBody]YakShop.Mvc.Pages.Components.Cart.CartModel modelData)
        {
            return ViewComponent(typeof(YakShop.Mvc.Pages.Components.Cart.CartViewComponent), new { model = modelData });
        }
    }
}