using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;

namespace WebVeiculos.Core.Services.Contratos
{
    public interface IVeiculoService
    {
        Task<bool> CreateVeiculoService(VeiculoDto veiculo);
        Task<PaginacaoListDto> GetAllVeiculosService(PaginacaoListDto paginacao);
        Task<VeiculoDto> GetVeiculoByIdService(int id);
        Task<ICollection<VeiculoDto>> GetUltimosVeiculosCadastradosService(int quantidade);
        Task<PaginacaoListDto> GetVeiculoByModeloService(PaginacaoListDto paginacao, string modelo);
    }
}
