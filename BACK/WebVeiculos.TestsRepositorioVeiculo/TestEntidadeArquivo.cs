using WebVeiculos.Models.Entities;
using Xunit;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestEntidadeArquivo
    {
        [Fact]
        public void SucessoAoCriarObjetoValido()
        {
            var arquivo = new Arquivo(0, "Legenda atual", "arquivo-1", 1);

            Assert.True(arquivo.EhValido);
        }

        [Fact]
        public void ErroAoCriarObjeto()
        {
            var arquivo = new Arquivo(0, "Legenda atual", "", 1);

            Assert.False(arquivo.EhValido);
        }
    }
}
