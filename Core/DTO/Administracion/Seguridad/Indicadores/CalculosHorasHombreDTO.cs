using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class CalculosHorasHombreDTO
    {
        public string cc { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public string fechas { get; set; }
    }
}
