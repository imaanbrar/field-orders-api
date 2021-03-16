using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldOrdersAPI.Enums;
using FieldOrdersAPI.Interfaces;
using FieldOrdersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Z.EntityFramework.Plus;

namespace FieldOrdersAPI.Controllers.Orders
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FieldOrdersController : ControllerBase
    {
        private readonly FieldOrdersContext _context;
        private readonly IUserInfoService _userInfo;

        protected string OrderType { get; } = "FieldOrder";
        protected string ShortCode { get; } = "FO";

        public FieldOrdersController(FieldOrdersContext context, IUserInfoService userInfo)
        {
            _context = context;
            _userInfo = userInfo;

            _context.Filter<Order>(q => q.Where(o => o.OrderType == OrderType));
        }

        // GET: api/FieldOrders/GetOrders
        [HttpGet, Authorize]
        public async Task<ActionResult> GetOrders()
        {
            var result = await _context.Order
                                       .OrderBy(x => x.Number)
                                       .ToListAsync();
            return Ok(result);
        }

        // GET: api/FieldOrders/GetOrderList
        [HttpGet, Authorize]
        public async Task<ActionResult> GetOrderList()
        {
            if (!_userInfo.UserFound)
            {
                return NoContent();
            }

            var userId = _userInfo.UserId;

            var order = await _context.Order
                                      .Select(r => new
                                      {
                                          r.Id,
                                          r.Project.CompanyId,
                                          r.ProjectId,
                                          r.Number,
                                          r.Name,
                                          r.StatusId
                                      })
                                      .OrderBy(x => x.Number)
                                      .ToListAsync();
            return Ok(order);
        }

        // GET: api/FieldOrders/GetOrderSummaryById?id=5
        [HttpGet, Authorize]
        public virtual async Task<IActionResult> GetOrderSummaryById(int id)
        {
            var summary = await _context.Order
                                        .Where(x => x.Id == id)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.ProjectId,
                                            x.Number,
                                            x.Name,
                                            x.StatusId,
                                            Status = x.Status.Value,
                                        })
                                        .FirstOrDefaultAsync();
            if (summary == null)
            {
                return NotFound();
            }

            return Ok(summary);
        }

        // GET: api/FieldOrders/GetOrderById?id=5
        [HttpGet, Authorize]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Order
                                    .Where(x => x.Id == id)
                                    .Include(x => x.Project)
                                    .Include(x => x.FieldVendor)
                                    .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/FieldOrders/PutOrder?id=5
        [HttpPut, Authorize]
        public async Task<IActionResult> PutOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            order.OrderType = OrderType;
            var fieldVendor = order.FieldVendor.FirstOrDefault() ?? new FieldVendor { OrderId = id };

            _context.Entry(order).State = EntityState.Modified;
            _context.Attach(fieldVendor).State = fieldVendor.Id > 0 ? EntityState.Modified : EntityState.Added;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/FieldOrders/PostOrder
        [HttpPost, Authorize]
        public virtual async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            order.OrderType = OrderType;

            await _context.Order.AddAsync(order);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrders", null, order);
        }

        // POST: api/FieldOrders/PostOrderUsingGrid
        [HttpPost, Authorize]
        public async Task<IActionResult> PostOrderUsingGrid([FromForm] string values)
        {
            var order = new Order();

            JsonConvert.PopulateObject(values, order);

            if (!TryValidateModel(order))
            {
                return BadRequest(ModelState);
            }

            order.Number = await GenerateNumber(order.ProjectId);
            order.OrderType = OrderType;

            order.InitiatedDate = DateTime.Now;

            await _context.Order.AddAsync(order);
            await _context.FieldVendor.AddAsync(new FieldVendor { OrderId = order.Id });

            await _context.SaveChangesAsync();

            _context.Entry(order).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // DELETE: api/FieldOrders/DeleteOrder/5
        [Authorize, HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);

            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // GET: api/FieldOrders/CheckOrderNumber
        [HttpGet, Authorize]
        public async Task<bool> CheckOrderNumber(int id, int projectId, string number)
        {
            return await _context.Order
                                 .AnyAsync(r =>
                                      r.ProjectId == projectId && r.Number == number && (id == 0 || r.Id != id) &&
                                      r.StatusId != (int)eOrderStatus.eReplaced &&
                                      r.StatusId != (int)eOrderStatus.eCancelled);
        }

        private async Task<string> GenerateNumber(int projectId)
        {
            var count = await _context.Order.CountAsync(x =>
                x.ProjectId == projectId && x.StatusId != (int)eOrderStatus.eReplaced);

            var number = new[]
            {
                (await _context.Project.FindAsync(projectId)).Number,
                ShortCode,
                (count + 1).ToString().PadLeft(4, '0')
            };

            return string.Join("-", number);
        }

        private async Task<bool> OrderExists(int id)
        {
            return await _context.Order.AnyAsync(e => e.Id == id);
        }
    }
}
