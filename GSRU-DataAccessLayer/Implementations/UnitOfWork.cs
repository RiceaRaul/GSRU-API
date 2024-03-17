using GSRU_DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace GSRU_DataAccessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection? _connection;
        private NpgsqlTransaction? _transaction;

        public UnitOfWork(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DatabaseConnection");
            if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString,"DatabaseConnection is null");

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
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
