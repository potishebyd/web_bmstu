using System;
using System.Collections.Generic;
using System.Linq;
using web_bmstu.Models;
using web_bmstu.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace web_bmstu.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Add(User model)
        {
            try
            {
                _context.Users.Add(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Ошибка при добавлении пользователя");
            }
        }

        public User Update(User model)
        {
            try
            {
                var curModel = _context.Users.FirstOrDefault(u => u.Id == model.Id);
                _context.Entry(curModel).CurrentValues.SetValues(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Ошибка при обновлении пользователя");
            }
        }

        public User Delete(int id)
        {
            try
            {
                User user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();

                }
                return user;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Ошибка при удалении пользователя");
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByID(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public IEnumerable<User> GetByPermission(string permission)
        {
            return _context.Users.Where(u => u.Permission == permission).ToList();
        }
    }
}
