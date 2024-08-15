using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class CuadroComparativoDetDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public string folio { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public string descripcion { get; set; }
        public decimal cantidad { get; set; }
        public decimal cant_ordenada { get; set; }
        public string unidad { get; set; }
        public decimal precio1 { get; set; }
        public string moneda1 { get; set; }
        public decimal precio2 { get; set; }
        public string moneda2 { get; set; }
        public decimal precio3 { get; set; }
        public string moneda3 { get; set; }
        public int? proveedor_uc { get; set; }
        public int? oc_uc { get; set; }
        public DateTime? fecha_uc { get; set; }
        public decimal? precio_uc { get; set; }

        public int? prov1 { get; set; }
        public string nombre_prov1 { get; set; }
        public int? prov2 { get; set; }
        public string nombre_prov2 { get; set; }
        public int? prov3 { get; set; }
        public string nombre_prov3 { get; set; }
        public string partidaDesc { get; set; }
        public string PERU_tipoCuadro { get; set; }
    }
}
