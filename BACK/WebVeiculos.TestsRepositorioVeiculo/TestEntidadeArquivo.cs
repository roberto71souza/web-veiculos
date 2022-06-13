using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using Xunit;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestEntidadeArquivo
    {
        [Fact]
        public void SucessoAoCriarObjetoValido()
        {
            var arquivo = new Arquivo(0, "Legenda atual", 1);

            Assert.True(arquivo.EhValido && (arquivo.ListaDeErros.Count == 0));
        }

        [Fact]
        public void ErroAoCriarObjetoValido()
        {
            var arquivo = new Arquivo(0, "Legenda atual", 0);

            Assert.False(arquivo.EhValido && (arquivo.ListaDeErros.Count > 0));
        }
    }
}
