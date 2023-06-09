using System.Collections.Generic;
using System.Linq;

namespace WebVeiculos.Models.Entities
{
    public abstract class EntitieBase
    {
        public int Id { get; private set; }
        private ICollection<dynamic> _listaDeErros { get; set; }
        public IReadOnlyCollection<dynamic> ListaDeErros { get { return _listaDeErros.ToList(); } }
        public bool EhValido
        {
            get
            {
                if (ListaDeErros.Count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        public EntitieBase(int id)
        {
            Id = id;
            _listaDeErros = new List<dynamic>();
        }

        protected void ValidarEntidade(bool hasError, string mensagemErro, string propriedade)
        {
            if (hasError)
            {
                _listaDeErros.Add(new { propriedade = propriedade, erro = mensagemErro });
            }
        }

        protected void ClearListaDeErros()
        {
            _listaDeErros.Clear();
        }
    }
}
