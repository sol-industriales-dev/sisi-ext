using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Reportes
{
    public class dsEncabezadoDTO
    {
        public byte[] logo { get; set; }
        public string titulo { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreReporte { get; set; }
        public string area { get; set; }

    }
}
