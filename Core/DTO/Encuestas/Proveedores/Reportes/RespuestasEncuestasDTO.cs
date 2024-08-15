using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores.Reportes
{
    public class RespuestasEncuestasDTO
    {
        public decimal Calificacion { get; set; }
        public int tipoPregunta { get; set; }
        public string Pregunta { get; set; }
        public string Comentario { get; set; }
        public string DescripcionTipo { get; set; }
        public DateTime fecha { get; set; }
        public string CalificacionDescripcion { get; set; }
    }

}
