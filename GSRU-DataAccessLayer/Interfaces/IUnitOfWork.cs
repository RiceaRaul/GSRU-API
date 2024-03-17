namespace GSRU_DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
