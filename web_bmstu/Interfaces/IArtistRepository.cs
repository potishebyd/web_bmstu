using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Artist GetByName(string name);
        IEnumerable<Artist> GetByCountry(string country);
    }
}
