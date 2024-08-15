using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion.Historial
{
    public class HistorialEmpleadoDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public string proyecto { get; set; }
        public TipoActo tipoActo { get; set; }
        public string tipoActoDesc { get; set; }
        public string accionDesc { get; set; }
        public string departamentoDesc { get; set; }
        public string clasificacionDesc { get; set; }
        public string procedimientoDesc { get; set; }
        public DateTime fechaSucesoDT { get; set; }
        public string fechaSuceso { get; set; }
        public bool tieneEvidencia { get; set; }
    }
}
