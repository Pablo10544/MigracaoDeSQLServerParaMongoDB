using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseConverter
{
    class LogAuditoria:IPropriedadesSql, IDisposable
    {
        int id_logauditoria;//PK
         int id_aplicacaoauditada;//FK PRECISA SER 1
        public DateTime CreatedAt;
        public string UserLogin;
        public string ONSSystem;
        public string MachineNameOrigin;
        public string EntitiesChanges;
        public string FeatureName;
        public string Operation;
        public string EntityDescription;
        string nom_executor;

        public LogAuditoria()
        {
          
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public string nomeId()
        {
            return "id_logauditoria";
        }
        public int id()
        {
            return id_logauditoria;
        }
        public void PopulatePropriedades(List<String> propriedades) {
            this.id_logauditoria = Int32.Parse(propriedades[0]);
            this.id_aplicacaoauditada = Int32.Parse(propriedades[1]);
            this.ONSSystem = "SGIPMO";
            this.EntityDescription = propriedades[2];
            this.CreatedAt = DateTime.Parse(propriedades[3]);
            this.FeatureName = propriedades[4];
            this.nom_executor = propriedades[5];
            if (nom_executor.IndexOf("-") != -1)
            {
                this.MachineNameOrigin = nom_executor.Substring(0, nom_executor.IndexOf("-")-1);
                this.UserLogin = nom_executor.Substring(nom_executor.IndexOf("-")+2);
            }
            else {
                this.MachineNameOrigin = nom_executor;
                this.UserLogin = nom_executor;
            }
            this.Operation = propriedades[6];
            this.EntitiesChanges = propriedades[7];
        }

    }
}
