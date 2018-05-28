using System.Linq;
using CSI_Miami.Data.Models.Abstracts;

namespace CSI_Miami.Data.Repository
{
    public interface IDataRepository<T> where T : class, IEditable, IDeletable
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllAndDeleted { get; }

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
