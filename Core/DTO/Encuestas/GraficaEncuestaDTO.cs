using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class GraficaEncuestaDTO
    {
        public int preguntaID { get; set; }
        public string preguntaDescripcion { get; set; }
        public decimal calificacion { get; set; }
    }
}
