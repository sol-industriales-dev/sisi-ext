using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class ReportePlanAccionDTO
    {
        #region TABLA SQL
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public int idConcepto { get; set; }
        public string planAccion { get; set; }
        public string justificacion { get; set; }
        public DateTime fechaCompromiso { get; set; }
        public string correoResponsableSeguimiento { get; set; }
        public int idEstatusPlanAccion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONALES
        public string estatus { get; set; }
        public string backgroundColor { get; set; }
        public string color { get; set; }
        public string html { get; set; }
        public List<int> lstCC { get; set; }
        public string concepto { get; set; }
        public string graficaCumplimientoPresupuestoAcumulado { get; set; }
        public string graficaCumplimientoPresupuestoMensual { get; set; }
        public string graficaProyeccion { get; set; }
        public string acumuladoGasto { get; set; }
        public string acumuladoIngreso { get; set; }
        public string acumuladoObjetivo { get; set; }
        public string acumuladoReal { get; set; }
        public string acumuladoCumplimiento { get; set; }
        public string mensualGasto { get; set; }
        public string mensualIngreso { get; set; }
        public string mensualObjetivo { get; set; }
        public string mensualReal { get; set; }
        public string mensualCumplimiento { get; set; }
        public List<byte[]> archivoPDF { get; set; }
        public int idMes { get; set; }
        public string mes { get; set; }
        public int idEmpresa { get; set; }
        #endregion
    }
}
