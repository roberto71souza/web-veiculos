using WebVeiculos.Models.Entities;
using Xunit;
using Xunit.Priority;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestEntidadeArquivo
    {
        [Fact, Priority(1)]
        public void SucessoAoCriarObjetoValido()
        {
            var arquivo = new Arquivo(0, "Legenda atual", "arquivo-1.png", 1);

            Assert.True(arquivo.EhValido);
        }

        [Fact, Priority(2)]
        public void ErroAoCriarObjeto()
        {
            var arquivo = new Arquivo(0, "Legenda atual", "arquivo-1.xml", 1);

            Assert.False(arquivo.EhValido);
        }
    }
}
