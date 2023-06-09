using System.Collections.Generic;

namespace WebVeiculos.Core.DTO_s
{
    public class ArquivoDto
    {
        public int Id { get; set; }
        public string Legenda { get; set; }
        public string NomeArquivo { get; set; }
        public int IdVeiculo { get; set; }
        public ICollection<dynamic> ListaDeErros { get; set; }
        public VeiculoDto Veiculo { get; set; }
    }
}
