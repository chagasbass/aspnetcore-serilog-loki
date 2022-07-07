using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Worker.Serilog.Loki.Shared.Configurations;

namespace Worker.Serilog.Loki.Infra.Data.DataContexts
{
    public class DataContext : IDisposable
    {
        private readonly BaseConfigurationOptions _baseConfigurationOptions;
        private IDbConnection _DbConnection;
        private readonly SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public DataContext(IOptions<BaseConfigurationOptions> options)
        {
            _baseConfigurationOptions = options.Value;
        }

        public IDbConnection AbrirConexao()
        {
            if (_DbConnection is null || _DbConnection.State != ConnectionState.Open)
            {
                _DbConnection = new SqlConnection(_baseConfigurationOptions.StringConexaoBancoDeDados);
                _DbConnection.Open();
            }

            return _DbConnection;

        }

        public void Dispose()
        {
            if (_DbConnection != null && _DbConnection.State == ConnectionState.Open)
                _DbConnection.Dispose();
        }
    }
}
