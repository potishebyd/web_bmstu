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

        public IEnumerable<Song> GetAll()
        {
            return _context.Songs.ToList();
        }

        public Song GetByID(int id)
        {
            return _context.Songs.Find(id);
        }

        public Song GetByTitle(string title)
        {
            return _context.Songs.FirstOrDefault(s => s.Title == title);
        }
        public IEnumerable<Song> GetByAlbumTitle(string albumTitle)
        {
            return _context.Songs.Where(s => s.AlbumTitle == albumTitle).ToList();
        }

        public IEnumerable<Song> GetByGenre(string genre)
        {
            return _context.Songs.Where(s => s.Genre == genre).ToList();
        }

        public IEnumerable<Song> GetByDuration(TimeSpan duration)
        {
            return _context.Songs.Where(s => s.Duration == duration).ToList();
        }
    }
}
