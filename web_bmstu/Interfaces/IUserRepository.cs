using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByLogin(string login);
        IEnumerable<User> GetByPermission(string permission);
    }
}