using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ICollection<Arquivo> Arquivos { get; private set; }

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
        }

        private void ValidarEntidadeVeiculo(string nomeProprietario, string modeloVeiculo, string fabricanteVeiculo, string anoVeiculo, string corVeiculo, string estado, string cidade)
        {
            ValidarEntidade(nomeProprietario.Length <= 3, "Nome do proprietario, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(nomeProprietario), "Nome do proprietario, é obrigatorio");

            ValidarEntidade(modeloVeiculo.Length <= 3, "Modelo do Veiculo, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(modeloVeiculo), "Modelo do Veiculo, é obrigatorio");

            ValidarEntidade(fabricanteVeiculo.Length <= 3, "Fabricante do Veiculo, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(fabricanteVeiculo), "Fabricante do Veiculo, é obrigatorio");

            ValidarEntidade(anoVeiculo.Length != 4, "Ano do veiculo, valor informado esta invalido! ex: 1995");

            ValidarEntidade(string.IsNullOrWhiteSpace(anoVeiculo), "Fabricante do Veiculo, é obrigatorio");

            ValidarEntidade(corVeiculo.Length <= 3, "Cor do veiculo, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(corVeiculo), "Cor do veiculo, é obrigatorio");

            ValidarEntidade(estado.Length <= 3, "Estado, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(estado), "Estado, é obrigatorio");

            ValidarEntidade(cidade.Length <= 3, "Cidade, devera ter mais que 3 caracteres");
            ValidarEntidade(string.IsNullOrWhiteSpace(cidade), "Cidade, é obrigatorio");
        }
    }
}
