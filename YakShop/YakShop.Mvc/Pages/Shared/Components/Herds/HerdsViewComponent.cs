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
        public HerdsViewComponent(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync(int elapsedDays, bool fetch = true)
        {
            var client = GetHttpClient();
            var path = "Yak-Shop/Load/";

            if (fetch)
            {
                var data = await client.GetStringAsync(path);

                if (!string.IsNullOrEmpty(data))
                    elapsedDays = Convert.ToInt32(data);
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
