using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class replacePassDTO
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string md5 { get; set; }
        public string aes { get; set; }
    }
}
