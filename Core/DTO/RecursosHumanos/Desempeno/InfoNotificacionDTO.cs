using Core.Entity.RecursosHumanos.Desempeno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class InfoNotificacionDTO
    {
        public bool DeEmpleadoAJefe { get; set; }
        public string MensajePushUp { get; set; }
        public string AsuntoCorreo { get; set; }
        public string CuerpoCorreo { get; set; }
    }
}