using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Bancos
{
    public class sbEdoCtaChequeraDTO
    {
        public string cuenta { get; set; }
        public DateTime fecha_mov { get; set; }
        public int tm { get; set; }
        public int numero { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public int banco { get; set; }
        public string st_che { get; set; }
        public string status_lp { get; set; }
        public int ipoliza { get; set; }
        public string itp { get; set; }
        public int ilinea { get; set; }
        public int naturaleza { get; set; }
    }
}
