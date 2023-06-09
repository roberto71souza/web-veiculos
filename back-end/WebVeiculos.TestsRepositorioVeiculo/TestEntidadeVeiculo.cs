using System.Collections.Generic;
using System.Linq;
using WebVeiculos.Models.Entities;
using Xunit;
using Xunit.Priority;

namespace WebVeiculos.TestsWebProjetoVeiculo
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestEntidadeVeiculo
    {
        [Theory, Priority(1)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosListData), MemberType = typeof(TestDataGenerator))]
        public void SucessoAoCriarObjetoValido(List<Veiculo> veiculos)
        {
            var results = new List<bool>();

            veiculos.ForEach((veiculo) =>
            {
                results.Add(veiculo.EhValido);
            });

            Assert.True(results.All(r => r == true));
        }

        [Theory, Priority(2)]
        [MemberData(nameof(TestDataGenerator.GetVeiculosListData), MemberType = typeof(TestDataGenerator))]
        public void ErroAoCriarObjeto(List<Veiculo> veiculos)
        {
            var results = new List<bool>();

            var veiculo = veiculos[1];

            veiculo.UpdateEntidadeVeiculo(new Veiculo(veiculo.Id, veiculo.NomeProprietario,
                                          veiculo.ModeloVeiculo, "", veiculo.AnoVeiculo, "", veiculo.Estado, veiculo.Cidade, veiculo.InformacoesGerais));

            veiculos.ForEach((veiculo) =>
            {
                results.Add(veiculo.EhValido);
            });

            Assert.False(results.All(r => r == true));
            Assert.Equal(1, veiculos.FindIndex(r => r.EhValido == false));
        }

    }
}
