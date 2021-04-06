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

        public object GetClientsAsLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Company
                         let text = i.Number + " - " + i.Name
                         orderby i.Number
                         select new
                         {
                             Value = i.Id,
                             Text = text,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }
        
        public object GetProjectWBSAsLookup(int projectId, DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.ProjectWbs
                         where i.ProjectId == projectId
                         select new
                         {
                             Value = i.Id,
                             Text = i.TaskCode + " - " + i.TaskDescription,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet]
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
    }
}
