using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using FieldOrdersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using F23.StringSimilarity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ESCM.Service.Controllers.Companies
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FieldVendorsController : ControllerBase
    {
        private readonly FieldOrdersContext _context;

        public FieldVendorsController(FieldOrdersContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public object GetShippingMethodAsLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.ShippingMethod
                         orderby i.Value
                         select new
                         {
                             Value = i.Id,
                             Text = i.Value,
                             Disabled = !i.IsActive
                         };
            return DataSourceLoader.Load(lookup, loadOptions);
        }

        //[HttpGet, Authorize]
        //public async Task<LoadResult> GetSimilarVendorContacts(int fieldVendorId, DataSourceLoadOptions loadOptions)
        //{
        //    var fieldVendor = await _context.FieldVendor.SingleAsync(fv => fv.Id == fieldVendorId);
        //    var contactName =
        //        new[] { fieldVendor.ContactFirstName, fieldVendor.ContactLastName, fieldVendor.ContactEmail }.Join(" ");

        //    var jw = new JaroWinkler();

        //    var query = _context.CompanyContact
        //                        .Where(cc => cc.IsActive)
        //                        .OrderBy(cc => cc.Id)
        //                        .Select(cc => new
        //                        {
        //                            cc.Id,
        //                            cc.CompanyId,
        //                            cc.CompanyLocationId,
        //                            cc.FirstName,
        //                            cc.LastName,
        //                            cc.Phone,
        //                            cc.Email,
        //                            cc.Cell,
        //                            cc.Fax,
        //                            Similarity = jw.Similarity(
        //                                 new[] { cc.FirstName, cc.LastName, cc.Email }.Join(" "),
        //                                 contactName
        //                             ),
        //                        });

        //    return await DataSourceLoader.LoadAsync(query, loadOptions);
        //}

        //[HttpGet, Authorize]
        //public async Task<LoadResult> GetSimilarVendorLocations(int fieldVendorId, DataSourceLoadOptions loadOptions)
        //{
        //    var fieldVendor = await _context.FieldVendor.SingleAsync(fv => fv.Id == fieldVendorId);
        //    var location = new[]
        //    {
        //        fieldVendor.LocationAddress,
        //        fieldVendor.LocationCity,
        //        fieldVendor.LocationState,
        //        fieldVendor.LocationCountry,
        //        fieldVendor.LocationPostalCode
        //    }.Join();

        //    var jw = new JaroWinkler();

        //    var query = _context.CompanyLocation
        //                        .Where(cl => cl.IsActive)
        //                        .OrderBy(cl => cl.Id)
        //                        .Select(cl => new
        //                        {
        //                            cl.Id,
        //                            cl.CompanyId,
        //                            cl.Address,
        //                            City = cl.City.Value,
        //                            cl.CityId,
        //                            State = cl.State.Value,
        //                            cl.StateId,
        //                            Country = cl.Country.Value,
        //                            cl.CountryId,
        //                            cl.PostalCode,
        //                            cl.Phone,
        //                            cl.Fax,
        //                            cl.Email,
        //                            Similarity = jw.Similarity(
        //                                 new[]
        //                                 {
        //                                     cl.Address,
        //                                     cl.City.Value,
        //                                     cl.State.Value,
        //                                     cl.Country.Value,
        //                                     cl.PostalCode
        //                                 }.Join(", "),
        //                                 location
        //                             )
        //                        });

        //    return await DataSourceLoader.LoadAsync(query, loadOptions);
        //}

        #region Autocomplete Lookups

        [HttpGet, Authorize]
        public object AutocompleteCompanyName(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new { i.CompanyName })
                                 .Union(
                                      _context.Company
                                              .Where(c => c.IsActive)
                                              .Select(i => new { CompanyName = i.Name })
                                  )
                                 .Where(s => s.CompanyName != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationAddress(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.LocationState,
                                     i.LocationCountry,
                                     i.LocationPostalCode,
                                     i.LocationEmail,
                                     i.LocationPhone,
                                     i.LocationFax,
                                     i.ModifiedDate,
                                 })
                                 .Where(s => s.LocationAddress != null)
                                 // Cannot do Group By on Union in EF Core;
                                 // Group By is apparently done in memory anyway, so this shouldn't make a difference
                                 .ToList()
                                 .GroupBy(
                                      s => new
                                      {
                                          s.CompanyName,
                                          s.LocationAddress,
                                          s.LocationCity
                                      },
                                      (key, g) => g.OrderByDescending(e => e.ModifiedDate).First()
                                  );

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationCity(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                            .Select(i => new
                            {
                                i.LocationCity,
                                i.LocationState,
                                i.LocationCountry
                            })
                            .Where(s => s.LocationCity != null)
                            .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationState(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                         .Select(i => new
                         {
                             i.LocationState,
                             i.LocationCountry
                         })
                                 .Where(s => s.LocationState != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationCountry(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                            .Select(i => new { i.LocationCountry })
                            .Where(s => s.LocationCountry != null)
                            .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationPostalCode(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.LocationPostalCode,
                                 })
                                 .Where(s => s.LocationPostalCode != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationEmail(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.LocationEmail,
                                 })
                                 .Where(s => s.LocationEmail != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationPhone(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.LocationPhone,
                                 })
                                 .Where(s => s.LocationPhone != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteLocationFax(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.LocationFax,
                                 })
                                 .Where(s => s.LocationFax != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactFirstName(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactEmail,
                                     i.ContactPhone,
                                     i.ContactFax,
                                     i.ContactCell,
                                     i.ModifiedDate,
                                 })
                                 .Where(s => s.ContactFirstName != null)
                                 // Cannot do Group By on Union in EF Core;
                                 // Group By is apparently done in memory anyway, so this shouldn't make a difference
                                 .ToList()
                                 .GroupBy(
                                      s => new
                                      {
                                          s.CompanyName,
                                          s.LocationAddress,
                                          s.LocationCity,
                                          s.ContactFirstName,
                                          s.ContactLastName
                                      },
                                      (key, g) => g.OrderByDescending(e => e.ModifiedDate).First()
                                  );

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactLastName(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.LocationAddress,
                                     i.LocationCity,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactEmail,
                                     i.ContactPhone,
                                     i.ContactFax,
                                     i.ContactCell,
                                     i.ModifiedDate,
                                 })
                                 .Where(s => s.ContactLastName != null)
                                 // Cannot do Group By on Union in EF Core;
                                 // Group By is apparently done in memory anyway, so this shouldn't make a difference
                                 .ToList()
                                 .GroupBy(
                                      s => new
                                      {
                                          s.CompanyName,
                                          s.LocationAddress,
                                          s.LocationCity,
                                          s.ContactFirstName,
                                          s.ContactLastName
                                      },
                                      (key, g) => g.OrderByDescending(e => e.ModifiedDate).First()
                                  );

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactEmail(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactEmail
                                 })
                                 .Where(s => s.ContactEmail != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactPhone(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactPhone
                                 })
                                 .Where(s => s.ContactPhone != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactFax(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactFax
                                 })
                                 .Where(s => s.ContactFax != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        [HttpGet, Authorize]
        public object AutocompleteContactCell(DataSourceLoadOptions loadOptions)
        {
            var lookup = _context.FieldVendor
                                 .Select(i => new
                                 {
                                     i.CompanyName,
                                     i.ContactFirstName,
                                     i.ContactLastName,
                                     i.ContactCell
                                 })
                                 .Where(s => s.ContactCell != null)
                                 .Distinct();

            return DataSourceLoader.Load(lookup, loadOptions);
        }

        #endregion
    }
}