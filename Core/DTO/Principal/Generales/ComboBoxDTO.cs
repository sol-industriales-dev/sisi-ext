using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Generales
{
    public class ComboBoxDTO
    {
        public object valor { get; set; }
        public string texto { get; set; }
        public List<ComboBoxDataDTO> datas { get; set; }

        public ComboBoxDTO()
        {
            datas = new List<ComboBoxDataDTO>();
        }
    }
}
