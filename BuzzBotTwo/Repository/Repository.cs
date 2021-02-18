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
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    where TKey :  IComparable, IEquatable<TKey>
    {
        private readonly BotContext _db;

        public Repository(BotContext db)
        {
            _db = db;
        }

        public async Task<bool> Contains(TKey key)
        {
            return (await _db.Set<TEntity>().FindAsync(key) != default(TEntity));
        }

        public async Task<TEntity> FindAsync(TKey id, QueryInject<TEntity> queryInject = null)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>();
            if (queryInject != null)
            {
                query = queryInject(query);
            }

            return await query.FirstOrDefaultAsync(ent => ent.Id.Equals(id));
        }

        public async Task<List<TEntity>> GetAsync(QueryInject<TEntity> queryInject = null)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>();
            if (queryInject != null)
            {
                query = queryInject(query);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> PostAsync(TEntity entity)
        {
            return (await _db.Set<TEntity>().AddAsync(entity)) != null;
        }

        public async Task<bool> PutAsync(TEntity entity)
        {
            var existing = await _db.Set<TEntity>().FindAsync(entity.Id);
            if (existing == default(TEntity)) return false;
            _db.Set<TEntity>().Remove(entity);
            return await PostAsync(entity);

        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(_db.Set<TEntity>().Remove(entity) != null);
        }

        public async Task SaveAllChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}