using GSRU_API.Common.Encryption.Interfaces;
using GSRU_DataAccessLayer.Interfaces;
using GSRU_DataAccessLayer.Repositories;
using GSRU_DataAccessLayer.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace GSRU_DataAccessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection? _connection;
        private IDbTransaction? _transaction;

        private IEmployeeRepository? _employeeRepository;
        private ITeamsRepository? _teamsRepository;
        private IBackLogRepository? _backLogRepository;
        public UnitOfWork(IEncryptionService encryptionService)
        {
            string? connectionString = Environment.GetEnvironmentVariable("GSRU__CONNECTIONSTRINGS__DatabaseConnection");
            if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString,"DatabaseConnection is null");
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _connection = new SqlConnection(encryptionService.Decrypt(connectionString));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IEmployeeRepository EmployeeRepository
        {
            get { return _employeeRepository ??= new EmployeeRepository(_transaction!); }
        }

        public ITeamsRepository TeamsRepository
        {
            get { return _teamsRepository ??= new TeamsRepository(_transaction!); }
        }

        public IBackLogRepository BackLogRepository
        {
            get { return _backLogRepository ??= new BackLogRepository(_transaction!); }
        }

        private void ResetRepositories()
        {
            _employeeRepository = null;
            _teamsRepository = null;
            _backLogRepository = null;
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
                ResetRepositories();
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
            ResetRepositories();
        }

        ~UnitOfWork() {
            Dispose(false);
        }
    }
}
