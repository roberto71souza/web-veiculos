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
        public ICollection<string> ListaDeErros { get; set; }
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
            ListaDeErros = new List<string>();
        }

        protected void ValidarEntidade(bool hasError, string mensagemErro)
        {
            if (hasError)
            {
                ListaDeErros.Add(mensagemErro);
            }
        }
    }
}
