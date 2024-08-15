using System;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_Contactos
    {
        #region SQL
        public int id { get; set; }
        public int FK_Cliente { get; set; }
        public string nombreContacto { get; set; }
        public string puesto { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string extension { get; set; }
        public string celular { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}