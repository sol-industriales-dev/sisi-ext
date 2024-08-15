using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cheque
{
    public class CboOCDTO
    {
        public int id { get; set; }
        public string text { get; set; }
        public decimal totalAnticipo { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public string proveedor { get; set; }
    }
}
