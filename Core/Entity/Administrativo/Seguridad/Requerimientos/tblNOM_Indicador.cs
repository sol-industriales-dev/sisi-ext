using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblNOM_Indicador
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public VerificacionEnum verificacion { get; set; }
        public decimal porcentaje { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string indice { get; set; }
        public PeriodicidadRequerimientoEnum periodicidad { get; set; }
        public int normaID { get; set; }
        public bool estatus { get; set; }
    }
}
