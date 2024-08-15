using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class DxPCuentaDTO
    {
        public int institucionId { get; set; }
        public string institucion { get; set; }
        public decimal tasa { get; set; }
        public int monedaId { get; set; }
        public string cc { get; set; }
        public int ctaAbono { get; set; }
        public int sctaAbono { get; set; }
        public int ssctaAbono { get; set; }
        public int ctaCargo { get; set; }
        public int sctaCargo { get; set; }
        public int ssctaCargo { get; set; }
    }
}
