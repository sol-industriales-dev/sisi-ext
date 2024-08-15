using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Enkontrol.Compras;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_VoboAutorizacionEspecial
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public int empleado { get; set; }
        public TipoVoboAutorizacionEnum tipo { get; set; }
        public int cantidad_vobos { get; set; }
        public decimal monto_minimo { get; set; }
        public decimal monto_maximo { get; set; }
        public bool registroActivo { get; set; }
    }
}
