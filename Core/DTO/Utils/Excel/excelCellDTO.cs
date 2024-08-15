using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Excel
{
    public class excelCellDTO
    {
        public string text { get; set; }
        public bool autoWidthFit { get; set; }
        public bool border { get; set; }
        public bool fill { get; set; }
        public int colSpan { get; set; }
        public int rowSpan { get; set; }
        public int borderType { get; set; }
        public int formatType { get; set; }
        public bool textAlignLeft { get; set; }
    }
}
