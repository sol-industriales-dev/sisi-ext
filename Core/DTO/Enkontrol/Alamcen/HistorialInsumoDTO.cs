using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class HistorialInsumoDTO
    {
        public int almacen { get; set; }
        public string almacenDesc { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
    }
}
