using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.ChartJS
{
    public class ChartDataDTO
    {
        public List<string> labels { get; set; }
        public List<DatasetDTO> datasets { get; set; }
    }
}
