using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class ReporteInventarioFisicoDTO
    {
        public string insumo { get; set; }
        public string insumoDesc { get; set; }
        public string cantidad { get; set; }
        public string unidad { get; set; }
        public string ubicacion { get; set; }
    }
}
