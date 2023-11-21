using System;
using System.Collections.Generic;
using System.Linq;
using web_bmstu.Models;
using web_bmstu.Interfaces;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace web_bmstu.Repository
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Artist Add(Artist model)
        {
            try
            {
                _context.Artists.Add(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении артиста");
            }
        }
        public async Task<Artist> AddAsync(Artist model)
        {
            await _context.Artists.AddAsync(model);
            await _context.SaveChangesAsync();
            return GetByID(model.Id);
        }
        public Artist Update(Artist model)
        {
            try
            {
                _context.Artists.Update(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении артиста");
            }
        }
        public async Task<Artist> UpdateAsync(Artist model)
        {
            try
            {
                _context.Artists.Update(model);
                await _context.SaveChangesAsync();
                return await GetByIDAsync(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении артиста");
            }
        }


        public Artist Delete(int id)
        {
            try
            {
                var artist = _context.Artists.Find(id);
                if (artist != null)
                {
                    _context.Artists.Remove(artist);
                    _context.SaveChanges();
                }
                return artist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении артиста");
            }
        }

        public async Task<Artist> DeleteAsync(int id)
        {
            try
            {
                var artist = await  _context.Artists.FindAsync(id);
                if (artist != null)
                {
                    _context.Artists.Remove(artist);
                    await _context.SaveChangesAsync();
                }
                return artist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении артиста");
            }
        }

        public IEnumerable<Artist> GetAll()
        {
            return _context.Artists.ToList();
        }

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public Artist GetByID(int id)
        {
            return _context.Artists.Find(id);
        }

        public async Task<Artist> GetByIDAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public Artist GetByName(string name)
        {
            return _context.Artists.FirstOrDefault(a => a.Name == name);
        }

        public async Task<Artist> GetByNameAsync(string name)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.Name == name);
        }

        public IEnumerable<Artist> GetByCountry(string country)
        {
            return _context.Artists.Where(a => a.Country == country).ToList();
        }
        public async Task<IEnumerable<Artist>> GetByCountryAsync(string country)
        {
            return await _context.Artists.Where(a => a.Country == country).ToListAsync();
        }
    }
}
