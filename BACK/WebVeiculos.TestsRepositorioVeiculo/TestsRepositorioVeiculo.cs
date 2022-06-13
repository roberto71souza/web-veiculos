using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Repositories;
using WebVeiculos.Models.Repositories.Implementacao;
using Xunit;

namespace WebVeiculos.TestsRepositorioVeiculo
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
            var v1 = new Veiculo(0, "Roberto de Souza", "Jaguar Xe 2.0", "Jaguar", "2019", "Verde Limao", "S�o Paulo", "S�o Paulo", "........");
            var v2 = new Veiculo(0, "Gilberto de Souza", "Polo", "Wolkswagem", "2004", "Preto", "Vitoria", "Bahia", "asaasasasa");
            var v3 = new Veiculo(0, "Allan Lima", "Palio", "Fiat", "2007", "Prata", "S�o Paulo", "S�o Paulo", "dasdsac vsadasdsadsa");
            var v4 = new Veiculo(0, "Bernardo Silva", "Vectra GT", "Chevrolet", "2010", "Preto", "Piaui", "Parana", "........");

            v1.Arquivos = new List<Arquivo>();
            v1.Arquivos.Add(new Arquivo(0, "Jag1", 0));
            v1.Arquivos.Add(new Arquivo(0, "Jag2", 0));
            v1.Arquivos.Add(new Arquivo(0, "Jag3", 0));

            v2.Arquivos = new List<Arquivo>();
            v2.Arquivos.Add(new Arquivo(0, "Wolk1", 0));
            v2.Arquivos.Add(new Arquivo(0, "Wolk2", 0));

            v3.Arquivos = new List<Arquivo>();
            v3.Arquivos.Add(new Arquivo(0, "Fia1", 0));
            v3.Arquivos.Add(new Arquivo(0, "Fia2", 0));

            v4.Arquivos = new List<Arquivo>();
            v4.Arquivos.Add(new Arquivo(0, "Che1", 0));

            _veiculos.AddRange(new List<Veiculo>() { v1, v2, v3, v4 });
        }

        [Fact]
        public void SucessoAoCadastrarVeiculos()
        {
            List<bool> retorno = new List<bool>();

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
        public void SucessoAoBuscarTodosOsCarros()
        {
            var veiculo = _repository.GetAllVeiculos().Result;

            Assert.NotNull(veiculo);
        }

        [Theory]
        [InlineData("Polo")]
        [InlineData("Jaguar")]
        public void SucessoAoBuscarCarrosPeloModelo(string modelo)
        {
            var veiculo = _repository.GetVeiculoByModelo(modelo).Result;

            Assert.True(veiculo.Count > 0);
        }

        [Theory]
        [InlineData("Polos")]
        [InlineData("Jaguar xpto")]
        public void ErroAoBuscarCarrosPeloModelo(string modelo)
        {
            var veiculo = _repository.GetVeiculoByModelo(modelo).Result;

            Assert.False(veiculo.Count > 0);
        }

        [Theory]
        [InlineData(69)]
        [InlineData(71)]
        public void SucessoAoBuscarCarrosPeloId(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.NotNull(veiculo);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(35)]
        public void ErroAoBuscarCarrosPeloId(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.Null(veiculo);
        }
    }
}
