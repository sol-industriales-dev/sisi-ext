using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Remision_Retencion
    {
        public int id { get; set; }
        public int remision { get; set; }
        public decimal insumo { get; set; }
        public int partida { get; set; }
        public decimal cantidad { get; set; }
        public decimal porc_ret { get; set; }
        public decimal importe { get; set; }
        public int calc_iva { get; set; }
        public int calc_iva_factura { get; set; }
        public int cia_sucursal { get; set; }
        public decimal ret_iva { get; set; }
        public int pedido { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
