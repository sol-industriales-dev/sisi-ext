using Core.Entity.Administrativo.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class VMPolizaDTO
    {
        public tblPo_Poliza objPol { get; set; }
        public List<tblPo_MovimientoPoliza> lstMov { get; set; }
    }
}
