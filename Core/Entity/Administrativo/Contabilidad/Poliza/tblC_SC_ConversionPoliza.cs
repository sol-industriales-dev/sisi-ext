using Core.Entity.Principal.Configuraciones;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblC_SC_ConversionPoliza : RegistroDTO
    {
        public int Id { get; set; }
        public EmpresaEnum PalEmpresa { get; set; }
        public int PalYear { get; set; }
        public int PalMes { get; set; }
        public string PalTP { get; set; }
        public int PalPoliza { get; set; }
        public decimal PalCargo { get; set; }
        public decimal PalAbono { get; set; }
        public EmpresaEnum SecEmpresa { get; set; }
        public int SecYear { get; set; }
        public int SecMes { get; set; }
        public string SecTP { get; set; }
        public int SecPoliza { get; set; }
        public decimal SecCargo { get; set; }
        public decimal SecAbono { get; set; }
    }
}
