using Core.Enum.Administracion.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.SaludOcupacional
{
    public class HistorialEmpleadoDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string empleadoDesc { get; set; }
        public string curp { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int area { get; set; }
        public string areaDesc { get; set; }
        public TipoDocumentoEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
    }
}
