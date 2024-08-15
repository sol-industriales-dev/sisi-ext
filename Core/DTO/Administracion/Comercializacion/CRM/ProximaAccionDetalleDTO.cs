using System;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class ProximaAccionDetalleDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Accion { get; set; }
        public string accion { get; set; }
        public DateTime fechaProximaAccion { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public decimal progreso { get; set; }
        public bool accionFinalizada { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreCompletoResponsable { get; set; }
        public int numAccionDet { get; set; }
        #endregion
    }
}