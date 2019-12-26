using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Models;

namespace Database.Interfaces
{
    public interface IRepository<TEntity> where TEntity: BaseModel
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> Create(TEntity entity);
        Task Delete(string id);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> GetById(string id);
    }
}
