using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Evaluacion
{
    public class ActividadDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal ponderacion { get; set; }
        public bool estatus { get; set; }

        public bool aplica { get; set; }
        public PeriodicidadEnum periodicidad { get; set; }

        //Propiedades del Dashboard
        public decimal cantidadProgramada { get; set; }
        public decimal cantidadRealizada { get; set; }
        public decimal cantidadAprobada { get; set; }
        public decimal porcentajeCumplido { get; set; }
        public int cantidadDiasTrabajados { get; set; }
    }
}
