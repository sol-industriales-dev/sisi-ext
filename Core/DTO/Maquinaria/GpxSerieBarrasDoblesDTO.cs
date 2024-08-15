using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria
{
    public class GpxSerieBarrasDoblesDTO
    {
        public string name { get; set; }
        public string color { get; set; }
        public List<decimal> data { get; set; }
        public decimal pointPadding { get; set; }
        public decimal pointPlacement { get; set; }
        public int yAxis { get; set; }

        public GpxSerieBarrasDoblesDTO()
        {
            data = new List<decimal>();
        }
    }

    public class GpxsHighCharts
    {
        public List<string> categories { get; set; }
        public List<GpxSerieBarrasDoblesDTO> series { get; set; }

        public GpxsHighCharts()
        {
            categories = new List<string>();
            series = new List<GpxSerieBarrasDoblesDTO>();
        }
    }
}
