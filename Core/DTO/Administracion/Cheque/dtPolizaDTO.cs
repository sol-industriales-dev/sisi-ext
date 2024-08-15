using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Cheque
{
    public class dtPolizaDTO
    {
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public string referecia { get; set; }
        public string cc { get; set; }
        public decimal debe { get; set; }
        public decimal haber { get; set; }
    }
}
