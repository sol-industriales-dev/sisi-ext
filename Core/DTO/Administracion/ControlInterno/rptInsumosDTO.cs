using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.ControlInterno
{
    public class rptInsumosDTO
    {
        public string insumo { get; set; }
        public string descripcion { get; set; }
        public string almacen { get; set; }
        public decimal cantidad { get; set; }
    }
}
