using Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_Cuenta : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public int tipoCuentaId { get; set; }
        public int tipoNominaId { get; set; }
        public int clasificacionCcId { get; set; }

        [ForeignKey("tipoCuentaId")]
        public virtual tblC_Nom_TipoCuenta tipoCuenta { get; set; }

        [ForeignKey("tipoNominaId")]
        public virtual tblC_Nom_TipoNomina tipoNomina { get; set; }

        [ForeignKey("clasificacionCcId")]
        public virtual tblC_Nom_ClasificacionCC clasificacionCc { get; set; }
    }
}
