using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Highcharts
{
    public class HighchartDTO
    {
        public List<object> series { get; set; }
        public object xAxis { get; set; }

        public HighchartDTO()
        {
            series = new List<object>();
        }
    }
}
