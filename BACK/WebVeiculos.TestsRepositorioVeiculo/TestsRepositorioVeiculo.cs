using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Entities.Paginacao;
using WebVeiculos.Models.Repositories;
using WebVeiculos.Models.Repositories.Implementacao;
using Xunit;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestsRepositorioVeiculo
    {
        private IConfiguration _configuration { get; set; }
        private ConexaoDbDapper _conexaoDbDapper { get; set; }
        private VeiculoRepository _repository { get; set; }
        private List<Veiculo> _veiculos { get; set; }

        public TestsRepositorioVeiculo()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _conexaoDbDapper = new ConexaoDbDapper(_configuration);
            _repository = new VeiculoRepository(_conexaoDbDapper);

            _veiculos = new List<Veiculo>();
            var v1 = new Veiculo(0, "Roberto de Souza", "Jaguar Xe 2.0", "Jaguar", "2019", "Verde Limao", "São Paulo", "São Paulo", "........");
            var v2 = new Veiculo(0, "Gilberto Castro", "Polo", "Wolkswagem", "2004", "Preto", "Vitoria", "Bahia", "........");
            var v3 = new Veiculo(0, "Allan Lima", "Palio", "Fiat", "2007", "Prata", "São Paulo", "São Paulo", "........");
            var v4 = new Veiculo(0, "Bernardo Silva", "Vectra GT", "Chevrolet", "2010", "Preto", "Piaui", "Parana", "........");

            v1.Arquivos = new List<Arquivo>();
            v1.Arquivos.Add(new Arquivo(0, "Jag1", "Jag-arquivo-1", 1));
            v1.Arquivos.Add(new Arquivo(0, "Jag2", "Jag-arquivo-2", 1));
            v1.Arquivos.Add(new Arquivo(0, "Jag3", "Jag-arquivo-3", 1));

            v2.Arquivos = new List<Arquivo>();
            v2.Arquivos.Add(new Arquivo(0, "Wolk1", "Wolk-arquivo-1", 2));
            v2.Arquivos.Add(new Arquivo(0, "Wolk2", "Wolk-arquivo-2", 2));

            v3.Arquivos = new List<Arquivo>();
            v3.Arquivos.Add(new Arquivo(0, "Fiat1", "Fiat-arquivo-1", 3));
            v3.Arquivos.Add(new Arquivo(0, "Fiat2", "Fiat-arquivo-2", 3));

            v4.Arquivos = new List<Arquivo>();
            v4.Arquivos.Add(new Arquivo(0, "Che1", "Che-arquivo-1", 4));

            _veiculos.AddRange(new List<Veiculo>() { v1, v2, v3, v4 });
        }

        [Fact]
        public void SucessoAoCadastrarVeiculos()
        {
            var retorno = new List<bool>();

            foreach (var item in _veiculos)
            {
                var result = _repository.CreateVeiculo(item).Result;
                retorno.Add(result);
            }

            Assert.False(retorno.Exists(x => x == false));
        }

        [Fact]
        public void ErroAoCadastrarVeiculos()
        {
            List<bool> retorno = new List<bool>();

            var veiculo = _veiculos[1];

            veiculo.UpdateEntidadeVeiculo(new Veiculo(veiculo.Id, "", veiculo.ModeloVeiculo,
            veiculo.FabricanteVeiculo, veiculo.AnoVeiculo, veiculo.CorVeiculo, veiculo.Estado, veiculo.Cidade, veiculo.InformacoesGerais));

            foreach (var item in _veiculos)
            {
                var result = _repository.CreateVeiculo(item).Result;
                retorno.Add(result);
            }

            Assert.Contains(retorno, x => x == false);
        }

        [Fact]
        public void SucessoAoBuscarTodosOsVeiculos()
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;

            var paginacaoList = _repository.GetAllVeiculos(paginacao).Result;

            Assert.NotEmpty(paginacaoList.Veiculos);
        }

        [Theory]
        [InlineData("Polo")]
        [InlineData("fiat")]
        public void SucessoAoBuscarVeiculosPeloModelo(string modelo)
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;

            paginacao = _repository.GetVeiculoByModelo(paginacao, modelo).Result;

            Assert.True(paginacao.Veiculos.Count > 0);
        }

        [Theory]
        [InlineData("Polo xpto")]
        [InlineData("Jaguar xpto")]
        public void ErroAoBuscarVeiculosPeloModelo(string modelo)
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;
            paginacao = _repository.GetVeiculoByModelo(paginacao, modelo).Result;

            Assert.False(paginacao.Veiculos.Count > 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void SucessoAoBuscarVeiculosPeloId(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.NotNull(veiculo);
        }

        [Theory]
        [InlineData(55)]
        [InlineData(56)]
        public void ErroAoBuscarVeiculosPeloId(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.Null(veiculo);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public void SucessoAoBuscarUltimosVeiculosCadastrados(int qtd)
        {
            var veiculo = _repository.GetUltimosVeiculosCadastrados(qtd).Result;

            Assert.NotEmpty(veiculo);
        }
    }
}
