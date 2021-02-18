using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Domain.Seed
{
    public static class DataSeederExtensions
    {
        public static ModelBuilder UseSeeder<TSeeder, TEntity>(this ModelBuilder modelBuilder) where TSeeder : IDataSeeder<TEntity> where TEntity : class
        {
            var seeder = Activator.CreateInstance<TSeeder>();
            modelBuilder.Entity<TEntity>().HasData(seeder.Data.ToArray());
            return modelBuilder;
        }
    }
}