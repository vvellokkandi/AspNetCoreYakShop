using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YakShop.Common.Models;
using YakShop.Mvc.Settings;

namespace YakShop.Mvc.Pages.Components.Orders
{
    public class OrdersViewComponent : ViewComponent
    {
        readonly IOptions<ApiSettings> _apiSettings;
        public OrdersViewComponent(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = GetHttpClient();

            //Get Herd List
            var path = "Yak-Shop/Order/";
            var response = await client.GetAsync(path);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
                return View("Default", orders);
            }
            else
            {
                return View("Default", new List<Order>());
            }
            
        }

        private HttpClient GetHttpClient()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_apiSettings.Value.Url);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
