using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<Song> AddAsync(Song model);
        Task<Song> UpdateAsync(Song model);
        Task<Song> DeleteAsync(int id);
        Task<IEnumerable<Song>> GetAllAsync();
        Task<Song> GetByIDAsync(int id);
        Task<Song> GetByTitleAsync(string title);
        Task<IEnumerable<Song>> GetByAlbumTitleAsync(string albumTitle);
        Task<IEnumerable<Song>> GetByGenreAsync(string genre);
        Task<IEnumerable<Song>> GetByDurationAsync(TimeSpan Duration);
        Song GetByTitle(string title);

        IEnumerable<Song> GetByAlbumTitle(string albumTitle);
        IEnumerable<Song> GetByGenre(string genre);
        IEnumerable<Song> GetByDuration(TimeSpan Duration);
    }
}
