using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_AFPtblRH_EK_Empleados
    {
        public int id { get; set; }
        public int afpID { get; set; }
        public int clave_empleado { get; set; }        
        public DateTime fechaRegistro { get; set; }
        public int usuarioRegistro { get; set; }
        public DateTime fechaModifica { get; set; }
        public int usuarioModifica { get; set; }
        public bool aplicaFamiliar { get; set; }
        public bool registroActivo { get; set; }
    }
}
