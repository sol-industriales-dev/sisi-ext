using Core.DTO.Enkontrol.Tablas.Poliza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class PolizaMovPolEkDTO
    {
        public sc_polizasDTO poliza { get; set; }
        public List<sc_movpolDTO> movimientos { get; set; }

        public int idReferencia { get; set; }
    }
}
