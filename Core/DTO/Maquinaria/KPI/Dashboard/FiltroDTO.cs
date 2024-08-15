using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class FiltroDTO
    {
        public string areaCuenta { get; set; }
        public List<int> idGrupos { get; set; }
        public List<int> idModelos { get; set; }
        public List<int> idEconomicos { get; set; }
        public int turno { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
