using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class VacacionesDTO
    {
        #region TABLA SQL
        public int id { get; set; }
        public int estado { get; set; }
        public string nombreEmpleado { get; set; }
        public string claveEmpleado { get; set; }
        public string ccEmpleado { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int idPeriodo { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public string comentarioRechazada { get; set; }
        public int? tipoVacaciones { get; set; }
        public int? consecutivo { get; set; }
        public bool esPagadas { get; set; }
        public string justificacion { get; set; }
        public int idJefeInmediato { get; set; }
        public int? numDiasDisponiblesAlDiaCaptura { get; set; }
        public string nombreJefeInmediato { get; set; }
        public string rutaArchivoActa { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int dias { get; set; }
        public int diasPaternidad { get; set; }
        public int diasMatrimonio { get; set; }
        public bool esFirmar { get; set; }
        public List<VacacionesGestionDTO> lstAutorizantes { get; set; }
        public List<string> lstFiltroCC { get; set; }
        public string descPuesto { get; set; }
        public string nombreCapturo { get; set; }
        public DateTime? fechaFiltroInicio { get; set; }
        public DateTime? fechaFiltroFin { get; set; }
        public int numDiasPagados { get; set; }
        #endregion
    }
}