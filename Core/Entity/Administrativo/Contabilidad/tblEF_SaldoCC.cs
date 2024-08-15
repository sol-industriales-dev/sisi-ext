using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_SaldoCC
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal cargosMes { get; set; }
        public decimal abonosMes { get; set; }
        public decimal cargosAcumulados { get; set; }
        public decimal abonosAcumulados { get; set; }
        public string cc { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public string areaCuenta { get; set; }
        public int itm { get; set; }
        public int corteMesID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("corteMesID")]
        public virtual tblEF_CorteMes corte { get; set; }
    }
}
