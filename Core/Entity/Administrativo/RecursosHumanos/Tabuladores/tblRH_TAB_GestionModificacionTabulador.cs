using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_GestionModificacionTabulador
    {
        #region SQL
        public int id { get; set; }
        public TipoModificacionEnum tipoModificacion { get; set; }
        public int FK_LineaNegocio { get; set; }
        public int FK_AreaDepartamento { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }
        public EstatusGestionAutorizacionEnum modificacionAutorizada { get; set; }
        public string comentarioRechazo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
