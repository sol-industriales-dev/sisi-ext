using Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_ResumenRaya : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int nominaId { get; set; }
        public int cuentaId { get; set; }
        public string cta { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }

        [ForeignKey("nominaId")]
        public virtual tblC_Nom_Nomina nomina { get; set; }

        [ForeignKey("cuentaId")]
        public virtual tblC_Nom_Cuenta cuenta { get; set; }
    }
}
