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
        public int IdVeiculo { get; private set; }
        public Veiculo Veiculo { get; private set; }
        public Arquivo(int id, string legenda, int idVeiculo) : base(id)
        {
            ValidarEntidadeArquivo(legenda, idVeiculo);
            Legenda = legenda;
            IdVeiculo = idVeiculo;
        }

        private void ValidarEntidadeArquivo(string legenda, int idVeiculo)
        {
            ValidarEntidade(string.IsNullOrWhiteSpace(legenda), "Legenda, é obrigatorio");

            ValidarEntidade(idVeiculo <= 0, "Id do Veiculo, é obrigatorio");
        }

        public void UpdateEntidadeArquivo(Arquivo arquivo)
        {
            ClearListaDeErros();
            ValidarEntidadeArquivo(arquivo.Legenda, arquivo.IdVeiculo);
            Legenda = arquivo.Legenda;
            IdVeiculo = arquivo.IdVeiculo;
        }
    }
}
