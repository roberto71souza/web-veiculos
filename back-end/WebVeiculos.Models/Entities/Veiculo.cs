using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace WebVeiculos.Models.Entities
{
    public class Veiculo : EntitieBase
    {
        public string NomeProprietario { get; private set; }
        public string ModeloVeiculo { get; private set; }
        public string FabricanteVeiculo { get; private set; }
        public string AnoVeiculo { get; private set; }
        public string CorVeiculo { get; private set; }
        public string Estado { get; private set; }
        public string Cidade { get; private set; }
        public string InformacoesGerais { get; private set; }
        private List<Arquivo> _arquivos { get; set; }
        public IReadOnlyCollection<Arquivo> Arquivos { get { return _arquivos; } }

        public Veiculo(int id, string nomeProprietario, string modeloVeiculo, string fabricanteVeiculo, string anoVeiculo,
                       string corVeiculo, string estado, string cidade, string informacoesGerais) : base(id)
        {
            ValidarEntidadeVeiculo(nomeProprietario, modeloVeiculo, fabricanteVeiculo, anoVeiculo, corVeiculo, estado, cidade);
            NomeProprietario = nomeProprietario;
            ModeloVeiculo = modeloVeiculo;
            FabricanteVeiculo = fabricanteVeiculo;
            AnoVeiculo = anoVeiculo;
            CorVeiculo = corVeiculo;
            Estado = estado;
            Cidade = cidade;
            InformacoesGerais = informacoesGerais;
            _arquivos = new List<Arquivo>();
        }

        private void ValidarEntidadeVeiculo(string nomeProprietario, string modeloVeiculo, string fabricanteVeiculo, string anoVeiculo, string corVeiculo, string estado, string cidade)
        {
            ValidarEntidade(string.IsNullOrWhiteSpace(nomeProprietario), "Nome do proprietario é obrigatorio", "nomeProprietario");
            ValidarEntidade(nomeProprietario?.Length <= 3, "Nome do proprietario devera ter mais que 3 caracteres", "nomeProprietario");

            ValidarEntidade(string.IsNullOrWhiteSpace(modeloVeiculo), "Modelo do veiculo é obrigatorio", "modeloVeiculo");
            ValidarEntidade(modeloVeiculo?.Length <= 3, "Modelo do veiculo devera ter mais que 3 caracteres", "modeloVeiculo");

            ValidarEntidade(string.IsNullOrWhiteSpace(fabricanteVeiculo), "Fabricante do veiculo é obrigatorio", "fabricanteVeiculo");
            ValidarEntidade(fabricanteVeiculo?.Length <= 3, "Fabricante do veiculo devera ter mais que 3 caracteres", "fabricanteVeiculo");

            ValidarEntidade(string.IsNullOrWhiteSpace(anoVeiculo), "Ano do veiculo é obrigatorio", "anoVeiculo");
            ValidarEntidade(anoVeiculo?.Length != 4, "Valor ano do veiculo informado esta invalido! ex: 1995", "anoVeiculo");

            ValidarEntidade(string.IsNullOrWhiteSpace(corVeiculo), "Cor do veiculo é obrigatorio", "corVeiculo");
            ValidarEntidade(corVeiculo?.Length <= 3, "Cor do veiculo devera ter mais que 3 caracteres", "corVeiculo");

            ValidarEntidade(string.IsNullOrWhiteSpace(estado), "Estado é obrigatorio", "estado");
            ValidarEntidade(estado?.Length <= 3, "Estado devera ter mais que 3 caracteres", "estado");

            ValidarEntidade(string.IsNullOrWhiteSpace(cidade), "Cidade é obrigatorio", "cidade");
            ValidarEntidade(cidade?.Length <= 3, "Cidade devera ter mais que 3 caracteres", "cidade");
        }

        public void UpdateEntidadeVeiculo(Veiculo veiculo)
        {
            ClearListaDeErros();
            ValidarEntidadeVeiculo(veiculo.NomeProprietario, veiculo.ModeloVeiculo, veiculo.FabricanteVeiculo, veiculo.AnoVeiculo, veiculo.CorVeiculo, veiculo.Estado, veiculo.Cidade);
            NomeProprietario = veiculo.NomeProprietario;
            ModeloVeiculo = veiculo.ModeloVeiculo;
            FabricanteVeiculo = veiculo.FabricanteVeiculo;
            AnoVeiculo = veiculo.AnoVeiculo;
            CorVeiculo = veiculo.CorVeiculo;
            Estado = veiculo.Estado;
            Cidade = veiculo.Cidade;
            InformacoesGerais = veiculo.InformacoesGerais;
        }

        public void AddArquivos(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                Random randNum = new Random();

                var extensao = Path.GetExtension(file.FileName);
                var modelo = ModeloVeiculo.Split();
                var proprietario = NomeProprietario.Split();
                var random = randNum.Next(500);
                var nomeArquivo = $"{modelo[0].ToLower()}-{proprietario[0].ToLower()}-{random}{extensao}";

                var arquivo = new Arquivo(0, ModeloVeiculo, nomeArquivo, Id);

                _arquivos.Add(arquivo);
            }
        }

        public void AddArquivos(Arquivo arquivo)
        {
            _arquivos.Add(arquivo);
        }
    }
}
