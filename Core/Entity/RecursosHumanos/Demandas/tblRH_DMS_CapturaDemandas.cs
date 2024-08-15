using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Demandas
{
    public class tblRH_DMS_CapturaDemandas
    {
        #region SQL
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombreDemandante { get; set; }
        public string puesto { get; set; }
        public bool esEmpleadoCP { get; set; }
        public string cc { get; set; }
        public string cc_Descripcion { get; set; }
        public DateTime? fechaIngreso { get; set; }
        public DateTime? fechaBaja { get; set; }
        public string motivoSalida { get; set; }
        public decimal sueldoDiario { get; set; }
        public decimal ofertaInicial { get; set; }
        public string antiguedad { get; set; }
        public DateTime fechaRecibioDemanda { get; set; }
        public DateTime fechaDemanda { get; set; }
        public string numExpediente { get; set; }
        public int FK_Juzgado { get; set; }
        public int FK_TipoDemanda { get; set; }
        public int FK_Estado { get; set; }
        public string demandado { get; set; }
        public decimal salarioDiario { get; set; }
        public string hechos { get; set; }
        public string peticiones { get; set; }
        public string estadoActual { get; set; }
        public DateTime? fechaAudiencia { get; set; }
        public string comentarioFechaAudiencia { get; set; }
        public string abogadoDemandante { get; set; }
        public decimal cuantiaTotal { get; set; }
        public decimal negociadoCerrado { get; set; }
        public decimal finiquitoAl100 { get; set; }
        public decimal diferencia { get; set; }
        public int semaforo { get; set; }
        public int esDemandaCerrada { get; set; }
        public DateTime? fechaCierreDemanda { get; set; }
        public string resolucionLaudo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
