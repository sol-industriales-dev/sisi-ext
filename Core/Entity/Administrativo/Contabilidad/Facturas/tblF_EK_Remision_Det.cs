using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Remision_Det
    {
        public int id { get; set; }
        public string folioPrefactura { get; set; }
        public int sucursal { get; set; }
        public int remision { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public string unidad { get; set; }
        public decimal cant_remision { get; set; }
        public decimal precio_remision { get; set; }
        public int pedido { get; set; }
        public int ped_part { get; set; }
        public string linea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; } 
    }
}
