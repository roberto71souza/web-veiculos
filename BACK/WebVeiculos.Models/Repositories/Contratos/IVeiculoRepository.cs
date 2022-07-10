using System.Collections.Generic;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Entities.Paginacao;

namespace WebVeiculos.Models.Repositories.Contratos
{
    public interface IVeiculoRepository
    {
        Task<bool> CreateVeiculo(Veiculo veiculo);
        Task<PaginacaoList> GetAllVeiculos(PaginacaoList paginacao);
        Task<Veiculo> GetVeiculoById(int id);
        Task<ICollection<Veiculo>> GetUltimosVeiculosCadastrados(int quantidade);
        Task<PaginacaoList> GetVeiculoByModelo(PaginacaoList paginacao, string modelo);
    }
}
