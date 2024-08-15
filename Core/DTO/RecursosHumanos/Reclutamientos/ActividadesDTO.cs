using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ActividadesDTO
    {
        public int id { get; set; }
        public int idFase { get; set; }
        public string nombreFase { get; set; }
        public string tituloActividad { get; set; }
        public string descActividad { get; set; }
        public bool esArchivos { get; set; }
        public bool esObligatoria { get; set; }
        public bool esGeneral { get; set; }
        public bool esCalificacion { get; set; }
        public bool esNecesarioAprobar { get; set; }
        public int? tipoArchivo { get; set; }
        public int idUsuarioEncargado { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public string estatus { get; set; }
        public string nombreEncargado { get; set; }
        public string colorFase { get; set; }
        public string color { get; set; }

        #region ADICIONALES
        public decimal calificacion { get; set; }
        public string comentario { get; set; }
        public string firma { get; set; }
        public int numEvidencia { get; set; }
        public bool esOfi { get; set; }
        public bool? esValidar { get; set; }
        public int? actividadValidar { get; set; }
        public bool esNotificada { get; set; }
        public bool esTodasActividades { get; set; } // PERMISO PARA TENER ACCESO A TODAS LAS ACTIVIDADES
        #endregion
    }
}
