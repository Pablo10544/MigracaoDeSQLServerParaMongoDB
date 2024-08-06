using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseConverter
{
    class ErroLogAuditoria:IPropriedadesSql
    {
        public int id_errologauditoria;
        public int cod_erro;
        public string dsc_erro;
        public string doc_registrologauditoria;

        public ErroLogAuditoria()
        {
        }
        public int id()
        {
            return id_errologauditoria;
        }
        public string nomeId() {
            return "id_errologauditoria";
        }
        public void PopulatePropriedades(List<String> propriedades) {
            this.id_errologauditoria = Int32.Parse(propriedades[0]);
            this.cod_erro = Int32.Parse(propriedades[1]);
            this.dsc_erro = propriedades[2];
            this.doc_registrologauditoria = propriedades[3];
        }
    }
}
