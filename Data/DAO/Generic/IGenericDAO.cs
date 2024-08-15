using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Generic
{
    public interface IGenericDAO<T> : IDisposable where T : class
    {
        void SaveEntity(dynamic entity, int modulo);
        void AddRows(List<T> list);
        void SaveChanges();
        void SaveChanges(SaveOptions options);
        void SaveChanges(dynamic entity, int modulo);
        IList<T> GetAll();
        void Delete(dynamic entity, int modulo);
        T Single(Func<T, bool> predicate);
        T Update(dynamic entity, long PK, int modulo);
    }
}
