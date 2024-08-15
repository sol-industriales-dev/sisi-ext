using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Generales.Enkontrol
{
    public class AreaCuentaDTO
    {
        public string CC { get; set; }
        public int Area { get; set; }
        public int Cuenta { get; set; }
        public string Descripcion { get; set; }
        public bool EsMaquinaria { get; set; }
        public bool CcActivo { get; set; }
        public bool AcCancelada { get; set; }
    }
}