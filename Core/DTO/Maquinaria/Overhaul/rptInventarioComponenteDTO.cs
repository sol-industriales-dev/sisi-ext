using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptInventarioComponenteDTO
    {
        public int idModelo { get; set; }
        public string modelo { get; set; }
        public int idSubconjunto { get; set; }
        public string subconjunto { get; set; }
        public int total { get; set; }
        public List<int> locaciones { get; set; }
        public List<ComboDTO> totalesLocaciones { get; set; }
    }
}
