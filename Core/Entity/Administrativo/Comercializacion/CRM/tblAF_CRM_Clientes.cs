using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_Clientes
    {
        #region SQL
        public int id { get; set; }
        public string nombreCliente { get; set; }
        public int FK_Division { get; set; }
        public int FK_Municipio { get; set; }
        public string paginaWeb { get; set; }
        public int FK_TipoCliente { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}