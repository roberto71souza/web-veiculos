using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Entities.Paginacao;
using WebVeiculos.Models.Repositories;
using WebVeiculos.Models.Repositories.Implementacao;
using Xunit;
using Xunit.Priority;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestsRepositorioVeiculo
    {
        private IConfiguration _configuration { get; set; }
        private ConexaoDbDapper _conexaoDbDapper { get; set; }
        private VeiculoRepository _repository { get; set; }

        public TestsRepositorioVeiculo()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            _conexaoDbDapper = new ConexaoDbDapper(_configuration);
            _repository = new VeiculoRepository(_conexaoDbDapper);
        }

        [Theory, Priority(1)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosFileListData), parameters: true, MemberType = typeof(TestDataGenerator))]
        public void SucessoAoCadastrarVeiculosRepositorio(List<Veiculo> listVeiculos, List<IFormFile> listFile)
        {
            var retorno = new List<bool>();

            foreach (var veiculo in listVeiculos)
            {
                veiculo.AddArquivos(listFile);

                var result = _repository.CreateVeiculo(veiculo).Result;
                retorno.Add(result);
            }

            Assert.False(retorno.Exists(x => x == false));
        }

        [Theory, Priority(2)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosFileListData), parameters: false, MemberType = typeof(TestDataGenerator))]
        public void ErroAoCadastrarVeiculosRepositorio(List<Veiculo> listVeiculos, List<IFormFile> listFile)
        {
            var retorno = new List<bool>();

            var veiculoItem = listVeiculos[1];

            veiculoItem.UpdateEntidadeVeiculo(new Veiculo(veiculoItem.Id, "", veiculoItem.ModeloVeiculo, veiculoItem.FabricanteVeiculo, veiculoItem.AnoVeiculo,
                                              veiculoItem.CorVeiculo, veiculoItem.Estado, veiculoItem.Cidade, veiculoItem.InformacoesGerais));

            foreach (var veiculo in listVeiculos)
            {
                veiculo.AddArquivos(listFile);
                var result = _repository.CreateVeiculo(veiculo).Result;
                retorno.Add(result);
            }

            Assert.DoesNotContain(retorno, x => x == true);
        }

        [Fact, Priority(3)]
        public void SucessoAoBuscarTodosOsVeiculosRepositorio()
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;

            var paginacaoList = _repository.GetAllVeiculos(paginacao).Result;

            Assert.NotEmpty(paginacaoList.Veiculos);
        }

        [Theory, Priority(4)]
        [InlineData("Polo")]
        [InlineData("Vectra")]
        public void SucessoAoBuscarVeiculosPeloModeloRepositorio(string modelo)
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;

            paginacao = _repository.GetVeiculoByModelo(paginacao, modelo).Result;

            Assert.True(paginacao.Veiculos.Any());
        }

        [Theory, Priority(5)]
        [InlineData("Polo xpto")]
        [InlineData("Jaguar xpto")]
        public void ErroAoBuscarVeiculosPeloModeloRepositorio(string modelo)
        {
            var paginacao = new PaginacaoList();

            paginacao.PaginaAtual = 1;
            paginacao = _repository.GetVeiculoByModelo(paginacao, modelo).Result;

            Assert.False(paginacao.Veiculos.Any());
        }

        [Theory, Priority(6)]
        [InlineData(1)]
        [InlineData(3)]
        public void SucessoAoBuscarVeiculosPeloIdRepositorio(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.NotNull(veiculo);
        }

        [Theory, Priority(7)]
        [InlineData(55)]
        [InlineData(56)]
        public void ErroAoBuscarVeiculosPeloIdRepositorio(int id)
        {
            var veiculo = _repository.GetVeiculoById(id).Result;

            Assert.Null(veiculo);
        }

        [Theory, Priority(8)]
        [InlineData(5)]
        [InlineData(3)]
        public void SucessoAoBuscarUltimosVeiculosCadastradosRepositorio(int qtd)
        {
            var veiculo = _repository.GetUltimosVeiculosCadastrados(qtd).Result;

            Assert.NotEmpty(veiculo);
        }

    }
}
