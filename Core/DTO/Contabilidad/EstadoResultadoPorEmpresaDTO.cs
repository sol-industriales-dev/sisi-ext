using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class EstadoResultadoPorEmpresaDTO
    {
        public decimal montoEmpresa { get; set; }
        public decimal montoYearAnteriorEmpresa { get; set; }
        public bool sumarConsolidado { get; set; }
        public decimal montoCruzado { get; set; }
        public decimal montoCruzadoAnterior { get; set; }
    }
}
