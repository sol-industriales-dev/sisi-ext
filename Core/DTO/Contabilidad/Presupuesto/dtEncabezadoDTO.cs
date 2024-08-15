using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Presupuesto
{
    public class dtEncabezadoDTO
    {
        public string Eco { get; set; }
        public string Modelo { get; set; }
        public string HrsTrab { get; set; }
        public dtSubEncabezadoDTO Depreciacion { get; set; }
        public dtSubEncabezadoDTO Seguro { get; set; }
        public dtSubEncabezadoDTO Filtros { get; set; }
        public dtSubEncabezadoDTO Correctivo { get; set; }
        public dtSubEncabezadoDTO DepreciacionOverhaul { get; set; }
        public dtSubEncabezadoDTO Aceite { get; set; }
        public dtSubEncabezadoDTO Carrileria { get; set; }
        public dtSubEncabezadoDTO Ansul { get; set; }
        public dtSubEncabezadoDTO Total { get; set; }
    }
}
