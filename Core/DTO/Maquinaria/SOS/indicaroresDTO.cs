using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.SOS
{
    public class indicaroresDTO
    {
        public int indicador { get; set; }

        public double alerta
        {
            get;
            set;
        }
        public string name { get; set; }
        public string hora_Aceite { get; set; }
        public string hora_equipo { get; set; }
        
        public double precaucion { get; set; }
    }
}
