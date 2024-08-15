using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.Almacen
{
    public class si_almacenDTO
    {
        public int almacen { get; set; }
        public string descripcion { get; set; }
        public string direccion { get; set; }
        public string responsable { get; set; }
        public string telefono { get; set; }
        public string valida_almacen_cc { get; set; }
        public string bit_pt { get; set; }
        public string bit_mp { get; set; }
        public string cc { get; set; }
        public int? almacen_virtual { get; set; }

        public bool botonEliminar { get; set; }
    }
}
