using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_CatPeriodo
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int tipoNomina { get; set; }
        public int periodo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime fechaLimite { get; set; }
        public int usuarioCaptura { get; set; }
        public bool estatus { get; set; }
        
    }
}
