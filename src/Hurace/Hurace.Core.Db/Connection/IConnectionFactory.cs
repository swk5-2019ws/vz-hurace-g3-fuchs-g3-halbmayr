using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.Db.Connection
{
    public interface IConnectionFactory
    {
        Task<DbConnection> CreateConnectionAsync();
    }
}
