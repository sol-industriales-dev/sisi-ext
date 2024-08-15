using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.KPI
{
    public class BusqKpiDiariaDTO
    {
        public string ac { get; set; }
        public int idModelo { get; set; }
        public int idGrupo { get; set; }
        public DateTime fecha { get; set; }
        public int turno { get; set; }
    }
}
