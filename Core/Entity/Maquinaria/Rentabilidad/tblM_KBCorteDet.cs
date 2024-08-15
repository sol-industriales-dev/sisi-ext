using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Rentabilidad
{
    public class tblM_KBCorteDet
    {
        public int id { get; set; }
        public int corteID { get; set; }
        public string poliza { get; set; }
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public DateTime fechapol { get; set; }
        public int tipoEquipo { get; set; }
        public string referencia { get; set; }
        public int empresa { get; set; }
        public int linea { get; set; }
    }
}
