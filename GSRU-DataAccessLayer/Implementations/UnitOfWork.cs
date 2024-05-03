using GSRU_DataAccessLayer.Interfaces;
using GSRU_DataAccessLayer.Repositories;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GSRU_DataAccessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection? _connection;
        private IDbTransaction? _transaction;

        private ITestRepository? _testRepository;

        public UnitOfWork(IConfiguration configuration)
        {
            string? connectionString = Environment.GetEnvironmentVariable("GSRU__CONNECTIONSTRINGS__DatabaseConnection");
            if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString,"DatabaseConnection is null");

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public ITestRepository TestRepository
        {
            get { return _testRepository ?? (_testRepository = new TestRepository(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                if (_transaction is null)
                    return;

                _transaction.Commit();
            }
            catch
            {
                _transaction!.Rollback();
                throw;
            }
            finally
            {
                if (_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = _connection!.BeginTransaction();
                }
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                if(_connection is not null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
        }

        ~UnitOfWork() {
            Dispose(false);
        }
    }
}
