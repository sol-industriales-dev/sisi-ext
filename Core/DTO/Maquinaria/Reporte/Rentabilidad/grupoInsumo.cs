using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class grupoInsumo
    {
        public int tipo_insumo { get; set; }
        public int grupo_insumo { get; set; }
        public string descripcion { get; set; }
        public string inventario { get; set; }
        public string tipo_kontrol { get; set; }
        public string valida_ppto { get; set; }
        public string tipo_basicos { get; set; }
        public string transfiere_poliza { get; set; }
        public string valida_ppto_cantidad { get; set; }
        public string valida_ppto_precio { get; set; }
        public string valida_ppto_importe { get; set; }
        public string valida_ppto_lista_precio { get; set; }
        public string bit_ppto { get; set; }
        public string valida_area_cta { get; set; }
        public string valida_af { get; set; }
    }
}
