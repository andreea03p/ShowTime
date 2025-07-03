using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ShowTimeDbContext _context;

    public  GenericRepository(ShowTimeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the entity.", ex);
        }
    }
   
    public async Task DeleteAsync(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();  
        }
        catch (ArgumentNullException ex)
        {
            throw new Exception("An error occurred while deleting the entity.", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try 
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (ArgumentNullException ex)
        {
            throw new Exception("An error occurred while updating the entity.", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _context.Set<T>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all data.", ex);
        }   
    }

    public async Task<T> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the entity with ID {id}.", ex);
        }
    }
}

