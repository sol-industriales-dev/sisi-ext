using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoRelMaqDepDTO
    {
        public ActivoFijoCatMaqDTO Maquina { get; set; }
        public ActivoFijoDepMaqDTO Depreciacion { get; set; }
    }
}