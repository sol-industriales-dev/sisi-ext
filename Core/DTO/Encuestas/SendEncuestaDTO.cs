using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class SendEncuestaDTO
    {
        public int encuestaID { get; set; }
        public string encuestaNombre { get; set; }
        public int encuestadoID { get; set; }
    }
}
