using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.conciliacion
{
    public class ConciliacionFacturadoDTO
    {
        public int id { get; set; }
        public string folio { get; set; }
        public int ccID { get; set; }
        public string cc { get; set; }
        public DateTime fechaInicioRaw { get; set; }
        public string fechaInicio { get; set; }
        public DateTime fechaFinRaw { get; set; }
        public string fechaFin { get; set; }
        public decimal importe { get; set; }
        public string facturas { get; set; }
    }
}
