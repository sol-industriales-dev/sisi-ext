using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Excel
{
    public class excelSheetDTO
    {
        public string name { get; set; }
        public List<excelRowDTO> Sheet { get; set; }
    }
}
