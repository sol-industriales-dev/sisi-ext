using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_Indicadores
    {
        public int id { get; set; }
        public int corteMesId { get; set; }
        public string cc { get; set; }
        public int? area { get; set; }
        public int? cuenta { get; set; }
        public string areaCuenta { get; set; }
        public decimal activoCirculante { get; set; }
        public decimal pasivoCirculante { get; set; }
        public decimal pasivoTotal { get; set; }
        public decimal activoTotal { get; set; }
        public decimal capitalContable { get; set; }
        public decimal inventario { get; set; }
        public decimal ebitda { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("corteMesId")]
        public virtual tblEF_CorteMes corte { get; set; }
    }
}
