using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;

namespace GSRU_DataAccessLayer.Repositories
{
    public class TestRepository : RepositoryBase, ITestRepository
    {
        public TestRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task GetTest()
        {
          
            
        }
    }
}
