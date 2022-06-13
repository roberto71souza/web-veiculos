using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;

namespace WebVeiculos.Models.Repositories.Contratos
{
    public interface IVeiculoRepository
    {
        Task<bool> CreateVeiculo(Veiculo veiculo);
        Task<ICollection<Veiculo>> GetAllVeiculos();
        Task<Veiculo> GetVeiculoById(int id);
        Task<ICollection<Veiculo>> GetVeiculoByModelo(string modelo);
    }
}
