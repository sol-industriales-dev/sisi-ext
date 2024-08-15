using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula
{
    public class CedulaDepColombiaDTO
    {
        public bool esMaquina { get; set; }
        public int cuenta { get; set; }
        public string concepto { get; set; }
        public decimal depActualMN { get; set; }
        public decimal depActualCOP { get; set; }
        public decimal depAcumuladaMN { get; set; }
        public decimal depAcumuladaCOP { get; set; }
        public decimal depContabilidadMN { get; set; }
        public decimal depContabilidadCOP { get; set; }
        public decimal diferenciaMN { get; set; }
        public decimal diferenciaCOP { get; set; }
    }
}
