using System;
using System.Collections.Generic;
using System.Linq;
using web_bmstu.Models;
using web_bmstu.Interfaces;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace web_bmstu.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly ApplicationDbContext _context;

        public SongRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Song Add(Song model)
        {
            try
            {
                _context.Songs.Add(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении песни");
            }
        }
        public async Task<Song> AddAsync(Song model)
        {
            try
            {
                await _context.Songs.AddAsync(model);
                await _context.SaveChangesAsync();
                return await GetByIDAsync(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении песни");
            }
        }

        public Song Update(Song model)
        {
            try
            {
                _context.Songs.Update(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении песни");
            }
        }

        public async Task<Song> UpdateAsync(Song model)
        {
            try
            {
                var updatedSong = await _context.Songs.FirstAsync(m => m.Id == model.Id);
                updatedSong.Title = model.Title;
                updatedSong.ArtistId = model.ArtistId;
                updatedSong.RecordingStudioId = model.RecordingStudioId;
                updatedSong.Duration = model.Duration;
                updatedSong.Genre = model.Genre;
                await _context.SaveChangesAsync();
                return await GetByIDAsync(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении песни");
            }
        }
        public Song Delete(int id)
        {
            try
            {
                var song = _context.Songs.Find(id);
                if (song != null)
                {
                    _context.Songs.Remove(song);
                    _context.SaveChanges();
                }
                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении песни");
            }
        }

        public async Task<Song> DeleteAsync(int id)
        {
            try
            {
                var song = await _context.Songs.FindAsync(id);
                if (song != null)
                {
                    _context.Songs.Remove(song);
                   await _context.SaveChangesAsync();
                }
                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении песни");
            }
        }

        public IEnumerable<Song> GetAll()
        {
            return _context.Songs.ToList();
        }

        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _context.Songs.ToListAsync();
        }
        public Song GetByID(int id)
        {
            return _context.Songs.Find(id);
        }
        public async Task<Song> GetByIDAsync(int id)
        {
            return await _context.Songs.FindAsync(id);
        }
        public Song GetByTitle(string title)
        {
            return _context.Songs.FirstOrDefault(s => s.Title == title);
        }

        public async Task<Song> GetByTitleAsync(string title)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.Title == title);
        }
        public IEnumerable<Song> GetByAlbumTitle(string albumTitle)
        {
            return _context.Songs.Where(s => s.AlbumTitle == albumTitle).ToList();
        }

        public async Task<IEnumerable<Song>> GetByAlbumTitleAsync(string albumTitle)
        {
            return await _context.Songs.Where(s => s.AlbumTitle == albumTitle).ToListAsync();
        }
        public IEnumerable<Song> GetByGenre(string genre)
        {
            return _context.Songs.Where(s => s.Genre == genre).ToList();
        }

        public async Task<IEnumerable<Song>> GetByGenreAsync(string genre)
        {
            return await _context.Songs.Where(s => s.Genre == genre).ToListAsync();
        }

        public IEnumerable<Song> GetByDuration(TimeSpan duration)
        {
            return _context.Songs.Where(s => s.Duration == duration).ToList();
        }

        public async Task<IEnumerable<Song>> GetByDurationAsync(TimeSpan duration)
        {
            return await _context.Songs.Where(s => s.Duration == duration).ToListAsync();
        }
    }
}
