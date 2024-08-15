using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class sb_movtran_chequeraDTO
    {
        public int banco { get; set; }
        public int cuenta { get; set; }
        public DateTime fecha_mov { get; set; }
        public int tm { get; set; }
        public string concepto { get; set; }
        public int numero { get; set; }
        public decimal monto { get; set; }
        public string st_concilia { get; set; }
        public int num_concilia { get; set; }
    }
}
