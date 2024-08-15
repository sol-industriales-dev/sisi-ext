using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class CuadroComparativoReporteDTO
    {
        public string cc { get; set; }
        public string folioCuadroComparativo { get; set; }
        public string fechaCuadro { get; set; }
        public string fechaActual { get; set; }
        public string proveedor1 { get; set; }
        public string proveedor2 { get; set; }
        public string proveedor3 { get; set; }
        public string subTotalProv1 { get; set; }
        public string subTotalProv2 { get; set; }
        public string subTotalProv3 { get; set; }
        public string descuentoProv1 { get; set; }
        public string descuentoProv2 { get; set; }
        public string descuentoProv3 { get; set; }
        public string total1Prov1 { get; set; }
        public string total1Prov2 { get; set; }
        public string total1Prov3 { get; set; }
        public string ivaProv1 { get; set; }
        public string ivaProv2 { get; set; }
        public string ivaProv3 { get; set; }
        public string total2Prov1 { get; set; }
        public string total2Prov2 { get; set; }
        public string total2Prov3 { get; set; }
        public string fletesProv1 { get; set; }
        public string fletesProv2 { get; set; }
        public string fletesProv3 { get; set; }
        public string gastosProv1 { get; set; }
        public string gastosProv2 { get; set; }
        public string gastosProv3 { get; set; }
        public string granTotalProv1 { get; set; }
        public string granTotalProv2 { get; set; }
        public string granTotalProv3 { get; set; }
        public string labProv1 { get; set; }
        public string labProv2 { get; set; }
        public string labProv3 { get; set; }
        public string pagoProv1 { get; set; }
        public string pagoProv2 { get; set; }
        public string pagoProv3 { get; set; }
        public string fechaEntregaProv1 { get; set; }
        public string fechaEntregaProv2 { get; set; }
        public string fechaEntregaProv3 { get; set; }
        public string comentarioProv1 { get; set; }
        public string comentarioProv2 { get; set; }
        public string comentarioProv3 { get; set; }

        public string moneda1 { get; set; }
        public string moneda2 { get; set; }
        public string moneda3 { get; set; }

        public List<CuadroComparativoPartidasReporteDTO> partidas { get; set; }
    }
}
