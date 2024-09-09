﻿using System.Linq.Expressions;

namespace Holdy.Holdy.Core.Domain.RepositoryContracts
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T?>> GetAllAsync();
        Task<IEnumerable<T?>> GetAllAsync(params string[] joins);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(int id, params string[] joins);
        Task InsertAsync (T entity);
        Task UpdateAsync (T oldEntity, T newEntity);
        Task DeleteAsync (T entity);
        Task<T?> SelectByMatchAsync (Expression<Func<T, bool>> match);
        Task<T?> SelectByMatchAsync (Expression<Func<T, bool>> match, params string[] joins);
        Task<IEnumerable<T?>?> SelectListByMatchAsync(Expression<Func<T, bool>> match);
        Task<IEnumerable<T?>?> SelectListByMatchAsync(Expression<Func<T, bool>> match, params string[] joins);
    }
}
