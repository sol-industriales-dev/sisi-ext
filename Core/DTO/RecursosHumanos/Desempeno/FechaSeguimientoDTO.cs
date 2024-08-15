using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class FechaSeguimientoDTO
    {
        public string Titulo { get; set; }
        public string Dias { get; set; }
        public string MensajeFinal { get; set; }
        public string Estatus { get; set; }
        public int EvaluacionId { get; set; }
    }
}