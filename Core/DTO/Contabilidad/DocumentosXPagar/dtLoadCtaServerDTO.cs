using Core.DTO.Contabilidad.Bancos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar
{
    public class dtLoadCtaServerDTO
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ctaDTO> data { get; set; }
    }
}
