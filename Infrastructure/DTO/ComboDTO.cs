using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    [Serializable]
    public class ComboDTO
    {
        public Int64 Value { get; set; }
        public string Text { get; set; }
        public string Prefijo { get; set; }
        public string name { get; set; }
    }
}
