﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class SubCategoryRepository : IGenericRepository<SubCategory, int>
    {
        private readonly StoreContext _context;

        public SubCategoryRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SubCategory>> GetALl()
        {
            return await _context.SubCategories.ToListAsync();
        }

        public async Task<SubCategory> GetById(int id)
        {
            return await _context.SubCategories
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<bool> Delete(int id)
        {
            var subCategory = await GetById(id);
            _context.Remove(subCategory);
            return await SaveChanges();
        }

        public async Task<bool> Add(SubCategory entity)
        {
            await _context.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> Update(SubCategory entity)
        {
            _context.Update(entity);
            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}