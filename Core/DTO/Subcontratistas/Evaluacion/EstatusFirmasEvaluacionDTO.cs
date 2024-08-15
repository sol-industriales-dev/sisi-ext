using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class EstatusFirmasEvaluacionDTO
    {
        public string puesto { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public bool estatusFirma { get; set; }
    }
}
