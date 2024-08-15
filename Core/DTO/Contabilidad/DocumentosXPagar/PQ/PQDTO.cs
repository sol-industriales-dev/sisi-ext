using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.PQ
{
    public class PQDTO
    {
        public int id { get; set; }
        public int bancoId { get; set; }
        public string ctaRelacion { get; set; }
        public string ctaDescripcion { get; set; }
        public string ctaCargoRelacion { get; set; }
        public string ctaCargoDescripcion { get; set; }
        public DateTime fechaFirma { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string cc { get; set; }
        public int monedaId { get; set; }
        public decimal importe { get; set; }
        public decimal interes { get; set; }
        public decimal? tipoCambio { get; set; }
        public decimal importeMN { get; set; }
    }
}
