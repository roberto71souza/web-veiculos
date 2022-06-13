using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVeiculos.Models.Entities
{
    public abstract class EntitieBase
    {
        public int Id { get; private set; }
        private ICollection<string> _listaDeErros { get; set; }
        public IReadOnlyCollection<string> ListaDeErros { get { return _listaDeErros.ToList(); } }
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
            _listaDeErros = new List<string>();
        }

        protected void ValidarEntidade(bool hasError, string mensagemErro)
        {
            if (hasError)
            {
                _listaDeErros.Add(mensagemErro);
            }
        }

        protected void ClearListaDeErros()
        {
            _listaDeErros.Clear();
        }
    }
}
