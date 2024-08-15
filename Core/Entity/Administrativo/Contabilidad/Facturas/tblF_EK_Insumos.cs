using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Insumos
    {
        public int id { get; set; }
        public string insumo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public bool bloqueado { get; set; }
        public string cancelado { get; set; }
        public string fijar_precio { get; set; }
        public decimal precio_a_fijar { get; set; }
        public string validar_lista_precios { get; set; }
        public DateTime fecha_mod { get; set; }
        public string bit_pt { get; set; }
        public string bit_mp { get; set; }
        public string bit_factura { get; set; }
        public decimal tolerancia { get; set; }
        public int? color_Resguardo { get; set; }
        public string bit_rotacion { get; set; }
        public int? bit_area_cta { get; set; }
        public string bit_af { get; set; }
        public string codigo_barras { get; set; }
        public string bit_devolucion { get; set; }
        public string imprime_ctapredial { get; set; }
        public int? compras_req { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
