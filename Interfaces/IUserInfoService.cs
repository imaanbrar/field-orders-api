using FieldOrdersAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldOrdersAPI.Interfaces
{
    public interface IUserInfoService
    {
        User CurrentUser { get; }

        int UserId { get; }

        bool UserFound { get; }
    }
}
