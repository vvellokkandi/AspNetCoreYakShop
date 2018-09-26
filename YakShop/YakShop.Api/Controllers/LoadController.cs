using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using YakShop.Common.Models;
using YakShop.Api.Infrastructure;

namespace YakShop.Api.Controllers
{
    /// <summary>
    /// YakShop Load API
    /// </summary>
    [Produces("application/json")]
    [Route("yak-shop/[controller]")]
    [ApiController]
    
    public class LoadController : ControllerBase
    {
        private readonly YakShopContext _context;

        public LoadController(YakShopContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Resets the herd list. This API will clear all the existing herds and loads new set of herds
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /yak-shop/load
        ///     <herd>
        ///         <labyak name = "Betty-1" age="4" sex="f"/>
        ///         <labyak name = "Betty-2" age="4.5" sex="f"/>
        ///     </herd>
        /// </remarks>
        /// <param name="herdList"></param>
        /// <returns></returns>
        /// <response code="205">Reset contentm</response>
        /// <response code="500">In case of any server error</response> 
        [HttpPost]
        public IActionResult Post([FromBody] HerdList herdList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ///Clear existing content
                var all = _context.Herds.Select(c => c);
                _context.Herds.RemoveRange(all);

                ///Load new data
                _context.Herds.AddRange(herdList.Herds);
                _context.SaveChanges();

                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status205ResetContent);
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet(Name = "GetElapsedDays")]
        public IActionResult Get()
        {
            var settings = _context.Settings.FirstOrDefault();

            var elapsedDays = 0;
            if (settings != null)
                elapsedDays = settings.ElapsedDays;

            return Ok(elapsedDays);
        }

        [HttpPost("{days}", Name = "SetElapsedDays")]
        public IActionResult Post(int days)
        {
            var settings = _context.Settings.FirstOrDefault();

            if(settings == null)
            {
                settings = new Settings();
                _context.Settings.Add(settings);
            }
            settings.ElapsedDays = days;

            _context.SaveChanges();

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status205ResetContent);
        }

    }
}
