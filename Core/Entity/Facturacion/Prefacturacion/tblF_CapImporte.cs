using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Facturacion.Prefacturacion
{
    public class tblF_CapImporte
    {
        public int id { get; set; }
        public int idReporte { get; set; }
        public string Label { get; set; }
        public string Valor { get; set; }
        public int Renglon { get; set; }
    }
}
