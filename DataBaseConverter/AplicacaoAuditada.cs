using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseConverter
{
    class AplicacaoAuditada:IPropriedadesSql
    {
        public int id_aplicacao;
        public string cod_aplicacaoauditada;
        public string nom_aplicacaoauditada;

        public AplicacaoAuditada(List<string> propriedades)
        {
            this.id_aplicacao = Int32.Parse(propriedades[0]);
            this.cod_aplicacaoauditada = propriedades[1];
            this.nom_aplicacaoauditada = propriedades[2];
        }
        public AplicacaoAuditada()
        {
           
        }
        public void PopulatePropriedades(List<String> propriedades) {
            this.id_aplicacao = Int32.Parse(propriedades[0]);
            this.cod_aplicacaoauditada = propriedades[1];
            this.nom_aplicacaoauditada = propriedades[2];
        }
        public int id() {
            return id_aplicacao;
        }
        public string nomeId()
        {
            return "id_aplicacaoauditada";
        }

    }
}
    
