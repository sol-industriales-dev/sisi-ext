using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class CCAgrupacionDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public int grupoID { get; set; }
    }
}
