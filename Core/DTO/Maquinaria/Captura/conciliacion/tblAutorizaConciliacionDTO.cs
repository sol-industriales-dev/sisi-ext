using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.conciliacion
{
    public class tblAutorizaConciliacionDTO
    {
        public string areaCuenta { get; set; }
        public string descripcion { get; set; }
        public string periodo { get; set; }
        public int estatus { get; set; }
        public int id { get; set; }
        public int btnRpt { get; set; }
        public int btnValidar { get; set; }
        public bool isUsuarioAutorisable { get; set; }
        public string comentario { get; set; }
       
    }
}
