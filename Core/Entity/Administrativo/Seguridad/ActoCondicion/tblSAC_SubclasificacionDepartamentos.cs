using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.ActoCondicion
{
    public class tblSAC_SubclasificacionDepartamentos
    {
        public int id { get; set; }
        public string subclasificacionDep { get; set; }
        public int idDepartamento { get; set; }
        public bool registroActivo { get; set; }
    }
}
