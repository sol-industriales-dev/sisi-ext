using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class busqDashboardDTO
    {
        public ClasificacionHHTEnum clasificacion { get; set; }
        public List<string> arrCC { get; set; }
        public List<MultiSegDTO> arrGrupos { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public List<int> arrDepto { get; set; }
        public List<int> arrSupervisor { get; set; }
        public tipoAccientabilidadEnum tipoAccidente { get; set; }
        public List<int> arrDivisiones { get; set; }
        public List<int> arrLineasNegocio { get; set; }
    }
}
