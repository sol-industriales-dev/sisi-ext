using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.SOS
{
    public class indicaroresMuestraGeneralDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string indicador { get; set; }
        public string elemento { get; set; }
        public string maquina { get; set; }
        public List<indicaroresMuestraGeneralDTO> children { get; set; }
    }
}
