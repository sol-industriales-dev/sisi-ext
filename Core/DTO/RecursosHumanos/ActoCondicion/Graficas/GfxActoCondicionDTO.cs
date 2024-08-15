using Core.Enum.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion.Graficas
{
    public class GfxActoCondicionDTO
    {
        public DateTime fechaSuceso { get; set; }
        public TipoActoCH tipoActo { get; set; }
        public int departamentoID { get; set; }
        public int clasificacionID { get; set; }
        public int accionID { get; set; }
        public EstatusActoCondicionCH estatus { get; set; }
        public int idAgrupacion { get; set; }
        public int idEmpresa { get; set; }
        public string nombreAgrupacion { get; set; }
        public int clave_empleado { get; set; }
    }
}
