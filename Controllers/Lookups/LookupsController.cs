using System.Linq;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FieldOrdersAPI.Models;
using FieldOrdersAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;

namespace FieldOrdersAPI.Controllers.Lookups
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly FieldOrdersContext _context;

        public LookupsController(FieldOrdersContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public object GetOrderStatusAsLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.OrderStatus
                         select new
                         {
                             Value = i.Id,
                             Text = i.Value,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object GetShippingMethodAsLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.ShippingMethod
                         select new
                         {
                             Value = i.Id,
                             Text = i.Value,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }
    }
}
