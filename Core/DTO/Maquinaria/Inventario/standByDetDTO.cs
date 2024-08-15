using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class standByDetDTO
    {
        public int noEconomicoID { get; set; }
        public DateTime FechaStandBy { get; set; }
        public string cc { get; set; }
        public int TipoConsideracion { get; set; }

        public string Economico { get; set; }
        public int id { get; set; }
    }
}
