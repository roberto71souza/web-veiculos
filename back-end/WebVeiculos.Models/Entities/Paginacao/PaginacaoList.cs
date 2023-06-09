using System.Collections.Generic;

namespace WebVeiculos.Models.Entities.Paginacao
{
    public class PaginacaoList
    {
        public ICollection<Veiculo> Veiculos{ get; set; }
        public int PaginaAtual { get; set; }
        public int ItemsPorPagina { get { return 5; } }
        public int TotalItems { get; set; }
        public int TotalPaginas { get; set; }
        public int BuscarItemsApartir { get; set; }
    }
}
