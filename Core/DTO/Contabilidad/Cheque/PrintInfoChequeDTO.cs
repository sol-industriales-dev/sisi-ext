using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class PrintInfoChequeDTO
    {
        public tblC_sb_cheques cheques { get; set; }
        public List<tblC_sc_polizas> polizas { get; set; }
        public List<tblC_sc_movpol> movPolizas { get; set; }

    }
}
