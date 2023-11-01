using System;
using System.Collections.Generic;
using web_bmstu.Models;
using web_bmstu.Repository;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IRecordingStudioRepository : IRepository<RecordingStudio>
    {
        RecordingStudio GetByName(string name);
        IEnumerable<RecordingStudio> GetByCountry(string country);
        IEnumerable<RecordingStudio> GetByYearFounded(int yearFounded);
    }
}