using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_Empleado
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
        public bool superUsuario { get; set; }
        public string cc { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaInicioCC { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
