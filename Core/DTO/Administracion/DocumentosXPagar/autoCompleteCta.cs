using Core.DTO.Contabilidad.Bancos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class autoCompleteCta : ctaDTO
    {
        public string id { get; set; }
        public string label { get; set; }
    }
}
