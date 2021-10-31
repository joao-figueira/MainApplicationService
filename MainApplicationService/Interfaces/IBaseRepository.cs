using System.Threading;
using System.Threading.Tasks;

namespace MainApplicationService.Interfaces
{
    /*
     * All the repositories should implement this interface. It has all the CRUD operations.
     */
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T genericObject, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T genericObject, CancellationToken cancellationToken = default);
        string? Delete(T genericObject, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
