using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CatalogoCCtblFA_Grupos
    {
        public int id { get; set; }
        public int catalogoCC_id { get; set; }
        public int grupo_id { get; set; }
        public bool registroActivo { get; set; }
    }
}
