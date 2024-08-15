using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class RelacionCCDepartamentoRazonSocialDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public string departamento { get; set; }
        public string departamentoDesc { get; set; }
        public int razonSocialID { get; set; }
        public string razonSocialDesc { get; set; }
        public string rfc { get; set; }
        public int empresa { get; set; }
        public string empresaDesc { get; set; }
        public bool estatus { get; set; }
    }
}
