using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_Surtido
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public int insumo { get; set; }
        public decimal cantidadTotal { get; set; }
        public bool estatus { get; set; }
        public string tipo { get; set; }

        public virtual List<tblCom_SurtidoDet> surtido_detalle { get; set; }
    }
}
