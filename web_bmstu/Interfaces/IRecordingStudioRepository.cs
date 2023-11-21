using System;
using System.Collections.Generic;
using web_bmstu.Models;
using web_bmstu.Repository;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IRecordingStudioRepository : IRepository<RecordingStudio>
    {
        Task<RecordingStudio> AddAsync(RecordingStudio model);
        Task<RecordingStudio> UpdateAsync(RecordingStudio model);
        Task<RecordingStudio> DeleteAsync(int id);
        Task<IEnumerable<RecordingStudio>> GetAllAsync();
        Task<RecordingStudio> GetByIDAsync(int id);
        Task<IEnumerable<RecordingStudio>> GetByCountryAsync(string country);
        Task<RecordingStudio> GetByNameAsync(string name);
        Task<IEnumerable<RecordingStudio>> GetByYearFoundedAsync(int yearFounded);
        RecordingStudio GetByName(string name);
        IEnumerable<RecordingStudio> GetByCountry(string country);
        IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded);
    }
}