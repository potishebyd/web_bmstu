using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface ISongRepository : IRepository<Song>
    {
        Song GetByTitle(string title);
        IEnumerable<Song> GetByAlbumTitle(string albumTitle);
        IEnumerable<Song> GetByGenre(string genre);
        IEnumerable<Song> GetByDuration(TimeSpan Duration);
    }
}
