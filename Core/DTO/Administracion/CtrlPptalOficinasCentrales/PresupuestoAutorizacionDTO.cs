using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class PresupuestoAutorizacionDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int year { get; set; }
        public string nombrePresupuesto { get; set; }
        public bool estatus { get; set; }
        public bool autorizado { get; set; }
        public bool elUsuarioPuedeAutorizar { get; set; }
        public int autorizantePendiente { get; set; }
    }
}
