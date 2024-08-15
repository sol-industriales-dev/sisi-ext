using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_CatCanales
    {
        #region SQL
        public int id { get; set; }
        public string canal { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}