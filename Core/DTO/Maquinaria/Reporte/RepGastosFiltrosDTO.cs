using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class RepGastosFiltrosDTO
    {
        public int mesID { get; set; }
        public string maq { get; set; }
        public string mes { get; set; }
        public int anio { get; set; }
        public int idTipo { get; set; }
        public int idGrupo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string ac { get; set; }

    }
}
