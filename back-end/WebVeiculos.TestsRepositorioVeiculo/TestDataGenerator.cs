using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Models.Entities;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestDataGenerator
    {
        private static List<Veiculo> listaVeiculos = new()
        {
          new Veiculo(0, "Roberto de Souza", "Jaguar Xe 2.0", "Jaguar", "2019", "Verde Limao", "São Paulo", "São Paulo", "........"),
          new Veiculo(0, "Gilberto Castro", "Polo", "Wolkswagem", "2004", "Preto", "Vitoria", "Bahia", "........"),
          new Veiculo(0, "Allan Lima", "Palio", "Fiat", "2007", "Prata", "São Paulo", "São Paulo", "........"),
          new Veiculo(0, "Bernardo Silva", "Vectra GT", "Chevrolet", "2010", "Preto", "Piaui", "Parana", "........")
        };

        private static List<VeiculoDto> listaVeiculosDto = new()
        {
          new VeiculoDto() {Id = 0, NomeProprietario = "Roberto de Souza", ModeloVeiculo = "Jaguar Xe 2.0", FabricanteVeiculo = "Jaguar", AnoVeiculo = "2019", CorVeiculo = "Verde Limao", Cidade = "São Paulo", Estado = "São Paulo", InformacoesGerais = "........"},
          new VeiculoDto() {Id = 0, NomeProprietario = "Gilberto de Souza", ModeloVeiculo = "Polo", FabricanteVeiculo = "Wolkswagem", AnoVeiculo = "2004", CorVeiculo = "Preto", Cidade = "Vitoria", Estado = "Bahia", InformacoesGerais = "........"},
          new VeiculoDto() {Id = 0, NomeProprietario = "Allan Lima", ModeloVeiculo = "Palio", FabricanteVeiculo = "Fiat", AnoVeiculo = "2007", CorVeiculo = "Prata", Cidade = "São Paulo", Estado = "São Paulo", InformacoesGerais = "........"}
        };

        private static List<string> caminhoArquivoSucesso = new()
        {
                @"C:\\Users\\rober\\Downloads\\baixados (1).jpeg",
                @"C:\Users\rober\Downloads\baixados.jpeg",
                @"C:\Users\rober\Downloads\LogoMakr.png"
        };

        private static List<string> caminhoArquivoErro = new()
        {
                @"C:\Users\rober\OneDrive\Documentos\1650327701968.pdf",
                @"C:\Users\rober\OneDrive\Documentos\1650465595295.pdf",
                @"C:\Users\rober\Downloads\LogoMakr.png"
        };

        public static List<IFormFile> GetFilesData(bool veiculoValido)
        {
            var pathArquivo = veiculoValido ? caminhoArquivoSucesso : caminhoArquivoErro;

            var listForm = new List<IFormFile>();

            foreach (var path in pathArquivo)
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                var file = new FormFile(stream, 0, stream.Length, Path.GetFileName(stream.Name), Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary()
                };

                listForm.Add(file);
            }
            return listForm;
        }

        public static IEnumerable<object[]> GetVeiculosListData() =>
            new List<object[]>
            {
                new object[]
                {
                    listaVeiculos
                }
            };

        public static IEnumerable<object[]> GetVeiculosDtoListData() =>
            new List<object[]>
            {
                new object[]
                {
                    listaVeiculosDto
                }
            };

        public static IEnumerable<object[]> GetVeiculosFileListData(bool veiculoValido)
        {
            return new List<object[]>
            {
                 new object[]
                 {
                     listaVeiculos,
                     GetFilesData(veiculoValido)
                 }
            };
        }

        public static IEnumerable<object[]> GetVeiculosDtoFilesListData(bool veiculoValido)
        {
            return new List<object[]>
           {
                new object[]
                {
                    listaVeiculosDto,
                    GetFilesData(veiculoValido)
                }
           };
        }
    }
}
