using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class contratoDTO
    {
        public decimal tipoCambio { get; set; }
        public DateTime fechaContrato { get; set; }
        public string folioContrato { get; set; }
        public string descripcion { get; set; }
        public string institucion { get; set; }
    }
}
