using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using Xunit;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    public class TestEntidadeVeiculo
    {
        [Fact]
        public void SucessoAoCriarObjetoValido()
        {
            var veiculo = new Veiculo(0, "Roberto", "Polo", "Wolkswagen", "2007", "Preto", "São Paulo", "São Paulo",
                          "");

            Assert.True(veiculo.EhValido && veiculo.ListaDeErros.Count == 0);
        }

        [Fact]
        public void ErroAoCriarObjetoValido()
        {
            var predicate = new List<bool>();

            var veiculo = new Veiculo(0, "Roberto de Souza", "Polo", "Wolkswagen", "2008", "Preto", "São Paulo", "São Paulo",
                          "ano 2007, em perfeito estado");

            predicate.Add(veiculo.EhValido);
            predicate.Add(veiculo.ListaDeErros.Count == 0 ? true : false);

            Assert.DoesNotContain(predicate, x => x == false);
        }

    }
}
