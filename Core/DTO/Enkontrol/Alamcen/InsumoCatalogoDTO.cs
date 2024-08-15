using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class InsumoCatalogoDTO
    {
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public int? modeloMaquinaria { get; set; }
        public string modeloMaquinariaDesc { get; set; }
        public int unidad { get; set; }
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

        public string PERU_insumo { get; set; }
        public string PERU_grupo { get; set; }
        public string PERU_estado { get; set; }
        public string PERU_codigo2 { get; set; }

        public int numeroRenglon { get; set; }
    }
}
