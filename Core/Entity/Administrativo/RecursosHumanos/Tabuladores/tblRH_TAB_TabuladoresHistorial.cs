using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_TabuladoresHistorial
    {
        #region SQL
        public int id { get; set; }
        public int FK_Tabulador { get; set; }
        public int FK_Puesto { get; set; }
        public EstatusGestionAutorizacionEnum tabuladorAutorizado { get; set; }
        public string comentarioRechazo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}