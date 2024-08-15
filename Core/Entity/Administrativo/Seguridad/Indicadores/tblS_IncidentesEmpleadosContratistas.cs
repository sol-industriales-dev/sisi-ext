using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesEmpleadosContratistas
    {
        public int id { get; set; }
        public int claveContratista { get; set; }
        public string nombre { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string puesto { get; set; }
    }
}
