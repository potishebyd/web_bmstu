using System;
using System.Collections.Generic;
using System.Linq;
using web_bmstu.Models;
using web_bmstu.Interfaces;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace web_bmstu.Repository
{
    public class RecordingStudioRepository : IRecordingStudioRepository
    {
        private readonly ApplicationDbContext _context;

        public RecordingStudioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public RecordingStudio Add(RecordingStudio model)
        {
            try
            {
                _context.RecordingStudios.Add(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при добавлении студии звукозаписи");
            }
        }

        public RecordingStudio Update(RecordingStudio model)
        {
            try
            {
                _context.RecordingStudios.Update(model);
                _context.SaveChanges();
                return GetByID(model.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при обновлении студии звукозаписи");
            }
        }

        public RecordingStudio Delete(int id)
        {
            try
            {
                var studio = _context.RecordingStudios.Find(id);
                if (studio != null)
                {
                    _context.RecordingStudios.Remove(studio);
                    _context.SaveChanges();
                }
                return studio;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Ошибка при удалении студии звукозаписи");
            }
        }

        public IEnumerable<RecordingStudio> GetAll()
        {
            return _context.RecordingStudios.ToList();
        }

        public RecordingStudio GetByID(int id)
        {
            return _context.RecordingStudios.Find(id);
        }

        public RecordingStudio GetByName(string name)
        {
            return _context.RecordingStudios.FirstOrDefault(s => s.Name == name);
        }

        public IEnumerable<RecordingStudio> GetByCountry(string country)
        {
            return _context.RecordingStudios.Where(s => s.Country == country).ToList();
        }

        public IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded)
        {
            return _context.RecordingStudios.Where(s => s.YearFounded == yearFounded).ToList();
        }
    }
}
