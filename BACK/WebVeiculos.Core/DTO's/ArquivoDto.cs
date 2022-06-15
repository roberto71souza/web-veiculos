using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Core.DTO_s
{
    public class ArquivoDto
    {
        public int Id { get; set; }
        public string Legenda { get; set; }
        public int IdVeiculo { get; set; }
        public VeiculoDto Veiculo { get; set; }
    }
}
