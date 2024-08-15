using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Generales
{
    public class ComboGroupDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string label { get; set; }
        public string Prefijo { get; set; }
        public bool Selectable { get; set; }
        public List<ComboDTO> options { get; set; }
        public bool isGroup { get; set; }
        public string addClass { get; set; }
    }
}
