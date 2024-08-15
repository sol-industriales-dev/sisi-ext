using Core.Enum.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoCatMaqDTO
    {
        public int Id { get; set; }
        public string CC { get; set; }
        public string NoEconomico { get; set; }
        public string Descripcion { get; set; }
        public string AreaCuenta { get; set; }
        public bool DepreciacionCapturada { get; set; }
        public AnexoMaquinaDTO Factura { get; set; }
        public bool CapturaAutomatica { get; set; }
        public int estatus { get; set; }
        public DateTime FechaAdquisicion { get; set; }
    }
}