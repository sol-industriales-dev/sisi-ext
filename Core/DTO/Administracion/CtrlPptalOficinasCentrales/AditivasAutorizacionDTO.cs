using Core.Enum.Administracion.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class AditivasAutorizacionDTO
    {
        public int idAditiva { get; set; }
        public int idPresupuesto { get; set; }
        public TipoPresupuestoEnum indicador { get; set; }
        public string agrupacion { get; set; }
        public string descripcion { get; set; }
        public string actividad { get; set; }
        public decimal importeEnero { get; set; }
        public decimal importeFebrero { get; set; }
        public decimal importeMarzo { get; set; }
        public decimal importeAbril { get; set; }
        public decimal importeMayo { get; set; }
        public decimal importeJunio { get; set; }
        public decimal importeJulio { get; set; }
        public decimal importeAgosto { get; set; }
        public decimal importeSeptiembre { get; set; }
        public decimal importeOctubre { get; set; }
        public decimal importeNoviembre { get; set; }
        public decimal importeDiciembre { get; set; }
        public decimal importeTotal { get; set; }
        public bool elUsuarioPuedeAutorizar { get; set; }
        public int autorizantePendiente { get; set; }
        public string comentario { get; set; }
        public string ccString { get; set; }
    }
}
