using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.SaludOcupacional;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class AtencionMedicaDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string empleadoDesc { get; set; }
        public DateTime fechaIngreso { get; set; }
        public string fechaIngresoString { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public int edad { get; set; }
        public int supervisor { get; set; }
        public string supervisorDesc { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public TipoAtencionMedicaEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public bool terminacion { get; set; }
        public bool archivoPendiente { get; set; }
        public int diasRestantes { get; set; }
        public string estatus { get; set; }
        public List<RevisionDTO> revisiones { get; set; }
        public string rutaArchivoST7 { get; set; }
        public string rutaArchivoST2 { get; set; }
    }
}
