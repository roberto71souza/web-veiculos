using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Core.Services.Implementacao;
using WebVeiculos.Models.Repositories;
using WebVeiculos.Models.Repositories.Implementacao;
using Xunit;
using AutoMapper;
using WebVeiculos.Core;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestsServicoVeiculo
    {
        private IConfiguration _configuration { get; set; }
        private ConexaoDbDapper _conexaoDbDapper { get; set; }
        private VeiculoRepository _repository { get; set; }
        private VeiculoService _service { get; set; }
        private List<VeiculoDto> _veiculos { get; set; }
        public TestsServicoVeiculo()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            //Mapeamento
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            _conexaoDbDapper = new ConexaoDbDapper(_configuration);
            _repository = new VeiculoRepository(_conexaoDbDapper);
            _service = new VeiculoService(_repository, mapper);

            _veiculos = new List<VeiculoDto>();

            var v1 = new VeiculoDto()
            {
                Id = 0,
                NomeProprietario = "Roberto de Souza",
                ModeloVeiculo = "Jaguar Xe 2.0",
                FabricanteVeiculo = "Jaguar",
                AnoVeiculo = "2019",
                CorVeiculo = "Verde Limao",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                InformacoesGerais = "........"
            };

            var v2 = new VeiculoDto()
            {
                Id = 0,
                NomeProprietario = "Gilberto de Souza",
                ModeloVeiculo = "Polo",
                FabricanteVeiculo = "Wolkswagem",
                AnoVeiculo = "2004",
                CorVeiculo = "Preto",
                Cidade = "Vitoria",
                Estado = "Bahia",
                InformacoesGerais = "asaasasasa"
            };

            var v3 = new VeiculoDto()
            {
                Id = 0,
                NomeProprietario = "Allan Lima",
                ModeloVeiculo = "Palio",
                FabricanteVeiculo = "Fiat",
                AnoVeiculo = "2007",
                CorVeiculo = "Prata",
                Cidade = "São Paulo",
                Estado = "São Paulo",
                InformacoesGerais = "dasdsac vsadasdsadsa"
            };

            var v4 = new VeiculoDto()
            {
                Id = 0,
                NomeProprietario = "Bernardo Silva",
                ModeloVeiculo = "Vectra GT",
                FabricanteVeiculo = "Chevrolet",
                AnoVeiculo = "2010",
                CorVeiculo = "Preto",
                Cidade = "Piaui",
                Estado = "Parana",
                InformacoesGerais = "........"
            };

            v1.Arquivos = new List<ArquivoDto>();
            v1.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Jag1", NomeArquivo = "Jag-arquivo1", IdVeiculo = 0 });
            v1.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Jag2", NomeArquivo = "Jag-arquivo2", IdVeiculo = 0 });
            v1.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Jag3", NomeArquivo = "Jag-arquivo3", IdVeiculo = 0 });

            v2.Arquivos = new List<ArquivoDto>();
            v2.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Wolk1", NomeArquivo = "Wolk-arquivo1", IdVeiculo = 0 });
            v2.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Wolk2", NomeArquivo = "Wolk-arquivo2", IdVeiculo = 0 });

            v3.Arquivos = new List<ArquivoDto>();
            v3.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Fiat1", NomeArquivo = "Fiat-arquivo1", IdVeiculo = 0 });
            v3.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Fiat2", NomeArquivo = "Fiat-arquivo2", IdVeiculo = 0 });

            v4.Arquivos = new List<ArquivoDto>();
            v4.Arquivos.Add(new ArquivoDto() { Id = 0, Legenda = "Che1", NomeArquivo = "Che-arquivo1", IdVeiculo = 0 });

            _veiculos.AddRange(new List<VeiculoDto>() { v1, v2, v3, v4 });
        }

        [Fact]
        public void SucessoAoCadastrarVeiculoService()
        {
            List<bool> retorno = new List<bool>();

            foreach (var item in _veiculos)
            {
                var result = _service.CreateVeiculoService(item).Result;
                retorno.Add(result);
            }

            Assert.False(retorno.Exists(x => x == false));
        }

        [Fact]
        public void ErroAoCadastrarVeiculoService()
        {
            List<bool> retorno = new List<bool>();

            var veiculo = _veiculos[1];

            veiculo.NomeProprietario = "";

            foreach (var item in _veiculos)
            {
                var result = _service.CreateVeiculoService(item).Result;
                retorno.Add(result);
            }

            Assert.Contains(retorno, x => x == false);
        }

        [Fact]
        public void SucessoAoBuscarTodosOsVeiculos()
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetAllVeiculosService(paginacao).Result;

            Assert.NotEmpty(paginacao.Veiculos);
        }

        [Theory]
        [InlineData("Polo")]
        [InlineData("Fiat")]
        public void SucessoAoBuscarVeiculosPeloModelo(string modelo)
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetVeiculoByModeloService(paginacao, modelo).Result;

            Assert.True(paginacao.Veiculos.Count > 0);
        }

        [Theory]
        [InlineData("Polo xpto")]
        [InlineData("Jaguar xpto")]
        public void ErroAoBuscarVeiculosPeloModelo(string modelo)
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetVeiculoByModeloService(paginacao, modelo).Result;

            Assert.False(paginacao.Veiculos.Count > 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void SucessoAoBuscarVeiculosPeloId(int id)
        {
            var veiculo = _service.GetVeiculoByIdService(id).Result;

            Assert.NotNull(veiculo);
        }

        [Theory]
        [InlineData(55)]
        [InlineData(56)]
        public void ErroAoBuscarVeiculosPeloId(int id)
        {
            var veiculo = _service.GetVeiculoByIdService(id).Result;

            Assert.Null(veiculo);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public void SucessoAoBuscarUltimosVeiculosCadastrados(int qtd)
        {
            var veiculo = _service.GetUltimosVeiculosCadastradosService(qtd).Result;

            Assert.NotEmpty(veiculo);
        }
    }
}
