using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.Gestion
{
    public class InsumoAutocompleteDTO
    {
        public string cc { get; set; }
        public int id { get; set; }
        public string value { get; set; }
        public string unidad { get; set; }
        public decimal precio { get; set; }
    }
}
