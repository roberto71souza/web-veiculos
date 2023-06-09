using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace WebVeiculos.Models.Repositories
{
    public class ConexaoDbDapper : IDisposable
    {
        private IConfiguration _configuration { get; set; }
        private DbConnection ConnectionDapper { get; set; }
        public ConexaoDbDapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection GetConnectionDb()
        {
            ConnectionDapper = new MySqlConnection(_configuration.GetConnectionString("conMySqlDbVeiculoInfo"));
            ConnectionDapper.Open();

            return ConnectionDapper;
        }

        public void Dispose()
        {
            ConnectionDapper?.Dispose();
        }
    }
}
