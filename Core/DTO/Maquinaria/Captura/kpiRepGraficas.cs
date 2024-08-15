using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class kpiRepGraficas
    {
        public List<kpiGraficaFamiliasDTO> GraficaFamiliasDTO { get; set; }
        public List<kpiMotivosParoDTO> MotivosParoDTO { get; set; }
    }
}
