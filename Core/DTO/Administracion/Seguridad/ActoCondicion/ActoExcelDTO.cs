using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class ActoExcelDTO
    {
        public string folio { get; set; }
        public string proyecto { get; set; }
        public string descripcion { get; set; }
        public string fechaSuceso { get; set; }
        public string accionDesc { get; set; }

        public int claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string departamento { get; set; }
        public string tipoActo { get; set; }
        public string clasificacion { get; set; }
        public string procedimiento { get; set; }

        public DateTime fechaSucesoDT { get; set; }
        public TipoActo tipoActoEnum { get; set; }
        public int subClasificacionDepID { get; set; }
        public string subclasificacion { get; set; }
        public string empleadoInformo { get; set; }
    }
}
