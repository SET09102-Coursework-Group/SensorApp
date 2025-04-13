namespace SensorApp.Infrastructure.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
}
