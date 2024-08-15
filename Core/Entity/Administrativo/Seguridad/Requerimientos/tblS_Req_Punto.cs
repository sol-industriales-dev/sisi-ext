using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_Punto
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public VerificacionEnum verificacion { get; set; }
        public decimal porcentaje { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string indice { get; set; }
        public PeriodicidadRequerimientoEnum periodicidad { get; set; }
        public int actividad { get; set; }
        public int condicionante { get; set; }
        public int seccion { get; set; }
        public string codigo { get; set; }
        public int area { get; set; }
        public int requerimientoID { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
