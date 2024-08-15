using System;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class CanalDTO
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

        #region ADICIONAL
        public string htmlDivisiones { get; set; }
        #endregion
    }
}