using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FieldOrdersAPI.Models;
using FieldOrdersAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FieldOrdersAPI.Controllers.Orders
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderCommentsController : ControllerBase
    {
        private readonly FieldOrdersContext _context;

        public OrderCommentsController(FieldOrdersContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<OrderComment>>> GetOrderCommentByOrderID(int id)
        {
            var result = await _context.OrderComment
                                       .Where(x => x.OrderId == id)
                                       .ToListAsync();
            return Ok(result);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> PutOrderComment([FromForm]int key, [FromForm] string values)
        {
            var orderComment = await _context.OrderComment.FirstOrDefaultAsync(o => o.Id == key);
            if (orderComment == null)
                return StatusCode(409, "Item not found");

            JsonConvert.PopulateObject(values, orderComment);

            if (!TryValidateModel(orderComment))
                return BadRequest();

            await _context.SaveChangesAsync();

            return Ok(orderComment);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrderComment([FromForm] string values)
        {
            var newOrderComment = new OrderComment();

            JsonConvert.PopulateObject(values, newOrderComment);

            if (!TryValidateModel(newOrderComment))
                return BadRequest();

            await _context.OrderComment.AddAsync(newOrderComment);
            await _context.SaveChangesAsync();

            return Ok(newOrderComment);

        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteOrderComment([FromForm] int key)
        {
            try
            {
                var orderComment = await _context.OrderComment.FirstAsync(o => o.Id == key);

                _context.OrderComment.Remove(orderComment);

                await _context.SaveChangesAsync();

                return Ok(orderComment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
