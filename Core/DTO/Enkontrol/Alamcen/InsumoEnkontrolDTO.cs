using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class InsumoEnkontrolDTO
    {
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public int? modeloMaquinaria { get; set; }
        public string modeloMaquinariaDesc { get; set; }
        public string unidad { get; set; }
        public string unidadDesc { get; set; }
        public int tipo { get; set; }
        public string tipoDesc { get; set; }
        public int grupo { get; set; }
        public string grupoDesc { get; set; }
        public decimal tolerancia { get; set; }

        public string validar_lista_precios { get; set; }
        public string bit_pt { get; set; }
        public string bit_mp { get; set; }
        public string bit_factura { get; set; }
        public int? color_resguardo { get; set; }
        public string color_resguardoDesc { get; set; }
        public string cancelado { get; set; }
        public int compras_req { get; set; }

        public DateTime? fechaAlta { get; set; }
        public string fechaAltaString { get; set; }

        public int? almacen { get; set; }
        public int? reportes_internos { get; set; }
        public int? default_ventas { get; set; }
        public int? bloqueado { get; set; }
        public string fijar_precio { get; set; }
        public decimal? precio_a_fijar { get; set; }
        public int? bloqueado_por { get; set; }
        public DateTime? bloqueado_desde { get; set; }
        public DateTime? fecha_mod { get; set; }
        public DateTime? hora_mod { get; set; }
        public int? nivel_calidad { get; set; }
        public string bit_rotacion { get; set; }
        public int? bit_area_cta { get; set; }
        public string bit_af { get; set; }
        public string codigo_barras { get; set; }
        public string bit_devolucion { get; set; }
        public int? id_modelo_maquinaria { get; set; }
        public string ClaveProdServ { get; set; }
        public string claveunidadsat { get; set; }
        public string codigo_gtin { get; set; }
        public string codigoadenda { get; set; }
        public string imprime_ctapredial { get; set; }
    }
}
