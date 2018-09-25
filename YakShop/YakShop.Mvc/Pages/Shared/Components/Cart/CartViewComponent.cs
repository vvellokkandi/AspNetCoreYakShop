using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YakShop.Common.Models;
using YakShop.Mvc.Settings;

namespace YakShop.Mvc.Pages.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        readonly IOptions<ApiSettings> _apiSettings;

        public CartViewComponent(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync(CartModel model)
        {
            return View("Default", model);
       }
    }

    public class CartModel
    {
        public CartModel()
        {
            Milk = 1;
            Skin = 1;
        }
        [Required]
        public string Customer { get; set; }

        [Required]
        public decimal Milk { get; set; }

        [Required]
        public int Skin { get; set; }

        public int Days { get; set; }

        public string Message { get; set; }
    }
}
