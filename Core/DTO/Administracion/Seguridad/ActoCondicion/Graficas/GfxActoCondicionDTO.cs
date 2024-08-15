using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion.Graficas
{
    public class GfxActoCondicionDTO
    {
        public DateTime fechaSuceso { get; set; }
        public TipoActo tipoActo { get; set; }
        public int departamentoID { get; set; }
        public int clasificacionID { get; set; }
        public int accionID { get; set; }
        public EstatusActoCondicion estatus { get; set; }
        public int idAgrupacion { get; set; }
        public int idEmpresa { get; set; }
        public string nombreAgrupacion { get; set; }
        public int clave_empleado { get; set; }
    }
}
