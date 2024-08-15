using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class dtProgramacionPagosDTO
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public string folioContrato { get; set; }
        public string fechaVencimiento { get; set; }
        public decimal importeProgramado { get; set; }
        public decimal importeFinal { get; set; }
        public decimal tipoCambio { get; set; }
        public int aplicaPago { get; set; }
        public int estatus { get; set; }
        public int parcialidad { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
    }
}
