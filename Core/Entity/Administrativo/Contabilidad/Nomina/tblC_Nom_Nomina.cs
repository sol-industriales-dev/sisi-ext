using Core.DTO;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_Nomina : InfoRegistroTablaDTO
    {
        public int id { get; set; }
        public int year { get; set; }
	    public int periodo { get; set; }
	    public int tipoNomina { get; set; }
	    public string CC { get; set; }
	    public string nombreCC { get; set; }
	    public DateTime fechaCaptura { get; set; }
        public bool validada { get; set; }
        public int? usuarioValidoId { get; set; }
        public DateTime? fechaValidacion { get; set; }
        public string poliza { get; set; }
        public string dirArchivo { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoRayaId { get; set; }
        public int clasificacionCcId { get; set; }

        [ForeignKey("usuarioValidoId")]
        public virtual tblP_Usuario usuarioValido { get; set; }

        
        [ForeignKey("nominaID")]
        public virtual ICollection<tblC_Nom_Raya> raya { get; set; }

        [ForeignKey("nominaId")]
        public virtual ICollection<tblC_Nom_ResumenRaya> resumen { get; set; }

        [ForeignKey("tipoRayaId")]
        public virtual tblC_Nom_TipoRaya tipoRaya { get; set; }

        [ForeignKey("clasificacionCcId")]
        public virtual tblC_Nom_ClasificacionCC clasificacionCC { get; set; }
    }
}
