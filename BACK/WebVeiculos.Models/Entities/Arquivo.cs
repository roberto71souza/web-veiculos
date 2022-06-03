using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Models.Entities
{
    public class Arquivo : EntitieBase
    {

        public string Legenda { get; set; }
        public int IdVeiculo { get; set; }
        public Veiculo Veiculo { get; set; }
        public Arquivo(string legenda, int idVeiculo)
        {
            ValidarEntidadeVeiculo(legenda, idVeiculo);
            Legenda = legenda;
            IdVeiculo = idVeiculo;
        }

        private void ValidarEntidadeVeiculo(string legenda, int idVeiculo)
        {
            ValidarEntidade(string.IsNullOrWhiteSpace(legenda), "Legenda, é obrigatorio");

            ValidarEntidade(idVeiculo <= 0, "Id do Veiculo, é obrigatorio");
        }
    }
}
