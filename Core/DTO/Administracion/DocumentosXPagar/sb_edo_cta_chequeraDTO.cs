using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class sb_edo_cta_chequeraDTO
    {
        public int cuenta { get; set; }
        public DateTime fecha_mov { get; set; }
        public int tm { get; set; }
        public int numero { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public decimal tc { get; set; }
        public string origen_mov { get; set; }
        public string generada { get; set; }
        public int iyear { get; set; }
        public int imes { get; set; }
        public int ipoliza { get; set; }
        public string itp { get; set; }
        public int ilinea { get; set; }
        public int banco { get; set; }

        public int num_concilia { get; set; }
    }
}
