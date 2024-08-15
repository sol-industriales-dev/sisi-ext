using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class RequisicionSinOCDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int numero { get; set; }
        public int solicito { get; set; }
        public string solicitoDesc { get; set; }
        public DateTime fecha { get; set; }
        public bool? consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public int? comprador { get; set; }
        public string compradorDesc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuentaDesc { get; set; }
        public int tipoRequisicion { get; set; }
        public string tipoRequisicionDesc { get; set; }
        public DateTime? fechaValidacionAlmacen { get; set; }
        public string fechaValidacionAlmacenString { get; set; }
        public string estatusVencido { get; set; }
        public string PERU_tipoRequisicion { get; set; }
        public string noEconomico { get; set; }
    }
}
