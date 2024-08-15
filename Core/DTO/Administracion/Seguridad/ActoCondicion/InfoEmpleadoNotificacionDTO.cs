using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class InfoEmpleadoNotificacionDTO
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public int area { get; set; }
    }
}
