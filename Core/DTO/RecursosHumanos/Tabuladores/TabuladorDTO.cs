using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class TabuladorDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Puesto { get; set; }
        public EstatusGestionAutorizacionEnum tabuladorAutorizado { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string cc { get; set; }
        public List<TabuladorDetDTO> lstTabuladoresDTO { get; set; }
        public List<int> lstPersonalNecesario { get; set; }
        public List<int> lstPuestosID { get; set; }
        public List<GestionAutorizanteDTO> lstGestionAutorizantesDTO { get; set; }
        public List<RepAutorizantesTABDTO> lstReporteAutorizantesDTO { get; set; }
        public int FK_LineaNegocio { get; set; }
        public List<int> lstFK_LineaNegocio { get; set; }
        public string puestoDesc { get; set; }
        public string tabuladorAutorizadoDesc { get; set; }
        public List<int> lstFK_AreaDepartamento { get; set; }
        public List<int> lstFK_Puestos { get; set; }
        public bool esFirmar { get; set; }
        public string comentarioRechazo { get; set; }
        public int tipoModificacion { get; set; }
        public List<string> lstDescLineaNegocio { get; set; }
        public List<int> lstPuestos { get; set; }
        public string nombreCompleto { get; set; }
        public List<string> lstCC { get; set; }
        public int FK_Tabulador { get; set; }
        public bool tabuladorDetAutorizado { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public List<int> lstPuestosDT { get; set; }
        public List<int> lstPuestosNuevos { get; set; }
        public bool _PRIMERA_BUSQUEDA { get; set; }
        public List<PersonalNecesarioDTO> lstPersonalNecesarioDTO { get; set; }
        public List<string> lstDescCC { get; set; }
        public int añoReporte { get; set; } //PARA TABULADOR
        public int FK_IncrementoAnual { get; set; }
        #endregion
    }
}