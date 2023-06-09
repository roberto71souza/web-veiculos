using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Core.Services.Contratos;

namespace WebVeiculos.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VeiculosController : ControllerBase
    {
        private IVeiculoService _veiculoService { get; set; }

        public VeiculosController(IVeiculoService service)
        {
            _veiculoService = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromHeader] int proximaPagina = 1)
        {
            try
            {
                var paginacao = new PaginacaoListDto();
                paginacao.PaginaAtual = proximaPagina;

                var paginacaoResult = await _veiculoService.GetAllVeiculosService(paginacao);

                if (paginacaoResult.Veiculos.Any())
                {
                    return Ok(new
                    {
                        data = paginacaoResult.Veiculos,
                        paginacao = new
                        {
                            paginaAtual = paginacaoResult.PaginaAtual,
                            totalPaginas = paginacaoResult.TotalPaginas
                        }
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                , $"Erro na aplicação ao tentar buscar todos os veiculos. ERRO: {ex.Message}");
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var veiculo = await _veiculoService.GetVeiculoByIdService(id);

                if (veiculo is not null)
                {
                    return Ok(veiculo);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                , $"Erro na aplicação ao tentar buscar veiculo. ERRO: {ex.Message}");
            }
        }

        [HttpGet("getUltimosVeiculos/{qtdItens}")]
        public async Task<ActionResult> GetUltimosVeiculos(int qtdItens)
        {
            try
            {
                var veiculos = await _veiculoService.GetUltimosVeiculosCadastradosService(qtdItens);

                if (veiculos.Any())
                {
                    return Ok(veiculos);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                 , $"Erro na aplicação ao tentar buscar veiculo. ERRO: {ex.Message}");
            }

        }

        [HttpGet("getByModelo/{modelo}/{proximaPagina}")]
        public async Task<ActionResult> GetByModelo(string modelo, int proximaPagina)
        {
            try
            {
                var paginacao = new PaginacaoListDto();
                paginacao.PaginaAtual = proximaPagina;

                var paginacaoResult = await _veiculoService.GetVeiculoByModeloService(paginacao, modelo);

                if (paginacaoResult.Veiculos.Any())
                {
                    return Ok(new
                    {
                        data = paginacaoResult.Veiculos,
                        paginacao = new
                        {
                            paginaAtual = paginacaoResult.PaginaAtual,
                            totalPaginas = paginacaoResult.TotalPaginas
                        }
                    });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                , $"Erro na aplicação ao tentar buscar buscar veiculo. ERRO: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] VeiculoIFormDto veiculoIForm)
        {
            try
            {
                var veiculoDto = veiculoIForm.Veiculo;

                var arquivos = veiculoIForm.FormFile;

                var (result, veiculoDtoMap) = await _veiculoService.CreateVeiculoService(arquivos, veiculoDto);

                if (result)
                {
                    return Ok("Veiculo cadastrado com sucesso!");
                }

                return BadRequest(new
                {
                    error = new
                    {
                        veiculoError = veiculoDtoMap.ListaDeErros,
                        arquivosError = veiculoDtoMap.Arquivos.SelectMany(x => x.ListaDeErros).Distinct()
                    }
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                , $"Erro na aplicação ao tentar cadastrar veiculo. ERRO: {ex.Message}");
            }
        }
    }
}
