using System.Collections.Generic;

namespace WebVeiculos.Core.DTO_s
{
    public class VeiculoDto
    {
        public int Id { get; set; }
        public string NomeProprietario { get; set; }
        public string ModeloVeiculo { get; set; }
        public string FabricanteVeiculo { get; set; }
        public string AnoVeiculo { get; set; }
        public string CorVeiculo { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string InformacoesGerais { get; set; }
        public ICollection<dynamic> ListaDeErros { get; set; }
        public ICollection<ArquivoDto> Arquivos { get; set; }

        public VeiculoDto()
        {
            Arquivos = new List<ArquivoDto>();
        }
    }
}
