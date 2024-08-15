using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Generales
{
    public class ComboDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Prefijo { get; set; }
        public string addClass { get; set ; }
        public ComboDTO()
        {
            addClass = "normalOption_Selectable";
        }
        public int Orden { get; set; }
        public string Id { get; set; }
        public string TextoOpcional { get; set; }
    }
}
