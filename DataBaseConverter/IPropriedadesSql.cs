using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseConverter
{
    interface IPropriedadesSql
    {
        void PopulatePropriedades(List<String> propriedades);
        int id();
        string nomeId();
    }
}
