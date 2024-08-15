using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_Tabuladores
    {
        #region SQL
        public int id { get; set; }
        public int FK_Puesto { get; set; }
        public EstatusGestionAutorizacionEnum tabuladorAutorizado { get; set; }
        public string comentarioRechazo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        [ForeignKey("FK_Puesto")]
        public virtual tblRH_EK_Puestos puesto { get; set; }
    }
}
