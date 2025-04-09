using Microsoft.EntityFrameworkCore;
using SensorApp.Infrastructure.Data;

namespace SensorApp.Infrastructure.Repositories;

public class DbRepository<T> : IRepository<T> where T : class
{
    private readonly SensorDbContext _dbContext;

    public DbRepository(SensorDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }
}
