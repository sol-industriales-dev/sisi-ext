using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class EvaluacionDTO
    {
        public int? Id { get; set; }
        public int ProcesoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsEliminacion { get; set; }
    }
}