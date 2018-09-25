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
    /// <summary>
    /// YakShop Herd API
    /// </summary>
    [Produces("application/json")]
    [Route("yak-shop/[controller]")]
    [ApiController]
    public class HerdController : ControllerBase
    {
        private readonly YakShopContext _context;
        public HerdController(YakShopContext context)
        {
            _context = context;

            //Test code to be removed later
            //if (!_context.Herds.Any())
            //{
            //    var items = new HerdList()
            //    {
            //        Herds = new List<LabYak>() {
            //            new LabYak() {
            //                Name = "Betty-1",
            //                Age = 4,
            //                Sex = "f"
            //            },
            //            new LabYak() {
            //                Name = "Betty-2",
            //                Age = 8,
            //                Sex = "f"
            //            },
            //             new LabYak() {
            //                Name = "Betty-3",
            //                Age = 9.5M,
            //                Sex = "f"
            //            }
            //        }
            //    };
            //    _context.Herds.AddRange(items.Herds);
            //    _context.SaveChanges();
            //}
        }

        /// <summary>
        /// Gets a view of all herds after specified number of days
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /yak-shop/herd/15
        ///     {
        ///     "herd": [
        ///         {
        ///           "id": 1,
        ///           "name": "Betty-1",
        ///           "age": 4.72,
        ///           "ageLastShaved": 4.65
        ///         },
        ///         {
        ///           "id": 2,
        ///           "name": "Betty-2",
        ///           "age": 8.72,
        ///           "ageLastShaved": 8.68
        ///         }
        ///     }
        ///
        /// </remarks>
        /// <returns>Returns a view of all herds after specified number of days </returns>
        /// <response code="200">OK</response>
        /// <response code="204">No Content</response>
        /// <response code="500">In case of any server error</response> 
        [HttpGet("{days}", Name = "GetHerd")]
        public IActionResult Get(int days)
        {
            var herdList = _context.GetHerdView(days);
            if (herdList.Herd.Count() == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(herdList);
            }
        }

    }
}
