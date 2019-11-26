using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.Dal
{
    public interface IDataAccessObject<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T newInstance);
        Task<bool> UpdateAsync(T updatedInstance);
        Task<bool> DeleteByIdAsync(int id);
    }
}
