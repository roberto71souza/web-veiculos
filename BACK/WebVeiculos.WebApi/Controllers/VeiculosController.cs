using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Core.Services.Contratos;
using WebVeiculos.WebApi.Helper;

namespace WebVeiculos.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VeiculosController : ControllerBase
    {
        private IVeiculoService _veiculoService { get; set; }
        private Util _util { get; set; }

        public VeiculosController(IVeiculoService service, Util util)
        {
            _veiculoService = service;
            _util = util;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var veiculos = await _veiculoService.GetAllVeiculosService();

                if (veiculos.Any())
                {
                    return Ok(veiculos);
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

        [HttpGet("getByModelo/{modelo}")]
        public async Task<ActionResult> GetByModelo(string modelo)
        {
            try
            {
                var veiculos = await _veiculoService.GetVeiculoByModeloService(modelo);

                if (veiculos.Any())
                {
                    return Ok(veiculos);
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
                var veiculo = veiculoIForm.Veiculo;

                var arquivos = veiculoIForm.FormFile;

                if (!_util.ExtensaoValida(arquivos))
                {
                    return BadRequest("Uma ou mais imagens possuem formato invalido");
                }

                veiculo.Arquivos = _util.CriarListaArquivos(arquivos, veiculo);

                var result = await _veiculoService.CreateVeiculoService(veiculo);

                if (result)
                {
                    await _util.SalvarImagem(arquivos, veiculo.Arquivos.ToList());

                    return Ok("Veiculo cadastrado com sucesso!");
                }

                return BadRequest("Erro ao tentar cadastrar veiculo!");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError
                , $"Erro na aplicação ao tentar cadastrar veiculo. ERRO: {ex.Message}");
            }
        }
    }
}
