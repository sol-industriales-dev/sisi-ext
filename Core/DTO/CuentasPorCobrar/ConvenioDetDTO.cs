using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.CuentasPorCobrar
{
    public class ConvenioDetDTO
    {
        public int id { get; set; }
        public int? idAcuerdo { get; set; }
        public decimal abonoDet { get; set; }
        public DateTime fechaDet { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool esActivo { get; set; }
    }
}
