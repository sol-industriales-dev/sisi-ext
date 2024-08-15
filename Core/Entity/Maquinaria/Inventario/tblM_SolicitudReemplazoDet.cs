using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_SolicitudReemplazoDet
    {
        public int id { get; set; }
        public int AsignacionEquiposID { get; set; }
        public int SolicitudEquipoReemplazoID { get; set; }
        public int estatus { get; set; }
        public string nombreArchivo { get; set; }
        public string ruta { get; set; }
        public string Comentario { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public virtual tblM_SolicitudReemplazoEquipo SolicitudEquipoReemplazo { get; set; }
        public virtual tblM_AsignacionEquipos AsignacionEquipos { get; set; }
    }
}
