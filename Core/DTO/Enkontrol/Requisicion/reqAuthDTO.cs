using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class reqAuthDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public int solicito { get; set; }
        public DateTime fecha { get; set; }
        public string ccNom { get; set; }
        public string solNom { get; set; }
        public bool isAuth { get; set; }
        public bool flagCheckBox { get; set; }
        public string cantidadTotal { get; set; }
        public int contieneCancelado { get; set; }
        public bool consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public decimal montoTotal { get; set; }
        public string monedaDesc { get; set; }
        public string PERU_tipoRequisicion { get; set; }
    }
}
