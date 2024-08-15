using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_PagoMensual
    {
        public int id { get; set; }
        public decimal montoMensual { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int capturaUsuarioID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int editaUsuarioID { get; set; }
        public DateTime fechaEdita { get; set; }
        public string areaCuenta { get; set; }

    }
}
