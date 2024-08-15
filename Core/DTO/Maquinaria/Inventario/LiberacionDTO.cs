using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class LiberacionDTO
    {
        public int idEconomico { get; set; }
        public string Economico { get; set; }
        public string CC { get; set; }
        public int idAsignacion { get; set; }
        public int estatusMaquina { get; set; }
        public DateTime FechaFin { get; set; }
        public int Horas { get; set; }
        public string Comentario { get; set; }
        public string descripcion { get; set; }
        public int estatus { get; set; }

    }
}
