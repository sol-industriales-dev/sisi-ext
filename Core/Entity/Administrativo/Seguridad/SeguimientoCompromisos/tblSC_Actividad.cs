using Core.Enum.SeguimientoCompromisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoCompromisos
{
    public class tblSC_Actividad
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int area { get; set; }
        public int areaEvaluadora { get; set; }
        public ClasificacionActividadSCEnum clasificacion { get; set; }
        public decimal porcentaje { get; set; }
        public int diasCompromiso { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
