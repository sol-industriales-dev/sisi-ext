using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class StandbyGridDTO
    {
        public int noEconomicoID { get; set; }
        public string Economico { get; set; }
        public string Grupo { get; set; }
        public string Modelo { get; set; }
        public string centro_costos { get; set; }
        public string Lugar { get; set; }
        public int TipoConsideracion { get; set; }
        public decimal HorometroInicial { get; set; }
        public decimal HorometroFinal { get; set; }
        public int estatus { get; set; }
        public string noEconomico { get; set; }
        public int id { get; set; }
    }
}
