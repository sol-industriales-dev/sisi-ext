using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Requisicion
{
    public class EmpleadoAutocompleteDTO
    {
        public int id { get; set; }
        public string value { get; set; }
        public int claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string puestoEmpleado { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
    }
}
