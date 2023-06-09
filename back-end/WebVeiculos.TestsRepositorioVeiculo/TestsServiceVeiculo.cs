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
using WebVeiculos.Core.Helper;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Xunit.Priority;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestsServiceVeiculo
    {
        private IConfiguration _configuration { get; set; }
        private ConexaoDbDapper _conexaoDbDapper { get; set; }
        private VeiculoRepository _repository { get; set; }
        private VeiculoService _service { get; set; }

        public TestsServiceVeiculo()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            var _mapper = mappingConfig.CreateMapper();

            _conexaoDbDapper = new ConexaoDbDapper(_configuration);
            _repository = new VeiculoRepository(_conexaoDbDapper);
            _service = new VeiculoService(_repository, _mapper, new Util());
        }

        [Theory, Priority(1)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosDtoFilesListData), parameters: true, MemberType = typeof(TestDataGenerator))]
        public void SucessoAoCadastrarVeiculoService(List<VeiculoDto> veiculoDto, List<IFormFile> listFile)
        {
            var retorno = new List<bool>();

            foreach (var veiculo in veiculoDto)
            {
                var result = _service.CreateVeiculoService(listFile, veiculo).Result;

                retorno.Add(result.Item1);
            }

            Assert.False(retorno.Exists(x => x == false));
        }

        [Theory, Priority(2)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosDtoFilesListData), parameters: false, MemberType = typeof(TestDataGenerator))]
        public void ErroAoCadastrarVeiculoService(List<VeiculoDto> veiculoDto, List<IFormFile> listFile)
        {
            var retorno = new List<bool>();

            veiculoDto[2].NomeProprietario = "";

            foreach (var veiculo in veiculoDto)
            {
                var result = _service.CreateVeiculoService(listFile, veiculo).Result;
                retorno.Add(result.Item1);
            }

            Assert.All(retorno, x => x.Equals(false));
        }

        [Fact, Priority(3)]
        public void SucessoAoBuscarTodosOsVeiculosService()
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetAllVeiculosService(paginacao).Result;

            Assert.NotEmpty(paginacao.Veiculos);
        }

        [Theory, Priority(4)]
        [InlineData("Polo")]
        [InlineData("Palio")]
        public void SucessoAoBuscarVeiculosPeloModeloService(string modelo)
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetVeiculoByModeloService(paginacao, modelo).Result;

            Assert.True(paginacao.Veiculos.Any());
        }

        [Theory, Priority(5)]
        [InlineData("Polo xpto")]
        [InlineData("Jaguar xpto")]
        public void ErroAoBuscarVeiculosPeloModeloService(string modelo)
        {
            var paginacao = new PaginacaoListDto();

            paginacao.PaginaAtual = 1;

            paginacao = _service.GetVeiculoByModeloService(paginacao, modelo).Result;

            Assert.False(paginacao.Veiculos.Count > 0);
        }

        [Theory, Priority(6)]
        [InlineData(1)]
        [InlineData(3)]
        public void SucessoAoBuscarVeiculosPeloIdService(int id)
        {
            var veiculo = _service.GetVeiculoByIdService(id).Result;

            Assert.NotNull(veiculo);
        }

        [Theory, Priority(7)]
        [InlineData(55)]
        [InlineData(56)]
        public void ErroAoBuscarVeiculosPeloIdService(int id)
        {
            var veiculo = _service.GetVeiculoByIdService(id).Result;

            Assert.Null(veiculo);
        }

        [Theory, Priority(8)]
        [InlineData(5)]
        [InlineData(3)]
        public void SucessoAoBuscarUltimosVeiculosCadastradosService(int qtd)
        {
            var veiculo = _service.GetUltimosVeiculosCadastradosService(qtd).Result;

            Assert.NotEmpty(veiculo);
        }
    }
}
