using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YakShop.Api.Infrastructure;
using YakShop.Common.Models;

namespace YakShop.Api.Controllers
{
    [Produces("application/json")]
    [Route("yak-shop/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly YakShopContext _context;
        public OrderController(YakShopContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Creates a new Order with specified CartData based on number of days specified
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /yak-shop/order/15
        ///     {
        ///         "customer" : "Medvedev",
        ///         "order" : {
        ///         "milk" : 1200,
        ///         "skins" : 3
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <returns>Returns a order data with fi=ulfillment details </returns>
        /// <response code="201">Created</response>
        /// <response code="206">Partial Content</response>
        /// <response code="500">In case of any server error</response> 
        [HttpPost("{days}", Name = "CreateOrder")]
        public IActionResult Post(int days, [FromBody] CartData cartData)
        {
            var orderData = _context.CreateOrder(days, cartData);

            if(orderData.Milk != null && orderData.Skin != null)
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, orderData);
            else
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent, orderData);
        }

        [HttpGet(Name = "GetOrders")]
        public IActionResult Get()
        {
            var orderData = _context.Orders.ToList();

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, orderData);
        }
    }
}
