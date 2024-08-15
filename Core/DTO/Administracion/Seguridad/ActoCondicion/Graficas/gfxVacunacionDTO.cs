using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion.Graficas
{
    public class gfxVacunacionDTO
    {
        public List<string> categorias { get; set; }
        public List<gfxVacunacionSeries> series { get; set; }

        public gfxVacunacionDTO()
        {
            categorias = new List<string>();
            series = new List<gfxVacunacionSeries>();
        }
    }

    public class gfxVacunacionSeries
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
        public string stack { get; set; }

        public gfxVacunacionSeries()
        {
            data = new List<decimal>();
        }
    }
}
