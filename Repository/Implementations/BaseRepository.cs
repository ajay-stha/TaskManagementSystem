﻿using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Database;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository.Interfaces;

namespace TaskManagementSystem.Repository.Implementations
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
    {
        protected readonly TaskDbContext _context;
        protected BaseRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstAsync(e => e.Id == id);
        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            if (entity != null)
            {
                _context.Remove(entity);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
