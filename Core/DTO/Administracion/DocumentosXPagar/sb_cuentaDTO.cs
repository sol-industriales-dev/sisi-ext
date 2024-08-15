using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class sb_cuentaDTO
    {
        public int cuenta { get; set; }
        public int banco { get; set; }
        public string descripcion { get; set; }
        public int moneda { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
    }
}
