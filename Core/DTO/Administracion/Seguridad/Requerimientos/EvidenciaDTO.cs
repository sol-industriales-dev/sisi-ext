using Core.Enum.Administracion.Seguridad.Requerimientos;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.Requerimientos
{
    public class EvidenciaDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int requerimientoID { get; set; }
        public int puntoID { get; set; }
        public DateTime fechaPunto { get; set; }
        public string rutaEvidencia { get; set; }
        public string comentariosCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCapturaID { get; set; }
        public int usuarioEvaluadorID { get; set; }
        public string comentariosEvaluador { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }
        public DateTime fechaEvaluacion { get; set; }
        public int asignacionID { get; set; }
        public decimal porcentajePunto { get; set; }
        public PeriodicidadRequerimientoEnum periodicidad { get; set; }
        public int actividad { get; set; }
        public int condicionante { get; set; }
        public int seccion { get; set; }
        public ClasificacionEnum clasificacion { get; set; }
        public string clasificacionDesc { get; set; }

        public HttpPostedFileBase evidencia { get; set; }
        public List<int> listaPuntos { get; set; }
        public List<DateTime> listaFechaPunto { get; set; }
        public List<HttpPostedFileBase> listaEvidencias { get; set; }
        public DateTime fechaInicioEvaluacion { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
    }
}
