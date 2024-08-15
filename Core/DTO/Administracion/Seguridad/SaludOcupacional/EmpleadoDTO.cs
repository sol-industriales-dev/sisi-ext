using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class EmpleadoDTO
    {
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string curp { get; set; }
        public DateTime fechaIngreso { get; set; }
        public string fechaIngresoString { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int edad { get; set; }
        public int supervisor { get; set; }
        public string supervisorDesc { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
    }
}
