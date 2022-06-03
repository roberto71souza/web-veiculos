using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Repositories.Contratos;

namespace WebVeiculos.Models.Repositories.Implementacao
{
    public class VeiculoRepository : IVeiculoRepository
    {
        public ConexaoDbDapper _conexaoDb { get; set; }
        public VeiculoRepository(ConexaoDbDapper conexaoDb)
        {
            _conexaoDb = conexaoDb;
        }

        public Task<ICollection<Veiculo>> GetAllVeiculos()
        {
            throw new NotImplementedException();
        }

        public Task CreateVeiculo(Veiculo veiculo)
        {
            throw new NotImplementedException();
        }


        public Task<Veiculo> GetVeiculoById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Veiculo>> GetVeiculoByModelo(string modelo)
        {
            throw new NotImplementedException();
        }
    }
}
