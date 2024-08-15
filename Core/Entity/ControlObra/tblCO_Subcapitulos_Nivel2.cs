using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Subcapitulos_Nivel2
    {
        public int id { get; set; }
        public string subcapitulo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool estatus { get; set; }

        public int subcapituloN1_id { get; set; }
        public virtual tblCO_Subcapitulos_Nivel1 subcapitulos_N1 { get; set; }

        public virtual List<tblCO_Subcapitulos_Nivel3> subcapitulos_N3 { get; set; }
        public virtual List<tblCO_Actividades> actividades { get; set; }
    }
}