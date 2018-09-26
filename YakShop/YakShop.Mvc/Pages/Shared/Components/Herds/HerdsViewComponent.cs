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

namespace YakShop.Mvc.Pages.Components.Herds
{
    public class HerdsViewComponent : ViewComponent
    {
        readonly IOptions<ApiSettings> _apiSettings;
        readonly IHttpClientFactory _clientFactory;

        public HerdsViewComponent(IOptions<ApiSettings> apiSettings, IHttpClientFactory clientFactory)
        {
            _apiSettings = apiSettings;
            _clientFactory = clientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(int elapsedDays, bool fetch = true)
        {
            var client = _clientFactory.CreateClient("YakShopAPI");
            var path = "Yak-Shop/Load/";

            if (fetch)
            {
                var response1 = await client.GetAsync(path);

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response1.Content.ReadAsStringAsync();
                    elapsedDays = Convert.ToInt32(data.Result);
                }
            }

            //Get Herd List
            path = "Yak-Shop/Herd/" + elapsedDays;
            var response = await client.GetAsync(path);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var herds = JsonConvert.DeserializeObject<HerdDataList>(await response.Content.ReadAsStringAsync()).Herd;
                return View("Default", herds);
            }
            else
            {
                return View("Default", new List<Herd>());
            }
        }
    }
}
