using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadControlAsistencia
    {
        public int id { get; set; }
        public int cursoID { get; set; }
        public virtual tblS_CapacitacionSeguridadCursos curso { get; set; }
        public int instructorID { get; set; }
        public virtual tblP_Usuario instructor { get; set; }
        public DateTime fechaCapacitacion { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public string lugar { get; set; }
        public string horario { get; set; }
        public int estatus { get; set; }
        public string nombreCarpeta { get; set; }
        public string rutaListaAsistencia { get; set; }
        public string rutaListaAutorizacion { get; set; }
        public string comentario { get; set; }
        public string rfc { get; set; }
        public string razonSocial { get; set; }
        public bool esExterno { get; set; }
        public string instructorExterno { get; set; }
        public string empresaExterna { get; set; }
        public bool activo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public virtual tblP_Usuario usuarioCreador { get; set; }
        public virtual List<tblS_CapacitacionSeguridadControlAsistenciaDetalle> asistentes { get; set; }
        public int division { get; set; }
        public bool validacion { get; set; }
    }
}
