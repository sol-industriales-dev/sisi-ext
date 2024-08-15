using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.ChartJS
{
    public class DatasetDTO
    {
        public string label { get; set; }
        public List<decimal> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public List<string> borderColor { get; set; }
        public decimal borderWidth { get; set; }
        public bool fill { get; set; }
        public string type { get; set; }
    }
}
