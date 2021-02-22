using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Utility;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Repository
{
    public interface IPaginatedMessageRepository : IRepository<PaginatedMessage, ulong> { }

    public class PaginatedMessageRepository : Repository<PaginatedMessage, ulong>, IPaginatedMessageRepository
    {
        public PaginatedMessageRepository(BotContext db) : base(db)
        {
        }
    }
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    where TKey : IComparable, IEquatable<TKey>
    {
        protected readonly BotContext Db;

        public Repository(BotContext db)
        {
            Db = db;
        }

        public virtual async Task<bool> Contains(TKey key)
        {
            return (await Db.Set<TEntity>().FindAsync(key) != default(TEntity));
        }

        public virtual async Task<TEntity> FindAsync(TKey id, QueryInject<TEntity> queryInject = null)
        {
            IQueryable<TEntity> query = Db.Set<TEntity>();
            if (queryInject != null)
            {
                query = queryInject(query, Db);
            }

            return await query.FirstOrDefaultAsync(ent => ent.Id.Equals(id));
        }

        public virtual async Task<List<TEntity>> GetAsync(QueryInject<TEntity> queryInject = null)
        {
            IQueryable<TEntity> query = Db.Set<TEntity>();
            if (queryInject != null)
            {
                query = queryInject(query, Db);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<bool> PostAsync(TEntity entity)
        {
            return (await Db.Set<TEntity>().AddAsync(entity)) != null;
        }

        public virtual async Task<bool> PutAsync(TEntity entity)
        {
            var existing = await Db.Set<TEntity>().FindAsync(entity.Id);
            if (existing == default(TEntity)) return false;
            Db.Set<TEntity>().Remove(entity);
            return await PostAsync(entity);

        }

        public virtual Task<bool> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Db.Set<TEntity>().Remove(entity) != null);
        }

        public virtual async Task SaveAllChangesAsync()
        {
            await Db.SaveChangesAsync();
        }
    }
}