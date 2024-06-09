using System.Data;

namespace GSRU_DataAccessLayer.Repositories
{
    public class RepositoryBase(IDbTransaction transaction)
    {
        protected IDbTransaction Transaction { get; private set; } = transaction;
        protected IDbConnection Connection { get { return Transaction.Connection!; } }
    }
}
