using WebVeiculos.Models.Entities;
using Xunit;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestEntidadeVeiculo
    {
        [Fact]
        public void SucessoAoCriarObjetoValido()
        {
            var veiculo = new Veiculo(0, "Roberto", "Polo", "Wolkswagen", "2007", "Preto", "São Paulo", "São Paulo", "");

            Assert.True(veiculo.EhValido);
        }

        [Fact]
        public void ErroAoCriarObjeto()
        {
            var veiculo = new Veiculo(0, "Roberto de Souza", "Polo", "Wolkswagen", "", "Preto", "São Paulo", "São Paulo",
                          "ano 2007, em perfeito estado");

            Assert.False(veiculo.EhValido);
        }

    }
}
