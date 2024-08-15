using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class ReporteCertificadoTrabajoDTO
    {
        #region REPORTE CERTIFICADO DE TRABAJO
        public int id { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreEmpleado { get; set; }
        public string puesto { get; set; }
        public string nombreCurso { get; set; }
        public string cc { get; set; }
        public DateTime fechaCurso { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string calificacion { get; set; }
        #endregion

        #region ADICIONAL
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string firma { get; set; }
        public int instructorID { get; set; }
        public string rutaArchivo { get; set; }
        public byte[] imagen { get; set; }
        #endregion
    }
}