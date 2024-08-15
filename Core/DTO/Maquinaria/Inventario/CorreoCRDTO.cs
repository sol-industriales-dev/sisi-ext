using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class CorreoCRDTO
    {
        public string solicitud { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public string fechaObra { get; set; }
        public string Economico { get; set; }
    }
}
