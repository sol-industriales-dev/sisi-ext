using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class EFSaldosCCDTO
    {
        public int anio { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string ccCtaSctaSscta { get; set; }
        public decimal cargosMes { get; set; }
        public decimal abonosMes { get; set; }
        public decimal cargoAcumulado { get; set; }
        public decimal abonoAcumulado { get; set; }
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public int corteMesID { get; set; }
        public tblEF_CorteMes corte { get; set; }
        public bool estatus { get; set; }
    }
}
