using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoDiferenciasContablesDTO
    {
        public int Cuenta { get; set; }
        public int Año { get; set; }
        public string Mes { get; set; }
        public decimal MontoMovPol { get; set; }
        public decimal MontoSalCont { get; set; }
        public decimal Diferencia { get; set; }

        public List<ActivoFijoDiferenciasContablesDetallesDTO> Detalles { get; set; }
    }

    public class ActivoFijoDiferenciasContablesDetallesDTO
    {
        public string CC { get; set; }
        public string NumEconomico { get; set; }
        public decimal MontoMovPol { get; set; }
        public decimal MontoSalCont { get; set; }
        public decimal Diferencia { get; set; }
    }
}