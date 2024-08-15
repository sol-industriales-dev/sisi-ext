using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.ReservacionVehiculo
{
    public class ReservacionVehiculoDTO
    {
        public int id { get; set; }
        public string fechaSalida { get; set; }
        public string fechaEntrega { get; set; }
        public string vigenciaLicencia { get; set; }
        public string motivo { get; set; }
        public string justificacion { get; set; }
        public bool autorizada { get; set; }
        public bool estatus { get; set; }
        public string solicitante { get; set; }
        public bool tienePermiso { get; set; }
    }
}
