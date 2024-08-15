using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptal_GastoIngresoRatio
    {
        public int id { get; set; }
        public int idCC { get; set; }
        public decimal porcentajeGastoIngreso { get; set; }
        public bool registroActivo { get; set; }
    }
}
