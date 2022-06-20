using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Models.Entities
{
    public class Arquivo : EntitieBase
    {
        public string Legenda { get; private set; }
        public string NomeArquivo { get; private set; }
        public int IdVeiculo { get; private set; }
        public Veiculo Veiculo { get; private set; }
        public Arquivo(int id, string legenda, string nomeArquivo, int idVeiculo) : base(id)
        {
            ValidarEntidadeArquivo(legenda, nomeArquivo, idVeiculo);
            Legenda = legenda;
            NomeArquivo = nomeArquivo;
            IdVeiculo = idVeiculo;
        }

        private void ValidarEntidadeArquivo(string legenda, string nomeArquivo, int idVeiculo)
        {
            ValidarEntidade(string.IsNullOrWhiteSpace(legenda), "Legenda, é obrigatorio");
            ValidarEntidade(string.IsNullOrWhiteSpace(nomeArquivo), "Nome do arquivo, é obrigatorio");
            ValidarEntidade(idVeiculo <= 0, "Id do Veiculo, é obrigatorio");
        }

        public void UpdateEntidadeArquivo(Arquivo arquivo)
        {
            ClearListaDeErros();
            ValidarEntidadeArquivo(arquivo.Legenda, arquivo.NomeArquivo, arquivo.IdVeiculo);
            Legenda = arquivo.Legenda;
            IdVeiculo = arquivo.IdVeiculo;
        }
    }
}
