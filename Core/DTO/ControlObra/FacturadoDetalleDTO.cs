using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class FacturadoDetalleDTO
    {
        public int id { get; set; }
        public int actividad_id { get; set; }
        public decimal volumen { get; set; }
        public decimal importe { get; set; }
        public bool estatus { get; set; }       
    }   
}
