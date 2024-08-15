using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.ControlPresupuestalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_ControlImpactos
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public string noCuenta { get; set; }
        public string descripcion { get; set; }
        public decimal total12meses { get; set; }
        public decimal promedioMes { get; set; }
        public int idEstrategia { get; set; }
        public int idResponsableLiderCuenta { get; set; }
        public decimal porcAhorroEsperado { get; set; }
        public decimal ahorroEsperadoAnual { get; set; }
        public string posiblesAcciones { get; set; }
        public int estatus { get; set; }
        public string accionesEstablecidas { get; set; }
        public DateTime? fechaImplementacion { get; set; }
        public string pendientes { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
