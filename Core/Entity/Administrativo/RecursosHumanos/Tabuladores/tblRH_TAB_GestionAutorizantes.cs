using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_GestionAutorizantes
    {
        #region SQL
        public int id { get; set; }
        public int FK_Registro { get; set; }
        public int FK_TabuladorDet { get; set; }
        public bool categoriaNueva { get; set; }
        public VistaAutorizacionEnum vistaAutorizacion { get; set; }
        public NivelAutorizanteEnum nivelAutorizante { get; set; }
        public int FK_UsuarioAutorizacion { get; set; }
        public EstatusGestionAutorizacionEnum autorizado { get; set; }
        public DateTime? fechaFirma { get; set; }
        public string comentario { get; set; }
        public bool procesoFinalizado { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
