using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Utility;

namespace BuzzBotTwo.Repository
{
    public interface IRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
        where TKey :  IComparable, IEquatable<TKey>
    {
        Task<TEntity> FindAsync(TKey id, QueryInject<TEntity> queryInject = null);
        Task<List<TEntity>> GetAsync(QueryInject<TEntity> queryInject = null);
        Task<bool> PostAsync(TEntity entity);
        Task<bool> PutAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task SaveAllChangesAsync();
        Task<bool> Contains(TKey key);
    }
}