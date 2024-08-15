using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula
{
    public class CedulaSaldoColombiaDTO
    {
        public bool esMaquina { get; set; }
        public int cuenta { get; set; }
        public string concepto { get; set; }
        public decimal saldoAnteriorMN { get; set; }
        public decimal saldoAnteriorCOP { get; set; }
        public decimal altaActualMN { get; set; }
        public decimal altaActualCOP { get; set; }
        public decimal bajaActualMN { get; set; }
        public decimal bajaActualCOP { get; set; }
        public decimal saldoActualMN { get; set; }
        public decimal saldoActualCOP { get; set; }
        public decimal contabilidadMN { get; set; }
        public decimal contabilidadCOP { get; set; }
        public decimal diferenciaMN { get; set; }
        public decimal diferenciaCOP { get; set; }
    }
}
