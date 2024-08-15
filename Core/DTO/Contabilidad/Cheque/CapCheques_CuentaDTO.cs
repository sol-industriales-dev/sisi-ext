using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class CapCheques_CuentaDTO
    {
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public int banco { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string tp { get; set; }
        public string sucursal { get; set; }
        public int ultimo_cheque { get; set; }
        public string bancoDescripcion { get; set; }
        public int ult_cheq_electronico { get; set; }
        public int d { get; set; }
    }
}
