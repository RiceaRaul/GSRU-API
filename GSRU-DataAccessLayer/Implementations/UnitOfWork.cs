using GSRU_DataAccessLayer.Interfaces;

namespace GSRU_DataAccessLayer.Implementations
{
    internal class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
