using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YakShop.Common.Models;
using YakShop.Mvc.Settings;
using YakShop.Mvc.Pages.Components.Cart;

namespace YakShop.Mvc.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int ElapsedDays { get; set; }

 
        [BindProperty]
        public StockData StockData { get; set; }

        [BindProperty]
        public CartData CartData { get; set; }

        readonly IOptions<ApiSettings> _apiSettings;
        public IndexModel(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
            StockData = new StockData();
            CartData = new CartData();
        }
        public async Task<IActionResult> OnGet()
        {
            ViewData["APIPath"] = _apiSettings.Value.Url;
            ViewData["ClientID"] = Guid.NewGuid().ToString();
            SetDefaultLoadContent();

            //Get elapsed days
            var client = GetHttpClient();
            var path = "Yak-Shop/Load/";
            var data = await client.GetStringAsync(path);

            if (!string.IsNullOrEmpty(data))
                ElapsedDays = Convert.ToInt32(data);


            //Get Stocks
            path = "Yak-Shop/Stock/" + ElapsedDays;
            var response = await client.GetAsync(path);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StockData = JsonConvert.DeserializeObject<StockData>(await response.Content.ReadAsStringAsync());
            }
            return Page();
        }

        public async Task<IActionResult> OnGetHerdView()
        {
            return Page();
            // return ViewComponentResult(typeof(YakShop.Mvc.Pages.Components.Herds.HerdsViewComponent), new { elapsedDays = 0, fetch = false });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostElapsed([FromBody] StartData postData)
        {
            ElapsedDays = postData.Days;

            var client = GetHttpClient();
            var path = "Yak-Shop/Load/" + postData.Days;

            var response = await client.PostAsync(path,null);
            if (response.StatusCode == System.Net.HttpStatusCode.ResetContent)
            {
                return new JsonResult(ElapsedDays);
            }
            else
                return new StatusCodeResult((int)response.StatusCode);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostResetContent([FromBody] HerdList postData)
        {
            var client = GetHttpClient();
            var path = "Yak-Shop/Load/";

            var data = JsonConvert.SerializeObject(postData);
            var response = await client.PostAsync(path, new StringContent(data, Encoding.UTF8, "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.ResetContent)
            {
                return new OkResult();
            }
            else
                return new StatusCodeResult((int)response.StatusCode);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostBuyNow([FromBody] CartModel postData)
        {
            var client = GetHttpClient();
            var path = "Yak-Shop/Order/" + postData.Days;

            var cartItem = new CartData() {
                Customer = postData.Customer,
                Order = new CartItem() {
                    Milk = postData.Milk,
                    Skin = postData.Skin,
                }
            };
            var data = JsonConvert.SerializeObject(cartItem);
            var response = await client.PostAsync(path, new StringContent(data, Encoding.UTF8, "application/json"));

            if (response.StatusCode == System.Net.HttpStatusCode.Created ||
                response.StatusCode == System.Net.HttpStatusCode.PartialContent)
            {
                var resultData = JsonConvert.DeserializeObject<CartItem>(await response.Content.ReadAsStringAsync());
                var cartModel = new CartModel() {
                    Customer = postData.Customer,
                    Skin = resultData.Skin.HasValue ? resultData.Skin.Value : 0,
                    Milk = resultData.Skin.HasValue ? resultData.Milk.Value : 0,
                };
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    cartModel.Message = $"Order for Milk: { cartModel.Milk} and Skin: {cartModel.Skin} placed successfully...";
                if (response.StatusCode == System.Net.HttpStatusCode.PartialContent)
                    cartModel.Message = $"Order for Milk: { cartModel.Milk} and Skin: {cartModel.Skin} partially placed...";

                return StatusCode((int)response.StatusCode, cartModel);
            }
            else
                return StatusCode((int)response.StatusCode, new CartModel() { Customer = postData.Customer });
        }

        private HttpClient GetHttpClient()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_apiSettings.Value.Url);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private void SetDefaultLoadContent()
        {
            var items = new HerdList()
            {
                Herds = new List<LabYak>() {
                        new LabYak() {
                            Name = "Betty-1",
                            Age = 4,
                            Sex = "f"
                        },
                        new LabYak() {
                            Name = "Betty-2",
                            Age = 8,
                            Sex = "f"
                        },
                         new LabYak() {
                            Name = "Betty-3",
                            Age = 9.5M,
                            Sex = "f"
                        }
                    }
            };

            ViewData["DefaultContent"] = JsonConvert.SerializeObject(items);
        }
    }

    public class StartData
    {
        public int Days { get; set; }
    }
}
