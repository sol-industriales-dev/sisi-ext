using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_Divisiones
    {
        #region SQL
        public int id { get; set; }
        public decimal numDivision { get; set; }
        public string division { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}