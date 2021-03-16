using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FieldOrdersAPI.Enums;
using FieldOrdersAPI.Models;
using FieldOrdersAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Z.EntityFramework.Plus;

namespace FieldOrdersAPI.Controllers.Orders
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly FieldOrdersContext _context;

        public OrderItemsController(FieldOrdersContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItemByOrderID(int id)
        {
            try
            {

                
                var result = await _context.OrderItem
                    .Where(x => x.OrderId == id)
                    .OrderBy(x => x.ItemNumber)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> PutOrderItem([FromForm]int key, [FromForm] string values)
        {
            var orderItem = _context.OrderItem.FirstOrDefault(o => o.Id == key);
            if (orderItem == null)
                return StatusCode(409, "Item not found");

            JsonConvert.PopulateObject(values, orderItem);

            if (!TryValidateModel(orderItem))
                return BadRequest();

            await _context.SaveChangesAsync();

            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrderItem([FromForm] string values)
        {
            var newOrderItem = new OrderItem();
            JsonConvert.PopulateObject(values, newOrderItem);

            if (!TryValidateModel(newOrderItem))
                return BadRequest();

            await _context.OrderItem.AddAsync(newOrderItem);
            await _context.SaveChangesAsync();

            return Ok(newOrderItem);

        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteOrderItem([FromForm] int key)
        {
            try
            {
                var orderItem = await _context.OrderItem.FirstAsync(o => o.Id == key);

                _context.OrderItem.Remove(orderItem);

                await _context.SaveChangesAsync();
                resetItemNumbers(orderItem.Id, orderItem.OrderId);
                await _context.SaveChangesAsync();

                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete, Authorize]
        public void DeleteOrderItemPretend([FromForm] int key)
        {
            //do nothing
        }

        [HttpGet, Authorize]
        public async Task<bool> CheckDelete(int id)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var item = await _context.OrderItem.FindAsync(id);

            if (item == null)
            {
                return false;
            }
            try
            {
                await DeleteOrderItem(id);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        #region Lookups

        [HttpGet, Authorize]
        public object GetProjectWBSAsLookup(int projectId, DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.ProjectWbs
                         where i.ProjectId == projectId
                         orderby i.TaskCode
                         select new
                         {
                             Value = i.Id,
                             Text = i.TaskCode + " - " + i.TaskDescription,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }

        #endregion

        private void resetItemNumbers(int currentId, int mrId)
        {
            var orderItems = _context.OrderItem.Where(x => x.OrderId == mrId && x.Id != currentId).OrderBy(x => x.ItemNumber).ToList();

            for (int i = 0; i < orderItems.Count; i++)
            {
                orderItems[i].ItemNumber = i + 1;
            }
        }

    }
}
