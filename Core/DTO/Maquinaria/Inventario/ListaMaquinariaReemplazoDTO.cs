using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ListaMaquinariaReemplazoDTO
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public HttpPostedFileBase Files { get; set; }

    }
}
