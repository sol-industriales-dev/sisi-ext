using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class TrackingVentaDTO
    {
        #region SQL
        public int id { get; set; }
        public string nombreProyecto { get; set; }
        public int FK_Cliente { get; set; }
        public int FK_Prioridad { get; set; }
        public int FK_Division { get; set; }
        public int FK_Municipio { get; set; }
        public decimal importeCotizadoAprox { get; set; }
        public DateTime fechaInicio { get; set; }
        public int FK_Estatus { get; set; }
        public int FK_Escenario { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public int FK_Riesgo { get; set; }
        public string descripcionObra { get; set; }
        public bool esProspecto { get; set; }
        public int FK_Canal { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string division { get; set; }
        public string esc { get; set; }
        public string prioridad { get; set; }
        public string nombreCliente { get; set; }
        public string ubicacion { get; set; }
        public string estatusActual { get; set; }
        public string proximaAccion { get; set; }
        public string strFechaProximaAccion { get; set; }
        public string nombreResponsableProyecto { get; set; }
        public string nombreResponsableAccion { get; set; }
        public string riesgo { get; set; }
        public string estatus { get; set; }
        public string porcCumplimiento { get; set; }
        public string go { get; set; }
        public string get { get; set; }
        #endregion
    }
}