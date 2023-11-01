using System;
using System.Collections.Generic;
using web_bmstu.Models;
using NodaTime;

namespace web_bmstu.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T model);
        T Update(T model);
        T Delete(int id);

        IEnumerable<T> GetAll();
        T GetByID(int id);
    }
}
