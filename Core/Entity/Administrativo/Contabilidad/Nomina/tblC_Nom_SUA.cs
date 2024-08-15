using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_SUA
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int tipoNomina { get; set; }
        public int tipoDocumento { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int usuarioRegistro { get; set; }
        public DateTime fechaModifica { get; set; }
        public int usuarioModifica { get; set; }
        public bool validado { get; set; }
        public DateTime fechaValida { get; set; }
        public int usuarioValida { get; set; }
        public bool polizaGuardada { get; set; }
        public string poliza { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("suaID")]
        public virtual List<tblC_Nom_SUA_Det> suaDetalle { get; set; }
    }
}
