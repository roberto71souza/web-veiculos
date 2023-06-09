using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;

namespace WebVeiculos.Core.Services.Contratos
{
    public interface IVeiculoService
    {
        Task<(bool, VeiculoDto)> CreateVeiculoService(List<IFormFile> arquivos, VeiculoDto veiculo);
        Task<PaginacaoListDto> GetAllVeiculosService(PaginacaoListDto paginacao);
        Task<VeiculoDto> GetVeiculoByIdService(int id);
        Task<ICollection<VeiculoDto>> GetUltimosVeiculosCadastradosService(int quantidade);
        Task<PaginacaoListDto> GetVeiculoByModeloService(PaginacaoListDto paginacao, string modelo);
    }
}
