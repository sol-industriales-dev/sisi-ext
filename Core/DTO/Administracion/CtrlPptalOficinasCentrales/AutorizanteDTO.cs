using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class AutorizanteDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int idCC { get; set; }
        public int idAutorizante { get; set; }
        public string nombreAutorizante { get; set; }
        public string descripcion { get; set; }
        public int idRow { get; set; }
    }
}
