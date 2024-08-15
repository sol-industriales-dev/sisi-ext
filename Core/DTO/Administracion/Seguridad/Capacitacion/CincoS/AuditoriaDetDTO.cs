using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class AuditoriaDetDTO
    {
        #region SQL
        public int id { get; set; }
        public int auditoriaId { get; set; }
        public int inspeccionId { get; set; }
        public string descripcion { get; set; }
        public int respuesta { get; set; }
        public string accion { get; set; }
        public int usuario5sId { get; set; }
        public DateTime? fecha { get; set; }
        public string comentarioLider { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int auditoriaDetId { get; set; }
        public string descInspeccion { get; set; }
        public int idArchivoDeteccion { get; set; }
        public string descDeteccion { get; set; }
        public int estatusAuditoriaDet { get; set; }
        public int idArchivoSeguimiento { get; set; }
        public string comentarioRechazo { get; set; }
        public int aprobado { get; set; }
        public int idArchivo { get; set; }
        public string rutaDeteccion { get; set; }
        public string rutaMedida { get; set; }
        public string nombreUsuario5s { get; set; }
        public int idArchivoMedida { get; set; }
        public string fechaStr { get; set; }
        public string inspeccion { get; set; }
        #endregion
    }
}
