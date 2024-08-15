using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblAlm_Insumo
    {
        public int id { get; set; }
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public DateTime fechaAlta { get; set; }
        public int bloqueado { get; set; }
        public string cancelado { get; set; }
        public string fijar_precio { get; set; }
        public decimal precio_a_fijar { get; set; }
        public string validar_lista_precios { get; set; }
        public string bit_pt { get; set; }
        public string bit_mp { get; set; }
        public string bit_factura { get; set; }
        public decimal tolerancia { get; set; }
        public int color_resguardo { get; set; }
        public string bit_rotacion { get; set; }
        public int bit_area_cta { get; set; }
        public string bit_af { get; set; }
        public string codigo_barras { get; set; }
        public int id_modelo_maquinaria { get; set; }
        public string modeloMaquinariaDesc { get; set; }
        public int compras_req { get; set; }
        public bool estatus { get; set; }
    }
}
