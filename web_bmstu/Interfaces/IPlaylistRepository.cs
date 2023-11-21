using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {

        Task<Playlist> AddAsync(Playlist model);
        Task<Playlist> UpdateAsync(Playlist model);
        Task<Playlist> DeleteAsync(int id);
        Task<IEnumerable<Playlist>> GetAllAsync();
        Task<Playlist> GetByIDAsync(int id);

        Task<Playlist> GetByNameAsync(string name);
        Task<Playlist> GetByUserIdAsync(int userId);


        void AddSongPlaylistAsync(int songId, int playlistId);
        void DeleteSongPlaylistAsync(int songId, int playlistId);

        Task<IEnumerable<SongPlaylist>> GetAllSongPlaylistAsync();
        Task<IEnumerable<SongPlaylist>> GetSongPlaylistBySongIdAsync(int songId);
        Task<SongPlaylist> GetSongPlaylistAsync(int songId, int playlistId);
        Task<IEnumerable<Song>> GetSongsByPlaylistIdAsync(int playlistId);
        Task<IEnumerable<Song>> GetSongsByUserIdAsync(int userId);

        Playlist GetByName(string name);
        Playlist GetByUserId(int userId);


        void AddSongPlaylist(int songId, int playlistId);
        void DeleteSongPlaylist(int songId, int playlistId);

        IEnumerable<SongPlaylist> GetAllSongPlaylist();
        IEnumerable<SongPlaylist> GetSongPlaylistBySongId(int songId);
        SongPlaylist GetSongPlaylist(int songId, int playlistId);
        IEnumerable<Song> GetSongsByPlaylistId(int playlistId);
        IEnumerable<Song> GetSongsByUserId(int userId);

    }
}
