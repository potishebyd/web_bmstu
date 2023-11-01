using System;
using System.Collections.Generic;
using System.Linq;
using web_bmstu.Models;
using web_bmstu.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using NodaTime;

namespace web_bmstu.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Playlist Add(Playlist model)
        {
            try
            {
                _context.Playlists.Add(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении плейлиста");
            }
        }

        public Playlist Update(Playlist model)
        {
            try
            {
                _context.Playlists.Update(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении плейлиста");
            }
        }

        public Playlist Delete(int id)
        {
            try
            {
                var playlist = _context.Playlists.Find(id);
                if (playlist != null)
                {
                    _context.Playlists.Remove(playlist);
                    _context.SaveChanges();
                }
                return playlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении плейлиста");
            }
        }

        public IEnumerable<Playlist> GetAll()
        {
            return _context.Playlists.ToList();
        }

        public Playlist GetByID(int id)
        {
            return _context.Playlists.Find(id);
        }

        public Playlist GetByName(string name)
        {
            return _context.Playlists.FirstOrDefault(p => p.Name == name);
        }

        public Playlist GetByUserId(int userId)
        {
            return _context.Playlists.FirstOrDefault(p => p.UserId == userId);
        }

        public void AddSongPlaylist(int songId, int playlistId)
        {
            try
            {
                SongPlaylist songPlaylist = new SongPlaylist
                {
                    SongId = songId,
                    PlaylistId = playlistId
                };
                _context.SongPlaylists.Add(songPlaylist);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении песни в плейлист");
            }
        }

        public void DeleteSongPlaylist(int songId, int playlistId)
        {
            try
            {
                SongPlaylist songPlaylist = _context.SongPlaylists.FirstOrDefault(sp => sp.SongId == songId && sp.PlaylistId == playlistId);
                if (songPlaylist != null)
                {
                    _context.SongPlaylists.Remove(songPlaylist);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении песни из плейлиста");
            }
        }

        public IEnumerable<SongPlaylist> GetAllSongPlaylist()
        {
            return _context.SongPlaylists.ToList();
        }

        public IEnumerable<SongPlaylist> GetSongPlaylistBySongId(int songId)
        {
            return _context.SongPlaylists.Where(sp => sp.SongId == songId).ToList();
        }

        public SongPlaylist GetSongPlaylist(int songId, int playlistId)
        {
            return _context.SongPlaylists.FirstOrDefault(sp => sp.SongId == songId && sp.PlaylistId == playlistId);
        }

        public IEnumerable<Song> GetSongsByPlaylistId(int playlistId)
        {
            IEnumerable<int> songPlaylist = _context.SongPlaylists
                .Where(sp => sp.PlaylistId == playlistId)
                .Select(sp => sp.SongId).ToList();

            IEnumerable<Song> songs = _context.Songs
                .Where(s => songPlaylist.Contains(s.Id)).ToList();

            return songs;
        }

        public IEnumerable<Song> GetSongsByUserId(int userId)
        {
            Playlist playlist = _context.Playlists.FirstOrDefault(p => p.UserId == userId);

            IEnumerable<int> songPlaylist = _context.SongPlaylists
                .Where(sp => sp.PlaylistId == playlist.Id)
                .Select(sp => sp.SongId).ToList();

            IEnumerable<Song> songs = _context.Songs
                .Where(s => songPlaylist.Contains(s.Id)).ToList();

            return songs;
        }

    }
}
