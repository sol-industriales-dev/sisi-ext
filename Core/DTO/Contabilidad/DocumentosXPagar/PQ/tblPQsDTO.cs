using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.PQ
{
    public class tblPQsDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public string banco { get; set; }
        public DateTime fechaFirma { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string cc { get; set; }
        public string moneda { get; set; }
        public decimal importe { get; set; }
        public decimal importeMN { get; set; }
        public decimal interes { get; set; }
        public decimal interesDiario { get; set; }
        public DateTime fechaCorte { get; set; }
        public decimal interesAcumulado { get; set; }
        public int tipoMovimientoId { get; set; }
        public DateTime? fechaLiquidacion { get; set; }
        public string poliza { get; set; }
        public bool tieneAbono { get; set; }

        //nuevos campos
        public string folioInterno { get; set; }
        public decimal interesSemanal { get; set; }
    }
}
