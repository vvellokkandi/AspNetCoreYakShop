﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YakShop.Api.Infrastructure;

namespace YakShop.Api.Controllers
{
    [Produces("application/json")]
    [Route("yak-shop/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly YakShopContext _context;
        public StockController(YakShopContext context)
        {
            _context = context;

        }

        /// <summary>
        /// Gets a view of stock after specified number of days
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /yak-shop/stock/15
        ///     {
        ///       "milk": 1104.48,
        ///       "skins": 3
        ///     }
        ///
        /// </remarks>
        /// <returns>Returns a view of stock after specified number of days </returns>
        /// <response code="200">OK</response>
        /// <response code="500">In case of any server error</response> 
        [HttpGet("{days}", Name = "GetStock")]
        public IActionResult Get(int days)
        {
            var stockData = _context.GetStockView(days);
  
            return Ok(stockData);
        }

    }
}
