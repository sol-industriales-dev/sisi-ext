using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class cboDTO
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<string> componentes { get; set; }
        public int id { get; set; }
    }
}
