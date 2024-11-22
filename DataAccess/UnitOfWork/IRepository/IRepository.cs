using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniversityStudyPlatform.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
        void Add(T entity);
        void Remove(T entity);
    }
}
