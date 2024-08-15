using Core.Enum.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class GuardarFirmaDTO
    {
        public int idRowImagen { get; set; }
        public int tipoTabla { get; set; }
        public int id { get; set; }
        public int claveEmpleado { get; set; }

        public int claveEmpleadoSST { get; set; }
        public string nombreEmpleadoSST { get; set; }
        public int idActoCondicion { get; set; }
        public TipoRiesgoCH tipoRiesgo { get; set; }
        public TipoFirmaEnum tipoFirma { get; set; }
        public string imagen { get; set; }
    }
}
