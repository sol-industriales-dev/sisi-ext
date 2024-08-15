using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ListaInteresadosDTO
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int listaAutorizacionID { get; set; }
    }
}
