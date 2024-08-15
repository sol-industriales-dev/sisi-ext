using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class EvaluacionDTO
    {
        public int id { get; set; }
        public string numeroContrato { get; set; }
        public string descripcioncc { get; set; }
        public string nombre { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int statusAutorizacion { get; set; }
        public int idDiv { get; set; }
        public int idEvaluacion { get; set; }
    }   
}
