using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> AddAsync(User model);
        Task<User> UpdateAsync(User model);
        Task<User> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIDAsync(int id);
        Task<User> GetByLoginAsync(string login);
        Task<IEnumerable<User>> GetByPermissionAsync(string permission);
        User GetByLogin(string login);
        IEnumerable<User> GetByPermission(string permission);
    }
}