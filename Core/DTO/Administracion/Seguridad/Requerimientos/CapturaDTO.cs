using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Requerimientos
{
    public class CapturaDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public string proyecto { get; set; }
        public int requerimientoID { get; set; }
        public string requerimiento { get; set; }
        public string requerimientoDesc { get; set; }
        public ClasificacionEnum requerimientoClasificacion { get; set; }
        public string requerimientoClasificacionDesc { get; set; }
        public string indice { get; set; }
        public string puntoDesc { get; set; }
        public string descripcion { get; set; }
        public int asignacionID { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public string fechaAsignacionString { get; set; }
        public DateTime fechaInicioEvaluacion { get; set; }
        public string fechaInicioEvaluacionString { get; set; }
        public string codigo { get; set; }
        public int evidenciaID { get; set; }
        public bool evidenciaCapturada { get; set; }
        public string rutaEvidencia { get; set; }
        public int usuarioEvaluadorID { get; set; }
        public bool aprobado { get; set; }
        public PeriodicidadRequerimientoEnum periodicidad { get; set; }
        public string periodicidadDesc { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public int responsable { get; set; }
        public string responsableDesc { get; set; }
        public DateTime fechaEvidencia { get; set; }
        public string fechaEvidenciaString { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
