using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Models.Repositories
{
    public class ConexaoDbDapper : IDisposable
    {
        private IConfiguration _configuration { get; set; }
        public DbConnection ConnectionDapper { get; set; }
        public ConexaoDbDapper(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionDapper = new MySqlConnection(_configuration.GetConnectionString("conMySqlDbVeiculoInfo"));
            ConnectionDapper.Open();
        }
        public void Dispose()
        {
            ConnectionDapper?.Dispose();
        }
    }
}
