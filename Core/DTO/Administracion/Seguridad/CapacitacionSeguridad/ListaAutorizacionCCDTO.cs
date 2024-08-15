using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class ListaAutorizacionCCDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int departamento { get; set; }
        public string departamentoDesc { get; set; }
        public int empresa { get; set; }
        public int listaAutorizacionID { get; set; }
        public bool estatus { get; set; }
    }
}
