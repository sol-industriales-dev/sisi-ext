using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Highcharts.Line
{
    public class xAxisDTO
    {
        public List<object> categories { get; set; }
        public bool crosshair { get; set; }

        public xAxisDTO()
        {
            categories = new List<object>();
        }
    }
}
