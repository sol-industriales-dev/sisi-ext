using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.ControlObra
{
    public class tblCO_Capitulos
    {
        public int id { get; set; }
        public string capitulo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int? periodoFacturacion { get; set; }
        public bool estatus { get; set; }

        public int? cc_id { get; set; }
        public virtual tblP_CC cc { get; set; }

        public int? autorizante_id { get; set; }
        public virtual tblP_Usuario usuario { get; set; }

        public virtual List<tblCO_Subcapitulos_Nivel1> subcapitulos_N1 { get; set; }
        public virtual List<tblCO_Actividades_Avance> actividad_avance { get; set; }
        public virtual List<tblCO_Actividades_Facturado> actividad_facturado { get; set; }
    }
}
