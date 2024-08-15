using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class PolizaDTO
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public string TipoPoliza { get; set; }
        public int Poliza { get; set; }
        public int Cuenta { get; set; }
        public decimal Cargo { get; set; }
        public decimal Abono { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPoliza { get; set; }
        public string Generada { get; set; }
        public string Estatus { get; set; }
        public bool Sigoplan { get; set; }
        public int? IdPolSigoplan { get; set; }
        public bool UltimaSigoplan { get; set; }
    }
}