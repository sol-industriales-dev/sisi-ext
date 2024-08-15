using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class GestionModificacionTabuladorDTO
    {
        #region SQL
        public int id { get; set; }
        public TipoModificacionEnum tipoModificacion { get; set; }
        public int FK_LineaNegocio { get; set; }
        public int FK_AreaDepartamento { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }
        public EstatusGestionAutorizacionEnum modificacionAutorizada { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public List<int> lstFK_Puestos { get; set; }
        public List<int> lstFK_LineaNegocio { get; set; }
        public DateTime fechaAplicaCambio { get; set; }
        public bool esFirmar { get; set; }
        public string comentarioRechazo { get; set; }
        public int FK_Registro { get; set; }
        public string tipoModificacionStr { get; set; }
        public string lineaNegocioStr { get; set; }
        public string areaDepartamentoStr { get; set; }
        public string puestoDesc { get; set; }
        public int FK_Tabulador { get; set; }
        #endregion
    }
}
