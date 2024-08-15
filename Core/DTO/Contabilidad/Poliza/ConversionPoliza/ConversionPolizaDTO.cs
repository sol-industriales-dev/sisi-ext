using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza.ConversionPoliza
{
    public class ConversionPolizaDTO
    {
        public EmpresaEnum PalEmpresa { get; set; }
        public int PalYear { get; set; }
        public int PalMes { get; set; }
        public string PalTP { get; set; }
        public int PalPoliza { get; set; }
        public decimal PalCargo { get; set; }
        public decimal PalAbono { get; set; }
        public decimal PalCargoUs { get; set; }
        public decimal PalAbonoUs { get; set; }
        public DateTime PalFechapol { get; set; }
        public EmpresaEnum SecEmpresa { get; set; }
        public int SecYear { get; set; }
        public int SecMes { get; set; }
        public string SecTP { get; set; }
        public int SecPoliza { get; set; }
        public decimal SecCargo { get; set; }
        public decimal SecAbono { get; set; }
        public string Estatus { get; set; }
        public string fechaPoliza { get; set; }
        public int estatusRegistro { get; set; }
    }
}
