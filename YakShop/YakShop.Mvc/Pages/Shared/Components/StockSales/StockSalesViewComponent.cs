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

namespace YakShop.Mvc.Pages.Components.StockSales
{
    public class StockSalesViewComponent : ViewComponent
    {
        readonly IOptions<ApiSettings> _apiSettings;
        public StockSalesViewComponent(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isStock, int elapsedDays, bool fetch = true)
        {
            var client = GetHttpClient();
            var path = "Yak-Shop/Load/";

            if (fetch)
            {
                var data = await client.GetStringAsync(path);

                if (!string.IsNullOrEmpty(data))
                    elapsedDays = Convert.ToInt32(data);
            }
            var model = new StockSalesModel();
            if (isStock)
            {
                path = "Yak-Shop/Stock/" + elapsedDays;
                model.Title = "Stocks";
            }
            else
            {
                path = "Yak-Shop/Sales/" + elapsedDays;
                model.Title = "Sales";
            }

            var response = await client.GetAsync(path);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                model.Data = JsonConvert.DeserializeObject<StockData>(await response.Content.ReadAsStringAsync());
            }
            return View("Default", model);
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

    public class StockSalesModel
    {
        public StockSalesModel()
        {
            Data = new StockData();
        }
        public string Title { get; set; }

        public StockData Data { get; set; }
    }
}
