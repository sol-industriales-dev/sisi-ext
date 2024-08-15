using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_EstructuraResumenNominaCC
    {
        public int id { get; set; }
        public int cuentaId { get; set; }
        public string columnaRaya { get; set; }
        public bool incluirScta { get; set; }
        public bool incluirSscta { get; set; }
        public int tipoRayaId { get; set; }
        public int tipoNominaId { get; set; }
        public int clasificacionCcId { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("cuentaId")]
        public virtual tblC_Nom_Cuenta cuenta { get; set; }

        [ForeignKey("tipoRayaId")]
        public virtual tblC_Nom_TipoRaya tipoRaya { get; set; }

        [ForeignKey("tipoNominaId")]
        public virtual tblC_Nom_TipoNomina tipoNomina { get; set; }

        [ForeignKey("clasificacionCcId")]
        public virtual tblC_Nom_ClasificacionCC clasificacionCC { get; set; }
    }
}
