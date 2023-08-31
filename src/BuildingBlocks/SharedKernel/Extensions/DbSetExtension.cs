﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace SharedKernel.Extensions
{
    public static class DbSetExtension
    {
        public static void AddOrUpdate<TEntity>(this DbSet<TEntity> set, TEntity entity) where TEntity : BaseEntity
        {
            _ = set.Any(e => e.Id == entity.Id) == false ? set.Add(entity) : set.Update(entity);
        }

        public static async Task AddOrUpdateAsync<TEntity>(this DbSet<TEntity> set, TEntity entity, CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            _ = await set.AnyAsync(e => e.Id == entity.Id) == false ? await set.AddAsync(entity) : set.Update(entity);
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            foreach (var entity in entities)
            {
                _ = set.Any(e => e.Id == entity.Id) == false ? set.Add(entity) : set.Update(entity);
            }
        }

        public static async Task AddOrUpdateRangeAsync<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            foreach (var entity in entities)
            {
                _ = await set.AnyAsync(e => e.Id == entity.Id) == false ? await set.AddAsync(entity) : set.Update(entity);
            }
        }
    }
}
