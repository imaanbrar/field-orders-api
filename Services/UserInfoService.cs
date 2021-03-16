using FieldOrdersAPI.Interfaces;
using FieldOrdersAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldOrdersAPI.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _http;
        private readonly FieldOrdersContext _context;

        public User CurrentUser { get; }

        public int UserId => CurrentUser?.Id ?? 0;

        public bool UserFound => CurrentUser != null;

        public UserInfoService(IHttpContextAccessor http, FieldOrdersContext context)
        {
            _http = http;
            _context = context;

            // var userId = _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // TODO: At some point this should be included in an authorization token from azure
            // TODO: At some point this should include default user roles and project roles for authorization
            CurrentUser = context.User.FirstOrDefault(u => u.Username == http.HttpContext.User.Identity.Name);

            context.SetCurrentUser(CurrentUser);
        }
    }
}
