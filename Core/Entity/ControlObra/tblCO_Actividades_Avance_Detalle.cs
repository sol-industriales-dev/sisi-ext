using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Actividades_Avance_Detalle
    {
        public int id { get; set; }       
        public decimal cantidadAvance { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public decimal? acumuladoAnterior { get; set; }
        public decimal? acumuladoActual { get; set; }
        public decimal? avancePorcentaje { get; set; }
        public decimal? avanceAcumuladoPorcentaje { get; set; }
        public string observaciones { get; set; }
        public bool estatus { get; set; }
        public decimal? importeAvanceAnt { get; set; }
        public decimal? importeAvancePeriodo { get; set; }
        public decimal? importeAvanceAcumulado { get; set; }

        public int actividad_id { get; set; }
        public virtual tblCO_Actividades actividad { get; set; }

        public int? actividadAvance_id { get; set; }
        public virtual tblCO_Actividades_Avance actividadAvance { get; set; }
    }
}
