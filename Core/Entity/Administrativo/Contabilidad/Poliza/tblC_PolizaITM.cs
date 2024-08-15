using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblC_PolizaITM
    {
        public int id { get; set; }
        public EmpresaEnum PalEmpresa { get; set; }
        public string PalSistema { get; set; }
        public int PaliTm { get; set; }
        public EmpresaEnum SecEmpresa { get; set; }
        public string SecSistema { get; set; }
        public int SeciTm { get; set; }
        public int idUsuarioRegistro { get; set; }        
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }



    }
}
