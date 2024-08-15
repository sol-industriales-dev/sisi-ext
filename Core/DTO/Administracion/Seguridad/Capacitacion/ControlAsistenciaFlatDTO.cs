using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class ControlAsistenciaFlatDTO
    {
        #region General
        public int id_general { get; set; }
        public int cursoID { get; set; }
        public int instructorID { get; set; }
        public DateTime fechaCapacitacion { get; set; }
        public string cc_general { get; set; }
        public int empresa { get; set; }
        public string lugar { get; set; }
        public string horario { get; set; }
        public EstatusControlAsistenciaEnum estatus_general { get; set; }
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
        public int division { get; set; }
        public bool validacion { get; set; }
        public bool migrado { get; set; }
        #endregion

        #region Detalle
        public int id_detalle { get; set; }
        public int claveEmpleado { get; set; }
        public string puesto { get; set; }
        public string cc_detalle { get; set; }
        public int examenID { get; set; }
        public EstatusEmpledoControlAsistenciaEnum estatus_detalle { get; set; }
        public decimal calificacion { get; set; }
        public EstatusAutorizacionEmpleadoControlAsistenciaEnum estatusAutorizacion { get; set; }
        public string rutaExamenInicial { get; set; }
        public string rutaExamenFinal { get; set; }
        public string rutaDC3 { get; set; }
        public int controlAsistenciaID { get; set; }
        #endregion

        #region Curso
        public int id_curso { get; set; }
        public string claveCurso { get; set; }
        public string cursoNombre { get; set; }
        public ClasificacionCursoEnum clasificacion { get; set; }
        public decimal duracion { get; set; }
        #endregion
    }
}
