using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cotnratistas
{
    public class EmpresasDTO
    {
        public int id { get; set; }
        public string nombreEmpresa { get; set; }
        public bool esActivo { get; set; }


        public string msjExito { get; set; }
        public int statusExito { get; set; }
    }
}
