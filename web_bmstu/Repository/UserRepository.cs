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

        public async Task<User> AddAsync(User model)
        {
            try
            {
                await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();
                return await GetByIDAsync(model.Id);
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

        public async Task<User> UpdateAsync(User model)
        {
            try
            {
                var curModel = _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                _context.Entry(curModel).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();
                return await GetByIDAsync(model.Id);
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

        public async Task<User> DeleteAsync(int id)
        {
            try
            {
                User user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public User GetByID(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User> GetByIDAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public User GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }


        public async Task<User> GetByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public IEnumerable<User> GetByPermission(string permission)
        {
            return _context.Users.Where(u => u.Permission == permission).ToList();
        }
        public async Task<IEnumerable<User>> GetByPermissionAsync(string permission)
        {
            return await _context.Users.Where(u => u.Permission == permission).ToListAsync();
        }
    }
}
