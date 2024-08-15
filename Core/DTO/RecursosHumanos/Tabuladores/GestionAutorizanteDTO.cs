using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class GestionAutorizanteDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Registro { get; set; }
        public VistaAutorizacionEnum vistaAutorizacion { get; set; }
        public NivelAutorizanteEnum nivelAutorizante { get; set; }
        public int FK_UsuarioAutorizacion { get; set; }
        public EstatusGestionAutorizacionEnum autorizado { get; set; }
        public DateTime? fechaFirma { get; set; }
        public string comentario { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreAutorizante { get; set; }
        public string puestoAutorizante { get; set; }
        public string descAutorizacion { get; set; }
        public int FK_Tabulador { get; set; }
        #endregion
    }
}
