using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Evaluacion
{
    public class EmpleadoDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public int puestoEvaluacionID { get; set; }
        public bool evaluador { get; set; }
        public RolEnum rol { get; set; }
        public DateTime fechaInicioRol { get; set; }
        public string cc { get; set; }
        public bool estatus { get; set; }

        public string puestoDesc { get; set; }
        public bool aplica { get; set; }

        //Propiedades del Dashboard
        public string empresa { get; set; }
        public CategoriaPuestoEnum categoria { get; set; }
        public decimal porcentajeCumplimientoMensual { get; set; }
        public List<ActividadDTO> actividades { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
    }
}
