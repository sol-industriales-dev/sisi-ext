using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ContratoDTO
    {
        public int id_contrato_empleado { get; set; }
        public int clave_empleado { get; set; }
        public int clave_duracion { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public DateTime? fecha_aplicacion { get; set; }
        public string fecha_aplicacionString { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string desc_duracion { get; set; }
        public bool esNuevoContrato { get; set; }
    }
}
