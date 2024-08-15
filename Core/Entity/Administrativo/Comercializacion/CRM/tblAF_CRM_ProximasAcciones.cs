using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_ProximasAcciones
    {
        #region SQL
        public int id { get; set; }
        public int FK_Proyecto { get; set; }
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
    }
}