using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class ClienteEmpresaDTO
    {
        public int id { get; set; }
        public string empresa { get; set; }
        public int clienteID { get; set; }
        public string cliente { get; set; }
    }
}
