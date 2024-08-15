using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class AuditoriaArchivoDTO
    {
        #region SQL
        public int id { get; set; }
        public int idAuditoriaDet { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionid { get; set; }
        public int usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
