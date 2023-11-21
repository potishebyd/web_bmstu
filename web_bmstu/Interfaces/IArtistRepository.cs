using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;
using web_bmstu.ModelsBL;

namespace web_bmstu.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<Artist> AddAsync(Artist model);
        Task<Artist> UpdateAsync(Artist model);
        Task<Artist> DeleteAsync(int id);
        Task<IEnumerable<Artist>> GetAllAsync();
        Task<Artist> GetByIDAsync(int id);
        Task<IEnumerable<Artist>> GetByCountryAsync(string country);
        Task<Artist> GetByNameAsync(string name);
        Artist GetByName(string name);
        IEnumerable<Artist> GetByCountry(string country);
    }
}
