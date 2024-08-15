using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_PlanAccion
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public int idConcepto { get; set; }
        public int idMes { get; set; }
        public string planAccion { get; set; }
        public string justificacion { get; set; }
        public DateTime fechaCompromiso { get; set; }
        public string correoResponsableSeguimiento { get; set; }
        public int idEstatusPlanAccion { get; set; }
        public bool vistoBueno { get; set; }
        public int idUsuarioVistoBueno { get; set; }
        public DateTime? fechaVistoBueno { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
