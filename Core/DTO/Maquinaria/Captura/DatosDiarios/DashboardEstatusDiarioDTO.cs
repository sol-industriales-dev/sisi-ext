using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.DatosDiarios
{
    public class DashboardEstatusDiarioDTO
    {
        public string tituloAreaCuenta { get; set; }
        public string fechaUltimaActualizacionString { get; set; }
        public GraficaDTO datosGrafica { get; set; }
    }
}
