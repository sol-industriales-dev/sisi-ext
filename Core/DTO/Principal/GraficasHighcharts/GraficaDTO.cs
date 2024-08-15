using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.GraficasHighcharts
{
    public class GraficaDTO
    {
        #region GRAFICA
        public string name { get; set; }
        public decimal y { get; set; }
        public List<decimal> lst_y { get; set; }
        public string drilldown { get; set; }
        #endregion
    }
}
