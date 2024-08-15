using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_CuadroComparativoDet
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public int folio { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio1 { get; set; }
        public decimal precio2 { get; set; }
        public decimal precio3 { get; set; }
        public int? proveedor_uc { get; set; }
        public int? oc_uc { get; set; }
        public DateTime? fecha_uc { get; set; }
        public decimal? precio_uc { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public string PERU_tipoCuadro { get; set; }
    }
}
