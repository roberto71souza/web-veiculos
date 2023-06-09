using System;
using System.IO;
using System.Linq;

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
            ValidarEntidade(string.IsNullOrWhiteSpace(legenda), "Legenda é obrigatorio", "legenda");
            ValidarEntidade(string.IsNullOrWhiteSpace(nomeArquivo), "Nome do arquivo é obrigatorio", "nomeArquivo");
            ValidarEntidade(!ExtensaoValida(nomeArquivo), "Uma ou mais imagens possuem formato invalido, formatos validos ex: png, jpeg e jpg", "nomeArquivo");
            ValidarEntidade(idVeiculo <= 0, "Id do Veiculo é obrigatorio", "idVeiculo");
        }

        public void UpdateEntidadeArquivo(Arquivo arquivo)
        {
            ClearListaDeErros();
            ValidarEntidadeArquivo(arquivo.Legenda, arquivo.NomeArquivo, arquivo.IdVeiculo);
            Legenda = arquivo.Legenda;
            IdVeiculo = arquivo.IdVeiculo;
        }

        private bool ExtensaoValida(string nomeArquivo)
        {
            var extensao = Path.GetExtension(nomeArquivo);
            var extensoesValidas = new string[] { ".jpg", ".jpeg", ".png" };

            if (!extensoesValidas.Contains(extensao.ToLower()))
            {
                return false;
            }
            return true;
        }

    }
}
