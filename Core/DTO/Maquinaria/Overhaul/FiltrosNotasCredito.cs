using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class FiltrosNotasCredito
    {
        public string Generador { get; set; }
        public string OC { get; set; }
        public int idEconomico { get; set; }
        public string Descripcion { get; set; }
        public int CausaRemosion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TipoFiltro { get; set; }
        public int FiltroTipoNC { get; set; }
        public string cc { get; set; }
        public string almacen { get; set; }
    }
}
