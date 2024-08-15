using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Sistemas
{
    public class tblP_EmpresasDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string icono { get; set; }
        public bool estatus { get; set; }
        public bool activo { get; set; }
        public string url { get; set; }
        public string urlInterna { get; set; }
        public string urlLocal { get; set; }
        public bool desarrollo { get; set; }
        public string urlRedireccion { get; set; }
        public string iconoRedireccion { get; set; }
    }
}
