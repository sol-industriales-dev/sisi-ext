using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ExpedienteDigitalDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string empleadoDesc { get; set; }
        public string descPuesto { get; set; }
        public bool? esEvaluacion { get; set; }
        public List<ArchivoExpedienteDigitalDTO> archivos { get; set; }
    }
}
