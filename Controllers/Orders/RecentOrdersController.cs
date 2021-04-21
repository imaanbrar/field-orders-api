using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldOrdersAPI.Enums;
using FieldOrdersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FieldOrdersAPI.Controllers.Order
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecentOrdersController : ControllerBase
    {
        private readonly FieldOrdersContext _context;

        public RecentOrdersController(FieldOrdersContext context)
        {
            _context = context;
        }

        // GET: api/RecentOrders/GetRecentOrders
        [HttpGet, Authorize]
        public async Task<ActionResult> GetRecentOrders(string type)
        {
            var query = _context.RecentOrder;

            var result = await query.Select(x => new
            {
                x.Id,
                x.UserId,
                x.OrderId,
                x.Order.OrderType,
                x.Order.Number,
                x.Order.Name,
                x.Order.StatusId,
                x.Order.ProjectId,
                Status = x.Order.Status.Value,
                ProjectNumber = x.Order.Project.Number,
                ProjectName = x.Order.Project.Name,
                CompanyNumber = x.Order.Project.Company.Number,
                CompanyName = x.Order.Project.Company.Name,
                x.CreatedDate
            })
                                    .OrderByDescending(x => x.CreatedDate)
                                    .Take(5)
                                    .ToListAsync();

            return Ok(result);
        }

        // PUT: api/RecentOrders/UpdateRecentOrderList
        [HttpPost, Authorize]
        public async Task<IActionResult> UpdateRecentOrderList([FromBody] int orderId)
        {

            var order = await _context.Order.FirstOrDefaultAsync(o => o.Id == orderId);
            var userId = 1;

            if (userId == 0 || order == null)
            {
                return NotFound();
            }

            var recentOrderList = await _context.RecentOrder
                                                .Where(x => x.UserId == userId && x.Order.OrderType == order.OrderType)
                                                .Include(x => x.Order)
                                                .ToListAsync();

            var isReplaced = order.StatusId == (int)eOrderStatus.eReplaced;
            var existingRecord = recentOrderList.FirstOrDefault(r => r.OrderId == order.Id);

            if (existingRecord != null && !isReplaced)
            {
                existingRecord.CreatedDate = DateTime.Now;

                _context.Entry(existingRecord).State = EntityState.Modified;
            }
            else if (existingRecord != null)
            {
                _context.RecentOrder.Remove(existingRecord);
            }
            else
            {
                if (recentOrderList.Count >= 5)
                {
                    var lastOrder = recentOrderList.OrderBy(x => x.CreatedDate).First();

                    _context.RecentOrder.Remove(lastOrder);
                }

                await _context.RecentOrder.AddAsync(
                    new RecentOrder
                    {
                        OrderId = orderId,
                        UserId = userId
                    }
                );
            }

            await _context.SaveChangesAsync();

            return Ok(orderId);
        }
    }
}