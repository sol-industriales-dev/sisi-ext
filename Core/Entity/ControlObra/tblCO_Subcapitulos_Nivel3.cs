using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Subcapitulos_Nivel3
    {
        public int id { get; set; }
        public string subcapitulo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool estatus { get; set; }

        public int subcapituloN2_id { get; set; }
        public virtual tblCO_Subcapitulos_Nivel2 subcapitulo_N2 { get; set; }

        public virtual List<tblCO_Actividades> actividades { get; set; }

    }
}
