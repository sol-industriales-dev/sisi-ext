using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class DetallePuestoDTO
    {
        public string nombrePersonal { get; set; }

        public decimal hrsPreventivo { get; set; }
        public decimal hrsPredictivo { get; set; }
        public decimal hrsCorrectivo { get; set; }
        public decimal cantidadOT { get; set; }
        public decimal totalhrsOT { get; set; }

    }
}
