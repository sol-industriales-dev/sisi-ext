using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Highcharts.Line
{
    public class SerieDTO
    {
        public string name { get; set; }
        public List<object> data { get; set; }

        public SerieDTO()
        {
            data = new List<object>();
        }
    }
}
