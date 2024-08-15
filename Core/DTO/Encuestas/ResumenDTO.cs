using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class ResumenDTO
    {

        public int id { get; set; }
        public int encuestaID { get; set; }
        public int usuarioResponderID { get; set; }
        public DateTime fechaEnv { get; set; }
        public DateTime fechaResp { get; set; }
        public string comentario { get; set; }
    }
}
