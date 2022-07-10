using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Core.DTO_s
{
    public class PaginacaoListDto
    {
        public ICollection<VeiculoDto> Veiculos { get; set; }
        public int PaginaAtual { get; set; }
        public int ItemsPorPagina { get; private set; }
        public int TotalItems { get; set; }
        public int TotalPaginas { get; set; }
        public int BuscarItemsApartir { get; set; }
    }
}
