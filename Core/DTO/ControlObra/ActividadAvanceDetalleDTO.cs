using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ActividadAvanceDetalleDTO
    {
        public int id { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public DateTime fechaI { get; set; }
        public DateTime fechaF { get; set; }
        public decimal cantidadAvance { get; set; }
        public bool estatus { get; set; }
        public int actividad_id { get; set; }
    }
}
