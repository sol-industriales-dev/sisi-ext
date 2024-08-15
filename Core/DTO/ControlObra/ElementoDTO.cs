using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ElementoDTO
    {
        public int contrato_id { get; set; }
        public int asignacion_id { get; set; }
        public int evaluacion_id { get; set; }
        public int elemento_id { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string mensaje { get; set; }
        public List<RequerimientoDTO> requerimientos { get; set; }
    }
}
